using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Core.DTO;
using SSH.Core.Entity;

namespace SSH.Core.IRepository
{
    public interface IOTPRepository : IRepository<OTP, int>
    {
        OTP GetByPhoneNumberOrEmail(string phoneNumber, string emailId);

        Task<int> DeleteExisting(OTPDTO dtoObject);

        Task<OTP> CreateOtp(OTP otp);

        bool Exist(OTPDTO otp);

        OTP CheckExpiry(OTPDTO otp);

        Task<OTP> UpdateOtp(OTP otp);

        Task<bool> VerifyOtp(OTP otp);
    }
}
