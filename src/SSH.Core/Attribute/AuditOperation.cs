using System;
using FinanceHouse.Core.Enum;

namespace FinanceHouse.Core.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuditOperation : System.Attribute
    {
        public AuditOperation(OperationType operationType)
        {
            OperationType = operationType;
        }

        public OperationType OperationType { get; set; }
    }
}
