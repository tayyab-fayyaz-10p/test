using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.DTO
{
    public class ChangeLOVStatusDTO
    {
        public bool IsActive { get; set; }

        public List<LOVIdDTO> Ids { get; set; }
    }
}
