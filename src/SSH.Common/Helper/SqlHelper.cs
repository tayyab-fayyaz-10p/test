using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SSH.Common.Helper
{
    public static class SqlHelper
    {
        private static int connectionTimeout = -1; // not used yet

        private static int commandTimeout = -1;

        private static object lockObj = new object();

        private static int lastTick;

        /// <summary>
        /// Added by Faisal on 11/22/2015
        /// </summary>
        private static Dictionary<SqlDbType, Func<string, object>> typeMapper = new Dictionary<SqlDbType, Func<string, object>>
        {
            { SqlDbType.BigInt, s => long.Parse(s) },
            { SqlDbType.Bit, s => bool.Parse(s) },
            { SqlDbType.Char, s => s },
            { SqlDbType.Date, s => DateTime.Parse(s) },
            { SqlDbType.DateTime, s => DateTime.Parse(s) },
            { SqlDbType.DateTime2, s => DateTime.Parse(s) },
            { SqlDbType.DateTimeOffset, s => DateTimeOffset.Parse(s) },
            { SqlDbType.Decimal, s => decimal.Parse(s) },
            { SqlDbType.Float, s => double.Parse(s) },
            { SqlDbType.Int, s => int.Parse(s) },
            { SqlDbType.Money, s => decimal.Parse(s) },
            { SqlDbType.NChar, s => s },
            { SqlDbType.NText, s => s },
            { SqlDbType.NVarChar, s => s },
            { SqlDbType.VarChar, s => s },
            { SqlDbType.Real, s => float.Parse(s) },
            { SqlDbType.SmallInt, s => short.Parse(s) },
            { SqlDbType.SmallMoney, s => decimal.Parse(s) },
            { SqlDbType.Text, s => s },
            { SqlDbType.Time, s => TimeSpan.Parse(s) },
            { SqlDbType.TinyInt, s => byte.Parse(s) },
            { SqlDbType.UniqueIdentifier, s => new Guid(s) },
            { SqlDbType.Xml, s => s },
            { SqlDbType.Udt, s => s }
        };

        public static int ConnectionTimeout
        {
            get
            {
                return connectionTimeout;
            }

            set
            {
                connectionTimeout = value;
            }
        }

        public static int CommandTimeout
        {
            get
            {
                return commandTimeout;
            }

            set
            {
                commandTimeout = value;
            }
        }

        public static Dictionary<SqlDbType, Func<string, object>> TypeMapper
        {
            get
            {
                return typeMapper;
            }

            set
            {
                typeMapper = value;
            }
        }

        #region Transaction Processing

        /// <summary>
        /// Starts a transaction on the database specified in the connection string
        /// </summary>
        /// <param name="connectionString">The connection string for the database on which the transaction will be started</param>
        /// <param name="txnh">The TransactionHelper object representing the transaction</param>
        /// <returns>A boolean that indicates the success result of starting the transaction</returns>
        public static bool BeginTransaction(string connectionString, out TransactionHelper txnh)
        {
            try
            {
                // Start a local transaction
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                try
                {
                    txnh = new TransactionHelper(connectionString, conn.BeginTransaction(IsolationLevel.ReadUncommitted));
                }
                catch
                {
                    conn.Close();
                    throw;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                txnh = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Commits a transaction
        /// </summary>
        /// <param name="txnh">The transaction that will be committed</param>
        /// <returns>A boolean that indicates the success result of committing the transaction</returns>
        public static bool CommitTransaction(TransactionHelper txnh)
        {
            if (txnh == null)
            {
                return false;
            }

            try
            {
                // Commit local transaction
                SqlConnection conn = txnh.Transaction.Connection;
                try
                {
                    txnh.Transaction.Commit();
                }
                catch
                {
                    try
                    {
                        txnh.Transaction.Rollback();
                    }
                    catch
                    {
                    }

                    throw;
                }
                finally
                {
                    if (conn != null && conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        /// <param name="txnh">The transaction that will be rolled back</param>
        /// <returns>A boolean that indicates the success result of rolling back the transaction</returns>
        public static bool RollbackTransaction(TransactionHelper txnh)
        {
            if (txnh == null)
            {
                return false;
            }

            try
            {
                // roll back local transaction
                SqlConnection conn = txnh.Transaction.Connection;
                try
                {
                    txnh.Transaction.Rollback();
                }
                finally
                {
                    if (conn != null && conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                return ExecuteNonQuery(cn, commandType, commandText, commandParameters);
            }
        }

        public static int ExecuteNonQuery(string connectionString, string storedProcedureName)
        {
            return ExecuteNonQuery(connectionString, storedProcedureName, null);
        }

        public static int ExecuteNonQuery(string connectionString, string storedProcedureName, object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length == 0)
            {
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, storedProcedureName); // just call the SP without params
            }

            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, storedProcedureName);

            ////assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            ////call the overload that takes an array of SqlParameters
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, storedProcedureName, commandParameters);
        }

        public static DataTable ExecuteDataTable(string connectionString, string storedProcedureName, SqlParameter[] sqlParameters, object[] parameterValues)
        {
            var dataTable = new DataTable();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var cmd = BeginStoredProcedure(sqlConnection, storedProcedureName, sqlParameters, parameterValues))
                {
                    using (var sqlDataAdapter = new SqlDataAdapter(cmd))
                    {
                        sqlDataAdapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            ////pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(connection, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
                LogCall(commandType, commandText, commandParameters);
                int retval = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                LogCall();
                return retval;
            }
        }

        public static int ExecuteNonQuery(SqlConnection connection, string storedProcedureName)
        {
            return ExecuteNonQuery(connection, storedProcedureName, null);
        }

        public static int ExecuteNonQuery(SqlConnection connection, string storedProcedureName, object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length == 0)
            {
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, storedProcedureName);
            }

            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, storedProcedureName);

            ////assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            ////call the overload that takes an array of SqlParameters
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, storedProcedureName, commandParameters);
        }

        public static int ExecuteNonQuery(TransactionHelper txnh, CommandType commandType, string commandText)
        {
            ////pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(txnh, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(TransactionHelper txnh, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (txnh == null)
            {
                throw new ArgumentNullException("txnh");
            }

            LogCall(commandType, commandText, commandParameters);
            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, txnh.Transaction.Connection, txnh.Transaction, commandType, commandText, commandParameters);
                int retval = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                LogCall();
                return retval;
            }
        }

        public static int ExecuteNonQuery(TransactionHelper txnh, string storedProcedureName)
        {
            ////pass through the call providing null for the set of objects
            return ExecuteNonQuery(txnh, storedProcedureName, null);
        }

        public static int ExecuteNonQuery(TransactionHelper txnh, string storedProcedureName, object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length == 0)
            {
                return ExecuteNonQuery(txnh, CommandType.StoredProcedure, storedProcedureName); // just call the SP without params
            }

            if (txnh == null)
            {
                throw new ArgumentNullException("txnh");
            }

            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(txnh.Transaction.Connection.ConnectionString, storedProcedureName);

            AssignParameterValues(commandParameters, parameterValues);

            return ExecuteNonQuery(txnh, CommandType.StoredProcedure, storedProcedureName, commandParameters);
        }

        #region ExecuteDataSet

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            ////pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(connectionString, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            ////create & open a SqlConnection, and dispose of it after we are done.
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                ////call the overload that takes a connection in place of the connection string
                return ExecuteDataset(cn, commandType, commandText, commandParameters);
            }
        }

        public static DataSet ExecuteDataset(string connectionString, string storedProcedureName)
        {
            ////pass through the call providing null for the set of objects
            return ExecuteDataset(connectionString, storedProcedureName, null);
        }

        public static DataSet ExecuteDataset(string connectionString, string storedProcedureName, object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length == 0)
            {
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProcedureName); // just call the SP without params
            }

            ////if we receive parameter values, we need to figure out where they go
            ////pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, storedProcedureName);

            ////assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            ////call the overload that takes an array of SqlParameters
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProcedureName, commandParameters);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            ////pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(connection, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            LogCall(commandType, commandText, commandParameters);

            ////create a command and prepare it for execution
            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);

                ////create the DataAdapter & DataSet
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();

                    ////fill the DataSet using default values for DataTable names, etc.
                    da.Fill(ds);

                    //// detach the SqlParameters from the command object, so they can be used again.
                    cmd.Parameters.Clear();

                    LogCall();

                    ////return the dataset
                    return ds;
                }
            }
        }

        public static DataSet ExecuteDataset(SqlConnection connection, string storedProcedureName)
        {
            ////pass through the call providing null for the set of objects
            return ExecuteDataset(connection, storedProcedureName, null);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, string storedProcedureName, object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length == 0)
            {
                return ExecuteDataset(connection, CommandType.StoredProcedure, storedProcedureName); // just call the SP without params
            }

            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, storedProcedureName);

            AssignParameterValues(commandParameters, parameterValues);

            return ExecuteDataset(connection, CommandType.StoredProcedure, storedProcedureName, commandParameters);
        }

        public static DataSet ExecuteDataset(TransactionHelper txnh, CommandType commandType, string commandText)
        {
            return ExecuteDataset(txnh, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(TransactionHelper txnh, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (txnh == null)
            {
                throw new ArgumentNullException("txnh");
            }

            LogCall(commandType, commandText, commandParameters);

            ////create a command and prepare it for execution
            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, txnh.Transaction.Connection, txnh.Transaction, commandType, commandText, commandParameters);

                ////create the DataAdapter & DataSet
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();

                    ////fill the DataSet using default values for DataTable names, etc.
                    da.Fill(ds);

                    //// detach the SqlParameters from the command object, so they can be used again.
                    cmd.Parameters.Clear();

                    LogCall();

                    ////return the dataset
                    return ds;
                }
            }
        }

        public static DataSet ExecuteDataset(TransactionHelper txnh, string storedProcedureName)
        {
            return ExecuteDataset(txnh, storedProcedureName, null);
        }

        public static DataSet ExecuteDataset(TransactionHelper txnh, string storedProcedureName, object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length == 0)
            {
                return ExecuteDataset(txnh, CommandType.StoredProcedure, storedProcedureName); // just call the SP without params
            }

            if (txnh == null)
            {
                throw new ArgumentNullException("txnh");
            }

            ////if we receive parameter values, we need to figure out where they go
            ////pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(txnh.Transaction.Connection.ConnectionString, storedProcedureName);

            ////assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            ////call the overload that takes an array of SqlParameters
            return ExecuteDataset(txnh, CommandType.StoredProcedure, storedProcedureName, commandParameters);
        }

        #endregion ExecuteDataSet

        #region ExecuteScalar

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            ////pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(connectionString, commandType, commandText, null);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            ////create & open a SqlConnection, and dispose of it after we are done.
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                ////call the overload that takes a connection in place of the connection string
                return ExecuteScalar(cn, commandType, commandText, commandParameters);
            }
        }

        public static object ExecuteScalar(string connectionString, string storedProcedureName)
        {
            ////pass through the call providing null for the set of objects
            return ExecuteScalar(connectionString, storedProcedureName, null);
        }

        public static object ExecuteScalar(string connectionString, string storedProcedureName, object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length == 0)
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, storedProcedureName); // just call the SP without params
            }

            if (parameterValues[0].GetType() != new SqlParameter().GetType())
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, storedProcedureName);

                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, storedProcedureName, commandParameters);
            }
            else
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, storedProcedureName, parameterValues.Select(x => (SqlParameter)x).ToArray());
            }
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            ////pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(connection, commandType, commandText, null);
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            LogCall(commandType, commandText, commandParameters);

            ////create a command and prepare it for execution
            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);

                ////execute the command & return the results
                object retval = cmd.ExecuteScalar();

                //// detach the SqlParameters from the command object, so they can be used again.
                cmd.Parameters.Clear();

                LogCall();

                return retval;
            }
        }

        public static object ExecuteScalar(SqlConnection connection, string storedProcedureName)
        {
            ////pass through the call providing null for the set of objects
            return ExecuteScalar(connection, storedProcedureName, null);
        }

        public static object ExecuteScalar(SqlConnection connection, string storedProcedureName, object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length == 0)
            {
                return ExecuteScalar(connection, CommandType.StoredProcedure, storedProcedureName); // just call the SP without params
            }

            ////pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, storedProcedureName);

            ////assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            ////call the overload that takes an array of SqlParameters
            return ExecuteScalar(connection, CommandType.StoredProcedure, storedProcedureName, commandParameters);
        }

        public static object ExecuteScalar(TransactionHelper txnh, CommandType commandType, string commandText)
        {
            return ExecuteScalar(txnh, commandType, commandText, null);
        }

        public static object ExecuteScalar(TransactionHelper txnh, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (txnh == null)
            {
                throw new ArgumentNullException("txnh");
            }

            LogCall(commandType, commandText, commandParameters);

            ////create a command and prepare it for execution
            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, txnh.Transaction.Connection, txnh.Transaction, commandType, commandText, commandParameters);

                object retval = cmd.ExecuteScalar();

                cmd.Parameters.Clear();

                LogCall();

                return retval;
            }
        }

        public static object ExecuteScalar(TransactionHelper txnh, string storedProcedureName)
        {
            return ExecuteScalar(txnh, storedProcedureName, null);
        }

        public static object ExecuteScalar(TransactionHelper txnh, string storedProcedureName, object[] parameterValues)
        {
            ////if we receive parameter values, we need to figure out where they go
            if (parameterValues == null || parameterValues.Length == 0)
            {
                return ExecuteScalar(txnh, CommandType.StoredProcedure, storedProcedureName); // just call the SP without params
            }

            if (txnh == null)
            {
                throw new ArgumentNullException("txnh");
            }
            ////pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(txnh.Transaction.Connection.ConnectionString, storedProcedureName);

            ////assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            ////call the overload that takes an array of SqlParameters
            return ExecuteScalar(txnh, CommandType.StoredProcedure, storedProcedureName, commandParameters);
        }
        #endregion ExecuteScalar

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (commandParameters == null)
            {
                return;
            }

            foreach (SqlParameter p in commandParameters)
            {
                if (p.Value == null && p.Direction == ParameterDirection.InputOutput)
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            bool isSqlPara = false;
            if (commandParameters == null || parameterValues == null)
            {
                return;
            }

            if (parameterValues[0].GetType() == new SqlParameter().GetType())
            {
                isSqlPara = true;
            }

            if (!isSqlPara)
            {
                if (commandParameters.Length != parameterValues.Length)
                {
                    throw new ArgumentException("Parameter count does not match Parameter Value count.");
                }
            }

            if (commandParameters.Length == 0)
            {
                return;
            }

            for (int i = 0; i < parameterValues.Length; ++i)
            {
                try
                {
                    if (isSqlPara)
                    {
                        var currentParameter = commandParameters.FirstOrDefault(x => x.ParameterName.ToLower() == parameterValues[i].ToString().ToLower());
                        if (currentParameter == null)
                        {
                            throw new ArgumentException(string.Format("Missing parameter {0}", parameterValues[i].ToString()));
                        }

                        if (currentParameter.SqlDbType == SqlDbType.VarBinary)
                        {
                            currentParameter.Value = ((SqlParameter)parameterValues[i]).SqlValue;
                        }
                        else
                        {
                            currentParameter.Value = TypeMapper[currentParameter.SqlDbType](((SqlParameter)parameterValues[i]).SqlValue.ToString());
                        }
                    }
                    else
                    {
                        commandParameters[i].Value = parameterValues[i];
                    }
                }
                catch
                {
                }
            }
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            command.Connection = connection;
            command.CommandTimeout = 0;

            command.CommandText = commandText;

            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            command.CommandType = commandType;

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            if (CommandTimeout >= 0)
            {
                command.CommandTimeout = CommandTimeout;
            }

            return;
        }

        #region Debug Methods

        [Conditional("DEBUG")]
        private static void LogCall()
        {
        }

        [Conditional("DEBUG")]
        private static void LogCall(CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
        }

        private static string GetCommandInfo(string commandText, SqlParameter[] commandParameters)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" EXEC ");
            sb.Append(commandText);
            sb.Append(" ");

            if (commandParameters.Length > 0)
            {
                for (int i = 0; i < commandParameters.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append(commandParameters[i].ParameterName);
                    sb.Append("=");
                    sb.Append(commandParameters[i].SqlValue);
                }
            }

            return sb.ToString();
        }

        #endregion
        private static SqlCommand BeginStoredProcedure(SqlConnection sqlConnection, string storedProcedure, SqlParameter[] parameters, object[] parameterValues)
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();  // connect to the database
                }

                return CreateCommand(storedProcedure, sqlConnection, CommandType.StoredProcedure, parameters, parameterValues, UpdateRowSource.None);
            }
            catch (SqlException e)
            {
                throw new Exception("Unable to connect to the database.", e);
            }
        }

        private static SqlCommand CreateCommand(
            string storedProcedure,
            SqlConnection sqlConnection,
            CommandType commandType,
            SqlParameter[] sqlParameters,
            object[] parameterValues,
            UpdateRowSource updateRowSource)
        {
            var cmd = new SqlCommand(storedProcedure, sqlConnection) { CommandType = commandType, CommandTimeout = 300 };

            if (sqlParameters != null)
            {
                for (var idx = 0; idx < sqlParameters.Length; ++idx)
                {
                    cmd.Parameters.Add(sqlParameters[idx]);

                    if (parameterValues != null && parameterValues.Length > idx && parameterValues[idx] != null)
                    {
                        cmd.Parameters[sqlParameters[idx].ParameterName].Value = parameterValues[idx];
                    }
                }
            }

            cmd.UpdatedRowSource = updateRowSource;
            return cmd;
        }
    }

    public static class SqlHelperParameterCache
    {
        #region variables, and constructors

        private static Dictionary<string, SqlParameter[]> paramCache = new Dictionary<string, SqlParameter[]>();
        private static object lockObj = new object();

        #endregion variables, and constructors

        #region caching functions

        public static void CacheParameterSet(string connectionString, string commandText, SqlParameter[] commandParameters)
        {
            lock (lockObj)
            {
                paramCache[string.Format("{0}:{1}", connectionString, commandText)] = commandParameters;
            }
        }

        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = string.Format("{0}:{1}", connectionString, commandText);
            SqlParameter[] cachedParameters;
            lock (lockObj)
            {
                paramCache.TryGetValue(hashKey, out cachedParameters);
            }

            return cachedParameters != null ? CloneParameters(cachedParameters) : null;
        }

        #endregion caching functions

        #region Parameter Discovery Functions

        public static SqlParameter[] GetSpParameterSet(string connectionString, string storedProcedureName)
        {
            return GetSpParameterSet(connectionString, storedProcedureName, false);
        }

        public static SqlParameter[] GetSpParameterSet(string connectionString, string storedProcedureName, bool includeReturnValueParameter)
        {
            string hashKey = string.Format(!includeReturnValueParameter ? "{0}:{1}" : "{0}:{1}:include ReturnValue Parameter", connectionString, storedProcedureName);
            SqlParameter[] cachedParameters;
            lock (lockObj)
            {
                paramCache.TryGetValue(hashKey, out cachedParameters);
            }

            if (cachedParameters == null)
            {
                cachedParameters = DiscoverSpParameterSet(connectionString, storedProcedureName, includeReturnValueParameter);
                lock (lockObj)
                {
                    paramCache[hashKey] = cachedParameters;
                }
            }

            return cachedParameters != null ? CloneParameters(cachedParameters) : null;
        }

        #endregion Parameter Discovery Functions

        private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string storedProcedureName, bool includeReturnValueParameter)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlCommandBuilder.DeriveParameters(cmd);
                    if (!includeReturnValueParameter)
                    {
                        cmd.Parameters.RemoveAt(0);
                    }

                    SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];
                    cmd.Parameters.CopyTo(discoveredParameters, 0);
                    return discoveredParameters;
                }
            }
        }

        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0; i < originalParameters.Length; ++i)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }
    }

    public sealed class TransactionHelper : IComparable, IComparable<TransactionHelper>, IEquatable<TransactionHelper>
    {
        public readonly string ConnectionString;
        public readonly SqlTransaction Transaction;
        public readonly Guid TransactionID;

        public TransactionHelper(string connectionString, SqlTransaction txn, Guid transactionID)
        {
            this.ConnectionString = connectionString;
            this.Transaction = txn;
            this.TransactionID = transactionID;
        }

        internal TransactionHelper(string connectionString)
            : this(connectionString, null)
        {
        }

        internal TransactionHelper(string connectionString, SqlTransaction txn)
            : this(connectionString, txn, Guid.NewGuid())
        {
        }

        public static bool operator ==(TransactionHelper a, TransactionHelper b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(TransactionHelper a, TransactionHelper b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return false;
            }

            if ((object)a == null || (object)b == null)
            {
                return true;
            }

            return a.CompareTo(b) != 0;
        }

        public static bool operator <(TransactionHelper a, TransactionHelper b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return false;
            }

            if ((object)a == null || (object)b == null)
            {
                return (object)a == null;
            }

            return a.CompareTo(b) < 0;
        }

        public static bool operator >(TransactionHelper a, TransactionHelper b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return false;
            }

            if ((object)a == null || (object)b == null)
            {
                return (object)b == null;
            }

            return a.CompareTo(b) > 0;
        }

        public override int GetHashCode()
        {
            return this.TransactionID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            TransactionHelper other = obj as TransactionHelper;
            if (other == null)
            {
                return false;
            }

            return (this.Transaction == null) == (other.Transaction == null) && this.TransactionID == other.TransactionID;
        }

        public bool Equals(TransactionHelper other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.Transaction == null) == (other.Transaction == null) && this.TransactionID == other.TransactionID;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            TransactionHelper other = obj as TransactionHelper;
            if (other == null)
            {
                throw new ArgumentException(string.Empty, "obj");
            }

            if ((this.Transaction == null) == (other.Transaction == null))
            {
                return this.TransactionID.CompareTo(other.TransactionID);
            }

            return this.Transaction != null ? 1 : -1;
        }

        public int CompareTo(TransactionHelper other)
        {
            if (other == null)
            {
                return 1;
            }

            if ((this.Transaction == null) == (other.Transaction == null))
            {
                return this.TransactionID.CompareTo(other.TransactionID);
            }

            return this.Transaction != null ? 1 : -1;
        }
    }
}
