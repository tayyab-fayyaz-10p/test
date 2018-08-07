using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.DTO
{
    public class LocationDTO
    {
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string LocationDetails { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public double CashValue { get; set; }

        public double CollectedAmount { get; set; }

        public bool DocumentRequired { get; set; }

        public string Instructions { get; set; }
    }
}
