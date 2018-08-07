using System;
using System.Collections.Generic;
using Recipe.Core.Base.Abstract;
using SSH.Core.Entity;

namespace SSH.Core.DTO
{
    public class UserPaymentSummaryReportDTO
    {
        public double PendingCash { get; set; }

        public double NextPayment { get; set; }

        public string PaymentDate { get; set; }

        public double TodaysEarning { get; set; }

        public List<UserPaymentReportDTO> Payments { get; set; }
    }

    public class UserPaymentReportDTO
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double ProcessedAmount { get; set; }

        public double ApprovedAmount { get; set; }

        public DateTime ProcessedDate { get; set; }

        public string TransactionId { get; set; }

        public int NoOfJobs { get; set; }

        public double Bonus { get; set; }
    }
}
