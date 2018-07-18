 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.QuartzManage
{
 
    public class CustomerJobInfoEntity : IEntity<CustomerJobInfoEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string JobGroupName { get; set; }
        public string JobName { get; set; }
        public string TriggerName { get; set; }
        public string Cron { get; set; }
        public string TriggerState { get; set; }
        public DateTime? JobStartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? PreTime { get; set; }
        public DateTime? NextTime { get; set; }
        public string TriggerGroupName { get; set; }
        public string DLLName { get; set; }
        public string FullJobName { get; set; }
        public Boolean? Deleted { get; set; }
        public string Exception { get; set; }
        public string RequestUrl { get; set; }
        public string WebApi { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
    }
}