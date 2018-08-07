using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe.Core.Base.Abstract;
using SSH.Core.Entity;

namespace SSH.Core.DTO
{
    public class OTPDTO : DTO<OTP, int>
    {
        public string PhoneNumber { get; set; }

        public string EmailId { get; set; }

        public string OneTimePin { get; set; }

        public DateTime Expiry { get; set; }

        public override OTP ConvertToEntity(OTP entity)
        {
            entity = base.ConvertToEntity(entity);
            
            entity.OneTimePine = this.OneTimePin;
            entity.PhoneNumber = this.PhoneNumber;
            entity.Expiry = this.Expiry;

            return entity;
        }

        public override void ConvertFromEntity(OTP entity)
        {
            base.ConvertFromEntity(entity);

            this.OneTimePin = entity.OneTimePine;
            this.PhoneNumber = entity.PhoneNumber;
            this.Expiry = entity.Expiry;
        }
    }    
}
