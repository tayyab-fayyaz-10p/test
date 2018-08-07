using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe.Core.Base.Abstract;

namespace SSH.Core.Entity
{
    public class OTP : EntityBase<int>
    {
        public string PhoneNumber { get; set; }

        public string OneTimePine { get; set; }

        public DateTime Expiry { get; set; }
    }
}
