using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.DTO
{
    public class CompleteJobDTO
    {
        public int Id { get; set; }

        public bool CashCollected { get; set; }

        public double CollectedAmount { get; set; }

        public string Comments { get; set; }
        
        public bool ReceiptByEmail { get; set; }

        public bool ReceiptBySms { get; set; }

        public bool IsAutoComplete { get; set; }

        public List<JobWithItemsDTO> ConnectedJobs { get; set; }
    }
}
