using System;
using System.Threading.Tasks;
using Recipe.Core.Base.Generic;
using SSH.Common.Helper;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.Service
{
    public class OTPService : Service<IOTPRepository, OTP, OTPDTO, int>, IOTPService
    {
        private IExceptionHelper exceptionHelper;
        private IConfigurationService configurationService;
        private ISSHUnitOfWork unitOfWork;
        private IEmailNotificationService emailService;

        public OTPService(ISSHUnitOfWork unitOfWork, IExceptionHelper exceptionHelper, IConfigurationService configurationService, IEmailNotificationService emailService)
            : base(unitOfWork, unitOfWork.OTPRepository)
        {
            this.exceptionHelper = exceptionHelper;
            this.unitOfWork = unitOfWork;
            this.configurationService = configurationService;
            this.emailService = emailService;
        }

        public async Task<OTPDTO> ResendOtp(OTPDTO dtoObject)
        {
            if (dtoObject != null)
            {
                dtoObject.Expiry = DateTime.UtcNow;
                var otp = this.Repository.CheckExpiry(dtoObject);
                if (otp == null)
                {
                    otp = this.Repository.GetByPhoneNumberOrEmail(dtoObject.PhoneNumber, dtoObject.EmailId);
                    otp.OneTimePine = new CommonHelper().GenerateOTP();
                    otp.Expiry = DateTime.UtcNow.AddMinutes(this.configurationService.OTPExpiry);
                    this.UnitOfWork.DBContext.Set<OTP>().Attach(otp);
                    this.UnitOfWork.DBContext.Entry(otp).Property(x => x.OneTimePine).IsModified = true;
                    this.UnitOfWork.DBContext.Entry(otp).Property(x => x.Expiry).IsModified = true;
                    await this.unitOfWork.SaveAsync();

                    if (!string.IsNullOrEmpty(dtoObject.EmailId))
                    {
                        await this.emailService.SendEmailNotification(dtoObject.EmailId, "Your last OTP has expired, please enter new OTP: " + otp.OneTimePine, Message.OTPSubject);
                    }

                    if (!string.IsNullOrEmpty(dtoObject.PhoneNumber))
                    {
                        await this.emailService.SendSMS(dtoObject.PhoneNumber, "Your OTP is : " + otp.OneTimePine);
                    }
                    
                    return dtoObject;
                }
                else
                {
                    if (!string.IsNullOrEmpty(dtoObject.EmailId))
                    {
                        await this.emailService.SendEmailNotification(dtoObject.EmailId, "Your OTP is : " + otp.OneTimePine, Message.OTPSubject);
                    }

                    if (!string.IsNullOrEmpty(dtoObject.PhoneNumber))
                    {
                        await this.emailService.SendSMS(dtoObject.PhoneNumber, "Your OTP is : " + otp.OneTimePine);
                    }
                }

                return dtoObject;
            }

            this.exceptionHelper.ThrowAPIException(Message.RequiredData);
            return null;
        }

        public async Task<OTPDTO> SendOtp(OTPDTO otpInfo)
        {
            if (otpInfo != null && otpInfo.PhoneNumber != null)
            {
                var user = await this.unitOfWork.UserRepository.GetByEmailOrMobileAsync(otpInfo.EmailId, otpInfo.PhoneNumber);
                if (user != null)
                {
                    this.exceptionHelper.ThrowAPIException("Account already exists please try Forgot Password");
                }

                var entity = otpInfo.ConvertToEntity();
                entity.OneTimePine = new CommonHelper().GenerateOTP();
                entity.Expiry = DateTime.UtcNow.AddMinutes(this.configurationService.OTPExpiry);

                var deleteExisting = await this.Repository.DeleteExisting(otpInfo);
                var result = await this.unitOfWork.OTPRepository.CreateOtp(entity);
                otpInfo.ConvertFromEntity(result);
                await this.emailService.SendSMS(otpInfo.PhoneNumber, "Your OTP is: " + entity.OneTimePine);
                return otpInfo;
            }

            this.exceptionHelper.ThrowAPIException(Message.RequiredData);
            return null;
        }

        public async Task<bool> VerifyOtp(OTPDTO dtoObject)
        {
            dtoObject.Expiry = DateTime.UtcNow;
            var otp = this.Repository.CheckExpiry(dtoObject);
            if (otp == null)
            {
                this.exceptionHelper.ThrowAPIException(Message.OneTimePinExpired);
                return false;
            }

            var result = this.Repository.Exist(dtoObject);
            if (!result)
            {
                this.exceptionHelper.ThrowAPIException(Message.OneTimePinInvalid);
                return false;
            }

            return true;
        }
    }
}