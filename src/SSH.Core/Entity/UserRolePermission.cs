using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Recipe.Core.Base.Abstract;

namespace SSH.Core.Entity
{
    public class UserRolePermission : EntityBase<int>
    {
        public UserRolePermission()
        {
        }

        [ForeignKey("RoleID")]
        public ApplicationRole Role { get; set; }

        [Required]
        public string RoleID { get; set; }

        [ForeignKey("PermissionID")]
        public Permission Permission { get; set; }

        [Required]
        public int PermissionID { get; set; }
    }
}
