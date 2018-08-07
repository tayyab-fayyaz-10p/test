using System.ComponentModel.DataAnnotations;
using Recipe.Core.Base.Interface;

namespace SSH.Core.Entity
{
    public class AuditDetail : IBase<int>
    {
        [Key]
        public int Id { get; set; }

        public string AuditData { get; set; }

        public string AuditParameters { get; set; }

        public int AuditId { get; set; }

        public Audit Audit { get; set; }
    }
}
