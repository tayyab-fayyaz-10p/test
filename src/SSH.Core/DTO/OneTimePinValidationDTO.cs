using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSH.Core.Entity;

namespace SSH.Core.DTO
{
    public class OneTimePinValidationDTO
    {
        public bool IsValid { get; set; }

        public ApplicationUser User { get; set; }
    }
}
