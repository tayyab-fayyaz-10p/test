using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe.Core.Base.Abstract;
using SSH.Core.Entity;

namespace SSH.Core.DTO
{
    public class RoleDTO : DTO<ApplicationRole, string>
    {
        public string Name { get; set; }

        public List<PermissionDTO> Permission { get; set; }

        public override void ConvertFromEntity(ApplicationRole entity)
        {
            base.ConvertFromEntity(entity);

            this.Id = entity.Id;
            this.Name = entity.Name;
            if (entity.UserRolePermission != null)
            {
                if (entity.UserRolePermission.Count > 0)
                {
                    this.Permission = new List<PermissionDTO>();
                    foreach (var item in entity.UserRolePermission)
                    {
                        this.Permission.Add(new PermissionDTO { Id = item.PermissionID, Name = item.Permission.Name });
                    }
                }
            }
        }
    }
}
