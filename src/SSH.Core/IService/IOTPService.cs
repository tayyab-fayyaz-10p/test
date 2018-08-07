using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.IRepository;

namespace SSH.Core.IService
{
    public interface IOTPService : IService<IOTPRepository, OTP, OTPDTO, int>
    {
        Task<OTPDTO> SendOtp(OTPDTO otpDto);

        Task<bool> VerifyOtp(OTPDTO otpDto);

        Task<OTPDTO> ResendOtp(OTPDTO otpDto);
    }
}
