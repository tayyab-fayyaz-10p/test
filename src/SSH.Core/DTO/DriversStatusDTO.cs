using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.DTO
{
    public class DriversStatusDTO
    {
        public int Registered { get; set; }

        public int Online { get; set; }

        public int Free { get; set; }
    }
}
