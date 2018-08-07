using System.Collections.Generic;
using Recipe.Core.Base.Abstract;
using Recipe.Core.Base.Interface;
using Recipe.Core.Enum;
using SSH.Common.Helper;
using SSH.Core.Constant;
using SSH.Core.Entity;

namespace SSH.Core.DTO
{
    public class AuditDTO : IBase<int>
    {
        public int Id { get; set; }

        public string TableName { get; set; }

        public string ReferenceId { get; set; }

        public int Operation { get; set; }

        public string Action { get; set; }

        public UserDTO User { get; set; }

        public string OperationTime { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string OldData { get; set; }

        public string NewData { get; set; }

        public string Message { get; set; }

        public static List<LOVDTO> ConvertEntityListToDTOList(IEnumerable<LOV> entityList)
        {
            var result = new List<LOVDTO>();
            if (entityList != null)
            {
                foreach (var entity in entityList)
                {
                    var dto = new LOVDTO();
                    dto.ConvertFromEntity(entity);
                    result.Add(dto);
                }
            }

            return result;
        }

        public void ConvertFromEntity(Audit entity)
        {
            this.Id = entity.Id;
            this.ReferenceId = entity.ReferenceId;
            this.Operation = entity.OperationType;
            this.Action = (OperationType)entity.OperationType == OperationType.Authorization ? entity.Message : ((OperationType)entity.OperationType).ToString();
            this.OperationTime = entity.OperationTime.GetISOStandardDateTime();
            this.TableName = entity.TableName;
            this.Date = entity.OperationTime.ToString(Validations.DateFormat);
            this.Time = entity.OperationTime.ToString(Validations.TimeFormat);
            this.Message = entity.Message;

            if (entity.User != null)
            {
                this.User = new UserDTO(entity.User);
            }

            if (entity.AuditDetails.Count > 0)
            {
                var details = entity.AuditDetails[0];
                this.NewData = details.AuditData;
            }
        }
    }
}
