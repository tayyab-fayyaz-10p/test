using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe.Core.Base.Generic;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.Service
{
    public class PushNotificationService : Service<IPushNotificationRepository, PushNotification, PushNotificationDTO, int>, IPushNotificationService
    {
        private ISSHUnitOfWork unitOfWork;
        private IExceptionHelper exceptionHelper;
        private ISSHRequestInfo requestInfo;

        public PushNotificationService(ISSHUnitOfWork unitOfWork, IExceptionHelper exceptionHelper, ISSHRequestInfo requestInfo)
            : base(unitOfWork, unitOfWork.PushNotificationRepository)
        {
            this.unitOfWork = unitOfWork;
            this.exceptionHelper = exceptionHelper;
            this.requestInfo = requestInfo;
        }

        public override async Task<IList<PushNotificationDTO>> CreateAsync(IList<PushNotificationDTO> dtoObjects)
        {
            foreach (var item in dtoObjects)
            {
                item.IsRead = false;
            }

            return await base.CreateAsync(dtoObjects);
        }

        public override async Task<PushNotificationDTO> CreateAsync(PushNotificationDTO dtoObject)
        {
            dtoObject.IsRead = false;
            return await base.CreateAsync(dtoObject);
        }

        public override async Task<PushNotificationDTO> UpdateAsync(PushNotificationDTO dtoObject)
        {
            var pushNotificationEntity = await this.Repository.GetAsync(dtoObject.Id);
            pushNotificationEntity.IsRead = true;

            this.UnitOfWork.DBContext.Set<PushNotification>().Attach(pushNotificationEntity);
            this.UnitOfWork.DBContext.Entry(pushNotificationEntity).Property(x => x.IsRead).IsModified = true;

            await this.UnitOfWork.DBContext.SaveChangesAsync();
            return dtoObject;
        }

        public async Task<IList<PushNotificationDTO>> GetAllByUserId()
        {
            var result = await this.Repository.GetAllByUserId(this.requestInfo.UserId);
            return PushNotificationDTO.ConvertEntityListToDTOList<PushNotificationDTO>(result);
        }

        public override async Task<IList<PushNotificationDTO>> UpdateAsync(IList<PushNotificationDTO> dtoObjects)
        {
            foreach (var item in dtoObjects)
            {
                var pushNotificationEntity = await this.Repository.GetAsync(item.Id);
                pushNotificationEntity.IsRead = item.IsRead;

                this.UnitOfWork.DBContext.Set<PushNotification>().Attach(pushNotificationEntity);
                this.UnitOfWork.DBContext.Entry(pushNotificationEntity).Property(x => x.IsRead).IsModified = true;
            }

            await this.UnitOfWork.DBContext.SaveChangesAsync();
            return dtoObjects;
        }

        public async Task<bool> ReadAllPushNotifications()
        {
            var result = await this.Repository.GetAllByUserId(this.requestInfo.UserId);
            if (result != null && result.Count > 0)
            {
                foreach (var notification in result)
                {
                    notification.IsRead = true;
                    this.UnitOfWork.DBContext.Set<PushNotification>().Attach(notification);
                    this.UnitOfWork.DBContext.Entry(notification).Property(x => x.IsRead).IsModified = true;
                }

                await this.UnitOfWork.DBContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<int> GetNotificatinoCount()
        {
            var result = await this.Repository.GetCountByUserId(this.requestInfo.UserId);
            return result;
        }
    }
}
