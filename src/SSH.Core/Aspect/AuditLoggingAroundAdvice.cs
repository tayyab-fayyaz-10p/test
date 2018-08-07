using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using Newtonsoft.Json;
using Recipe.Core.Attribute;
using Recipe.Core.Base.Interface;
using Recipe.Core.Enum;
using SSH.Common.Helper;
using SSH.Core.Entity;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;

namespace SSH.Core.Aspect
{
    public class AuditLoggingAroundAdvice : IInterceptionBehavior
    {
        private ISSHRequestInfo requestInfo;
        private ISSHUnitOfWork unitOfWork;
        private IUserRepository userRepository;

        public AuditLoggingAroundAdvice(ISSHRequestInfo requestInfo, ISSHUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            this.requestInfo = requestInfo;
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }

        public bool WillExecute
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            bool hasExecuted = false;

            foreach (var attribute in input.MethodBase.GetCustomAttributes(true))
            {
                AuditOperation auditOperation = attribute as AuditOperation;

                if (auditOperation != null && !hasExecuted)
                {
                    if (input.Arguments != null)
                    {
                        var tableName = input.Target.GetType();

                        Audit audit = new Audit();
                        audit.OperationTime = DateTime.UtcNow;
                        audit.OperationType = (int)auditOperation.OperationType;
                        audit.UserId = this.requestInfo.UserId;
                        audit.TableName = tableName.BaseType.GetGenericArguments().Length > 0 ? tableName.BaseType.GetGenericArguments()[0].Name : tableName.Name;

                        foreach (var argument in input.Arguments)
                        {
                            if (auditOperation.OperationType == OperationType.Create || auditOperation.OperationType == OperationType.Update)
                            {
                                if (argument is IBase<string>)
                                {
                                    audit.ReferenceId = ((IBase<string>)argument).Id;
                                }
                                else if (argument is IBase<int>)
                                {
                                    audit.ReferenceId = ((IBase<int>)argument).Id.ToString();
                                }

                                AuditDetail auditDetail = new AuditDetail();

                                using (StringWriter writer = new StringWriter())
                                {
                                    using (CustomJsonTextWriter jsonWriter = new CustomJsonTextWriter(writer))
                                    {
                                        Func<bool> include = () => jsonWriter.CurrentDepth <= 2;
                                        var resolver = new CustomContractResolver(include);
                                        var serializer = new JsonSerializer { ContractResolver = resolver, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                                        serializer.Serialize(jsonWriter, argument);
                                    }

                                    auditDetail.AuditData = writer.ToString();
                                }

                                audit.AuditDetails.Add(auditDetail);
                            }

                            if (auditOperation.OperationType == OperationType.Delete)
                            {
                                if (argument != null && argument.ToString() != "SSH.Common.Helper.JsonApiRequest")
                                {
                                    audit.ReferenceId = argument.ToString();
                                }
                            }

                            if (auditOperation.OperationType == OperationType.Authorization)
                            {
                                string message = null;
                                if (argument != null && argument.ToString() == "SSH.Core.DTO.AuthDTO")
                                {
                                    var email = ((DTO.AuthDTO)argument).EmailId;
                                    var user = Task.Run(() => this.userRepository.FindByEmailAsync(email));
                                    user.Wait();
                                    var r = user.Result;
                                    message = "Login";
                                    if (r == null)
                                    {
                                        hasExecuted = true;
                                        var outputVal = getNext()(input, getNext);
                                        return outputVal;
                                    }

                                    audit.UserId = r.Id;
                                }
                                else
                                {
                                    message = "Logout";
                                }

                                audit.Message = message;
                            }
                        }

                        this.unitOfWork.AuditRepository.Create(audit);
                        this.unitOfWork.Save();
                        hasExecuted = true;
                    }
                }
            }

            var output = getNext()(input, getNext);
            return output;
        }
    }
}
