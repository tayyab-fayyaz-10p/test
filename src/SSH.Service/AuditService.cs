using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipe.Common.Helper;
using Recipe.Core.Enum;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.Helper;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.Service
{
    public class AuditService : IAuditService
    {
        private ISSHUnitOfWork unitOfWork;

        public AuditService(ISSHUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<AuditDTO>> GetAllAsync()
        {
            var audits = await this.unitOfWork.AuditRepository.GetAll();
            var listDTO = new List<AuditDTO>();

            foreach (var item in audits)
            {
                var dto = new AuditDTO();
                dto.ConvertFromEntity(item);
                listDTO.Add(dto);
            }

            return listDTO;
        }

        public async Task<IList<AuditDTO>> GetAllAsync(JsonApiRequest request)
        {
            var auditList = await this.unitOfWork.AuditRepository.GetAll(request);
            return this.GetAuditList(auditList);
        }

        public async Task<AuditDTO> GetAsync(int id)
        {
            var audit = await this.unitOfWork.AuditRepository.GetAsync(id);
            var dto = new AuditDTO();
            dto.ConvertFromEntity(audit);

            if (audit.OperationType == (int)OperationType.Update)
            {
                var secondLastRecord = await this.unitOfWork.AuditRepository.GetSecondLastRecordAsync(id, audit.ReferenceId, audit.TableName);
                var secondLastRecordDto = new AuditDTO();
                secondLastRecordDto.ConvertFromEntity(secondLastRecord);
                dto.OldData = secondLastRecordDto.NewData;
            }

            return dto;
        }

        public async Task<List<AuditReportDTO>> GetAuditByUserId(string userId, string startDate = null, string endDate = null)
        {
            IEnumerable<Audit> auditList = null;
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                auditList = await this.unitOfWork.AuditRepository.GetAuditByUserId(userId);
            }
            else
            {
                auditList = await this.unitOfWork.AuditRepository.GetAuditByUserIdAndDate(userId, startDate, endDate);
            }

            return this.GetAuditReport(auditList);
        }
        
        public async Task<List<AuditReportDTO>> GetAccessLog(string userId, string startDate = null, string endDate = null)
        {
            IEnumerable<Audit> auditList = null;
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                auditList = await this.unitOfWork.AuditRepository.GetAccessLogByUserId(userId);
            }
            else
            {
                auditList = await this.unitOfWork.AuditRepository.GetAccessLogByUserIdAndDate(userId, startDate, endDate);
            }

            return this.GetAuditReport(auditList);
        }

        public async Task<string> GetAuditReportDownload(string userId, EntityReportType reportType, string startDate = null, string endDate = null)
        {
            var data = await this.GetAuditByUserId(userId, startDate, endDate);
            if (reportType == EntityReportType.CSV)
            {
                return ReportHelper.CreateCSVFile(data, "AuditReport");
            }
            else if (reportType == EntityReportType.PDF)
            {
                return ReportHelper.CreatePDFFile(data, "AuditReport");
            }

            return string.Empty;
        } 

        #region Private Function
        private Audit GetSecondLast(int auditId, string referenceId, string tableName, List<Audit> auditList)
        {
            return auditList.Where(x => x.ReferenceId == referenceId && x.TableName == tableName && x.Id < auditId && (x.OperationType == (int)OperationType.Update || x.OperationType == (int)OperationType.Create)).OrderByDescending(x => x.Id).FirstOrDefault();
        }

        private List<AuditDTO> GetAuditList(IEnumerable<Audit> auditList)
        {
            var result = new List<AuditDTO>();

            foreach (var audit in auditList)
            {
                var dto = new AuditDTO();
                dto.ConvertFromEntity(audit);
                if (audit.OperationType == (int)OperationType.Update)
                {
                    var secondLastRecord = this.GetSecondLast(audit.Id, audit.ReferenceId, audit.TableName, auditList.ToList());
                    if (secondLastRecord != null)
                    {
                        var secondLastRecordDto = new AuditDTO();
                        secondLastRecordDto.ConvertFromEntity(secondLastRecord);
                        dto.OldData = secondLastRecordDto.NewData;
                    }
                }
                else
                {
                    dto.OldData = null;
                }

                result.Add(dto);
            }

            return result;
        }

        private List<AuditReportDTO> GetAuditReport(IEnumerable<Audit> auditList)
        {
            if (auditList != null)
            {
                var auditReportList = new List<AuditReportDTO>();
                foreach (var entity in auditList)
                {
                    if (entity.User != null)
                    {
                        string actionOn = string.Empty;
                        if (entity.TableName.Contains("Repository"))
                        {
                            actionOn = entity.TableName.Replace("Repository", string.Empty);
                            actionOn = actionOn.Replace("I", string.Empty);
                        }
                        else if (entity.TableName.Contains("Service"))
                        {
                            actionOn = entity.TableName.Replace("Service", string.Empty);
                        }

                        auditReportList.Add(new AuditReportDTO
                        {
                            UserName = entity.User.UserName,
                            Action = (OperationType)entity.OperationType == OperationType.Authorization ? entity.Message : ((OperationType)entity.OperationType).ToString()
                            + " - " + actionOn,
                            Date = entity.OperationTime.ToString(Validations.DateFormat),
                            Time = entity.OperationTime.ToString(Validations.TimeFormat)
                        });
                    }
                }

                return auditReportList;
            }

            return null;
        }

        #endregion
    }
}
