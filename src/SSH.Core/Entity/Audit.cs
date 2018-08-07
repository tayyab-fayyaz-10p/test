using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Recipe.Core.Base.Interface;

namespace SSH.Core.Entity
{
    public class Audit : IBase<int>
    {
        public Audit()
        {
            this.AuditDetails = new List<AuditDetail>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string TableName { get; set; }

        [MaxLength(50)]
        public string ReferenceId { get; set; }

        public DateTime OperationTime { get; set; }

        public int OperationType { get; set; }

        [MaxLength(200)]
        public string Message { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public virtual IList<AuditDetail> AuditDetails { get; set; }
    }
}
