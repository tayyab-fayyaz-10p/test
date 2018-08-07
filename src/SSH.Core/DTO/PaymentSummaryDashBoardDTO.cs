using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.DTO
{
    public class PaymentSummaryDashBoardDTO
    {
        public int NoOfDeliveryPartners { get; set; }

        public string LastPaymentDate { get; set; }

        public double LastDisburseAmount { get; set; }

        public string CurrencyCode { get; set; }
    }
}
