using NFine.Domain.Entity;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.WebManage
{
    public class SysBackupServerMap : EntityTypeConfiguration<SysBackupServerEntity>
    {
		 public SysBackupServerMap()
        {
            this.ToTable("Sys_BackupServer");
            this.HasKey(t => t.F_Id);
        }
    }
}