using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSH.Core.Enum;

namespace SSH.Core.DTO
{
    public class ChangeDpStatusDTO
    {
        public DeliveryPartnerStatus Status { get; set; }

        public string Reason { get; set; }

        public bool IsOnline { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public List<int> Id { get; set; }
    }
}
