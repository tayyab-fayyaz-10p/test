using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Recipe.Core.Base.Generic;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;

namespace SSH.Repository
{
    public class OTPRepository : AuditableRepository<OTP, int>, IOTPRepository
    {
        public OTPRepository(ISSHRequestInfo requestInfo)
            : base(requestInfo)
        {
        }

        public async Task<OTP> CreateOtp(OTP otp)
        {
            var entity = this.Create(otp);
            await this.DBContext.SaveChangesAsync();
            return otp;
        }

        public async Task<int> DeleteExisting(OTPDTO dtoObject)
        {
            var entities = await this.DefaultListQuery.Where(x => x.PhoneNumber == dtoObject.PhoneNumber || x.PhoneNumber == dtoObject.EmailId).ToListAsync();
            foreach (var entity in entities)
            {
                await this.DeleteAsync(entity.Id);
            }

            return await this.DBContext.SaveChangesAsync();
        }

        public OTP GetByPhoneNumberOrEmail(string phoneNumber, string emailId)
        {
            return this.DefaultSingleQuery.Where(x => x.PhoneNumber == phoneNumber || x.PhoneNumber == emailId).FirstOrDefault();
        }

        public bool Exist(OTPDTO dtoObject)
        {
            var result = this.DefaultSingleQuery.FirstOrDefault(x => (x.PhoneNumber == dtoObject.PhoneNumber || x.PhoneNumber == dtoObject.EmailId) && x.OneTimePine == dtoObject.OneTimePin);
            return result != null;
        }

        public OTP CheckExpiry(OTPDTO dtoObject)
        {
            var result = this.DefaultSingleQuery.FirstOrDefault(x => (x.PhoneNumber == dtoObject.PhoneNumber || x.PhoneNumber == dtoObject.EmailId) && x.Expiry > dtoObject.Expiry);
            return result;
        }

        public OTP GetOTP(OTP otpInfo)
        {
            return this.DefaultSingleQuery.FirstOrDefault(x => x.PhoneNumber.Trim().ToString() == otpInfo.PhoneNumber.Trim());
        }

        public async Task<OTP> UpdateOtp(OTP otp)
        {
            var ontTimePin = await this.GetAsync(otp.Id);
            ontTimePin.LastModifiedOn = DateTime.UtcNow;
            ontTimePin.Expiry = otp.Expiry;

            otp = await this.Update(ontTimePin);
            await this.DBContext.SaveChangesAsync();

            return otp;
        }

        public Task<bool> VerifyOtp(OTP otp)
        {
            throw new NotImplementedException();
        }
    }
}
