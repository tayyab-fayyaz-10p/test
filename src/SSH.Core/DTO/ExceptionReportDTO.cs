using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.DTO
{
    public class ExceptionReportDTO
    {
        public string Type { get; set; }

        public string Message { get; set; }

        public string User { get; set; }

        public DateTime TimeUtc { get; set; }

        public string Date { get; set; }
    }
}
