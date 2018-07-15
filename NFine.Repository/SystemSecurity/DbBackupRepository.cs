
using NFine.Code;
using NFine.Data;
using NFine.Data.Extensions;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Domain.IRepository.SystemSecurity;
using NFine.Domain.ViewModel;
using System.Collections.Generic;
using System.Text;

namespace NFine.Repository.SystemSecurity
{
    public class DbBackupRepository : RepositoryBase<DbBackupEntity>, IDbBackupRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                var dbBackupEntity = db.FindEntity<DbBackupEntity>(keyValue);
                if (dbBackupEntity != null)
                {
                    FileHelper.DeleteFile(dbBackupEntity.F_FilePath);
                }
                db.Delete<DbBackupEntity>(dbBackupEntity);
                db.Commit();
            }
        }
        public void ExecuteDbBackup(DbBackupEntity dbBackupEntity)
        {
            DbHelper.ExecuteSqlCommand(string.Format("backup database {0} to disk ='{1}'", dbBackupEntity.F_DbName, dbBackupEntity.F_FilePath));
            dbBackupEntity.F_FileSize = FileHelper.ToFileSize(FileHelper.GetFileSize(dbBackupEntity.F_FilePath));
            dbBackupEntity.F_FilePath = "/Resource/DbBackup/" + dbBackupEntity.F_FileName;
            this.Insert(dbBackupEntity);
        }

        public List<DbBackupDto> GetMaxCreatorTime()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select max(F_BackupTime) F_BackupTime from [dbo].[Sys_DbBackup] ");
            var result = new RepositoryBase<DbBackupDto>().FindList(strSql.ToString());
            return result;
        }
        
    }
}
