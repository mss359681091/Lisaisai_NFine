using System;
namespace NFine.Domain.Entity
{
    /// <summary>
    /// SysBackupServer Entity Model
    /// </summary>
    public class SysBackupServerEntity : IEntity<SysBackupServerEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public String F_Id { get; set; }
        public String F_FullName { get; set; }
        public String F_FtpServerIp { get; set; }
        public String F_FtpUserId { get; set; }
        public String F_FtpPassword { get; set; }
        public Int32? F_SortCode { get; set; }
        public Boolean? F_DeleteMark { get; set; }
        public Boolean? F_EnabledMark { get; set; }
        public String F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public String F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public String F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public String F_DeleteUserId { get; set; }
    }
}