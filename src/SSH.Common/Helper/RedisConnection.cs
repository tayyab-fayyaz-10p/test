using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;

namespace SSH.Common.Helper
{
    public class RedisConnection
    {
        private static string redisConnection = string.IsNullOrEmpty(ConfigurationManager.AppSettings["RedisConnection"]) ? string.Empty : ConfigurationManager.AppSettings["RedisConnection"].ToString();

        static RedisConnection()
        {
            RedisConnection.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(redisConnection);
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
