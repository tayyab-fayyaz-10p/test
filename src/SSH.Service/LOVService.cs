using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipe.Common.Helper;
using Recipe.Core.Base.Generic;
using SSH.Common.Helper;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.Service
{
    public class LOVService : Service<ILOVRepository, LOV, LOVDTO, int>, ILOVService
    {
        private ISSHUnitOfWork unitOfWork;
        private IExceptionHelper exceptionHelper;

        public LOVService(ISSHUnitOfWork unitOfWork, IExceptionHelper exceptionHelper) : base(unitOfWork, unitOfWork.LOVRepository)
        {
            this.unitOfWork = unitOfWork;
            this.exceptionHelper = exceptionHelper;
        }

        public async Task<IList<LOVDTO>> GetAllAsync(LOVGroup group)
        {
            var result = await this.unitOfWork.LOVRepository.GetAll();
            result = result
                    .Where(x => x.Group == group.ToString());

            return LOVDTO.ConvertEntityListToDTOList(result);
        }

        public override async Task<LOVDTO> CreateAsync(LOVDTO dtoObject)
        {
            if (!string.IsNullOrEmpty(dtoObject.Key) && !string.IsNullOrEmpty(dtoObject.Value) && !string.IsNullOrEmpty(dtoObject.Group))
            {
                LOV entity = new LOV();
                entity = dtoObject.ConvertToEntity();
                var result = await this.Repository.Exists(entity);
                if (!result)
                {
                    await this.Repository.Create(entity);
                    this.unitOfWork.Save();
                    dtoObject.ConvertFromEntity(entity);
                }
                else
                {
                    this.exceptionHelper.ThrowAPIException(Message.LOVExists);
                }
            }
            else
            {
                this.exceptionHelper.ThrowAPIException(string.Format(Message.InvalidObject, "Data"));
            }

            return dtoObject;
        }

        public override async Task<LOVDTO> UpdateAsync(LOVDTO dtoObject)
        {
            var group = LOVGroup.None;

            if (!Enum.TryParse<LOVGroup>(dtoObject.Group, out group))
            {
                this.exceptionHelper.ThrowAPIException(string.Format(Message.InvalidLOVGroup));
            }

            var entity = await this.Repository.GetAsync(dtoObject.Id);
            entity.Group = dtoObject.Group;
            entity.ImageResourceId = dtoObject.ImageResourceId;
            entity.IsActive = dtoObject.IsActive;
            entity.Key = dtoObject.Key;
            entity.Meta = dtoObject.Meta;
            entity.Value = dtoObject.Value;

            this.UnitOfWork.DBContext.Set<LOV>().Attach(entity);
            this.UnitOfWork.DBContext.Entry(entity).Property(x => x.Group).IsModified = true;
            this.UnitOfWork.DBContext.Entry(entity).Property(x => x.ImageResourceId).IsModified = true;
            this.UnitOfWork.DBContext.Entry(entity).Property(x => x.IsActive).IsModified = true;
            this.UnitOfWork.DBContext.Entry(entity).Property(x => x.Key).IsModified = true;
            this.UnitOfWork.DBContext.Entry(entity).Property(x => x.Meta).IsModified = true;
            this.UnitOfWork.DBContext.Entry(entity).Property(x => x.Value).IsModified = true;

            await this.UnitOfWork.SaveAsync();

            return dtoObject;
        }

        public async Task<int> DeleteAsync(List<LOVIdDTO> dtoList)
        {
            return await this.Repository.Delete(dtoList);
        }

        public async Task<int> UpdateStatusAsync(ChangeLOVStatusDTO dtoObject)
        {           
            var result = await this.Repository.UpdateStatus(dtoObject);          
            return result;
        }

        public async Task<TotalResultDTO<LOVDTO>> GetUpdatedListAsync(JsonApiRequest request, string dateTime = null)
        {
            var lovData = await this.GetAllAsync(request);
            var total = (await this.Repository.GetAll()).Count();
            if (string.IsNullOrEmpty(dateTime))
            {
                return new TotalResultDTO<LOVDTO> { Result = lovData, TotalRecords = total };
            }
            else
            {
                var date = dateTime.GetDate();
                lovData = lovData.Where(x => x.Date >= date).ToList();
                return new TotalResultDTO<LOVDTO> { Result = lovData, TotalRecords = lovData.Count() };
            }
        }

        public override async Task<IList<LOVDTO>> GetAllAsync(JsonApiRequest request)
        {
            var result = await this.Repository.GetAll(request);
            return LOVDTO.ConvertEntityListToDTOList(result);
        }
        
        public async Task<string[]> GetAllGroups()
        {
            var groups = await this.Repository.GetAllGroups();
            return groups.Where(x => !x.Contains(LOVGroup.None.ToString())).ToArray();
        }

        public async Task<IList<LOVDTO>> GetByGroup(string groupName)
        {
            var lovData = await this.GetAllAsync();
            lovData = lovData.Where(x => x.Group == groupName).ToList();
            return lovData;
        }
    }
}
