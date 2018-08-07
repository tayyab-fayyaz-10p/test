using System.Collections.Generic;
using System.Linq;
using SSH.Core.Entity;
using SSH.Core.Enum;

namespace SSH.Core.Constant
{
    public class RoleMapping
    {
        public ApplicationRole Role { get; set; }

        public List<ApplicationRole> Roles { get; set; }

        private static List<RoleMapping> All
        {
            get
            {
                var roleMapping = new List<RoleMapping>
                {
                    new RoleMapping
                    {
                        Role = new ApplicationRole { Id = SSH.Core.Constant.Roles.GetRoleId(UserRoles.Admin), Name = UserRoles.Admin.ToString() },
                        Roles = new List<ApplicationRole>
                        {
                            new ApplicationRole { Id = SSH.Core.Constant.Roles.GetRoleId(UserRoles.Accounts), Name = UserRoles.Accounts.ToString() },
                            new ApplicationRole { Id = SSH.Core.Constant.Roles.GetRoleId(UserRoles.HR), Name = UserRoles.HR.ToString() },
                            new ApplicationRole { Id = SSH.Core.Constant.Roles.GetRoleId(UserRoles.InformationDesk), Name = UserRoles.InformationDesk.ToString() }
                        }
                    },
                    new RoleMapping
                    {
                        Role = new ApplicationRole { Name = UserRoles.HR.ToString() },
                        Roles = new List<ApplicationRole>
                        {
                            new ApplicationRole { Id = SSH.Core.Constant.Roles.GetRoleId(UserRoles.Reception), Name = UserRoles.Reception.ToString() }
                        }
                    }
                };

                return roleMapping;
            }
        }

        public static RoleMapping GetMapping(UserRoles role)
        {
            var roleMapping = All.FirstOrDefault(x => x.Role.Name == role.ToString());
            return roleMapping;
        }
    }
}
