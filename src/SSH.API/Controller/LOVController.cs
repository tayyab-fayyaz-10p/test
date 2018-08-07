using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Recipe.Common.Helper;
using SSH.API.Infrastructure;
using SSH.Common.Attribute;
using SSH.Core.Attribute;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Helper;
using SSH.Core.IService;

namespace SSH.API.Controller
{
    [CustomAuthorize]
    [RoutePrefix("LOV")]
    public class LOVController : Recipe.Core.Base.Generic.Controller<ILOVService, LOVDTO, LOV, int>
    {
        private ILOVService lovService;

        public LOVController(ILOVService lovService)
            : base(lovService)
        {
            this.lovService = lovService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ResponseMessageResult> Create(LOVDTO dtoObject)
        {
            var result = await this.lovService.CreateAsync(dtoObject);
            return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ResponseMessageResult> Update(LOVDTO dtoObject)
        {
            var result = await this.lovService.UpdateAsync(dtoObject);
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
            }

            return await this.JsonApiNoContentAsync(Message.DataUpdateFailed, null);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<ResponseMessageResult> Delete(List<LOVIdDTO> dtoList)
        {
            var result = await this.lovService.DeleteAsync(dtoList);

            foreach (var dtoObject in dtoList)
            {
                var lovDTO = await this.Service.GetAsync(dtoObject.Id);
                await BlobStorageHelper.DeleteBlobAsync(lovDTO.ImageResourceId.ToString());
            }

            if (result > 0)
            {
                return await this.JsonApiSuccessAsync(string.Format(Message.RecordsDeleted, result), null);
            }

            return await this.JsonApiBadRequestAsync(Message.DataDeleteFailed, null);
        }

        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<ResponseMessageResult> UpdateStatus(ChangeLOVStatusDTO dtoObject)
        {
            var result = await this.lovService.UpdateStatusAsync(dtoObject);
            if (result > 0)
            {
                return await this.JsonApiSuccessAsync(string.Format(Message.RecordsUpdated, result), null);
            }

            return await this.JsonApiNoContentAsync(Message.DataUpdateFailed, null);
        }

        [HttpGet]
        [Route("GetAll")]
        [AllowAnonymous]
        [AuthorizeAnonymousApi]
        public async Task<ResponseMessageResult> GetAll(string dateTime = null)
        {
            var request = Request.GetJsonApiRequest();
            var result = await this.lovService.GetUpdatedListAsync(request, dateTime);
            return await this.JsonApiSuccessAsync(Message.SuccessResult, result, System.DateTime.UtcNow.ToString(Validations.GenericDateTimeFormat));
        }

        [HttpGet]
        [Route("~/api/App/Lov/GetAll")]
        [AllowAnonymous]
        [AuthorizeAnonymousApi]
        public async Task<ResponseMessageResult> GetAllForApp(string dateTime = null)
        {
            var request = Request.GetJsonApiRequest();
            var result = await this.lovService.GetUpdatedListAsync(request, dateTime);
            return await this.JsonApiSuccessAsync(Message.SuccessResult, result, System.DateTime.UtcNow.ToString(Validations.GenericDateTimeFormat));
        }

        [HttpGet]
        [Route("GetAllGroups")]
        public async Task<ResponseMessageResult> GetAllGroups()
        {
            var result = await this.lovService.GetAllGroups();
            return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
        }

        [HttpGet]
        [Route("GetByGroup")]
        public async Task<ResponseMessageResult> GetByGroup(string groupName)
        {
            var result = await this.lovService.GetByGroup(groupName);
            return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
        }

        [HttpGet]
        [Route("~/api/App/Lov/GetByGroup")]
        public async Task<ResponseMessageResult> GetByGroupForApp(string groupName)
        {
            var result = await this.lovService.GetByGroup(groupName);
            return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<ResponseMessageResult> Get(int id)
        {
            var lov = await this.Service.GetAsync(id);
            if (lov != null)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, lov);
            }

            return await this.JsonApiNotFoundAsync(string.Format(Message.NotFound, "LOV"), null);
        }
    }
}
