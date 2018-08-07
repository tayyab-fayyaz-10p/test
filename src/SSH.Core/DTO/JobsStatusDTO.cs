using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.DTO
{
    public class JobsStatusDTO
    {
        public int Created { get; set; }

        public int Accepted { get; set; }

        public int Orphan { get; set; }

        public int Abandoned { get; set; }

        public int Completed { get; set; }

        public int Closed { get; set; }

        public int InProgress { get; set; }

        public int Exceptions { get; set; }
    }
}
