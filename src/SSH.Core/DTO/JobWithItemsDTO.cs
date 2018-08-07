using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSH.Core.Enum;

namespace SSH.Core.DTO
{
    public class JobWithItemsDTO
    {
        public int Id { get; set; }
        
        public JobStatus JobStatus { get; set; }

        public string ExceptionType { get; set; }
    }
}
