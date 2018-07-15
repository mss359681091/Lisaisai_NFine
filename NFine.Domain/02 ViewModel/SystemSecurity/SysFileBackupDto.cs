using System;
namespace NFine.Domain.ViewModel
{

    public class SysFileBackupDto
    {
        public String F_Id { get; set; }
        public String F_ServerIp { get; set; }
        public String F_BackupName { get; set; }
        public String F_BackupPath { get; set; }
        public String F_FileName { get; set; }
        public String F_FileSize { get; set; }
        public String F_BackupType { get; set; }
        public String F_BackupCategory { get; set; }
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

        public String ServerName { get; set; }
        public int records { get; set; }
    }
}