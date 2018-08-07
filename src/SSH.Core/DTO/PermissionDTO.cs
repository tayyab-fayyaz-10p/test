using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Recipe.Core.Base.Abstract;
using SSH.Core.Entity;

namespace SSH.Core.DTO
{
    public class PermissionDTO : DTO<Permission, int>
    {
        public string Name { get; set; }

        [JsonIgnore]
        public string Group { get; set; }

        public static List<PermissionDTO> ConvertEntityListToDTOList(IEnumerable<Permission> permissions)
        {
            var result = new List<PermissionDTO>();
            if (permissions != null)
            {
                foreach (var entity in permissions)
                {
                    var dto = new PermissionDTO();
                    dto.ConvertFromEntity(entity);
                    result.Add(dto);
                }
            }

            return result;
        }

        public override void ConvertFromEntity(Permission permission)
        {
            base.ConvertFromEntity(permission);

            this.Id = permission.Id;
            this.Name = permission.Name;
            this.Group = permission.Group;
        }
    }
}