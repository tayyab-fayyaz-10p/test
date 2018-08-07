using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSH.Core.Enum;

namespace SSH.Core.DTO
{
    public class LdapAuthRolesDTO
    {
        public bool IsExists { get; set; }

        public UserRoles UserRole { get; set; }
    }
}
