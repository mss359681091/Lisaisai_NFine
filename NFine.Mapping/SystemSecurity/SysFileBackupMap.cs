using NFine.Domain.Entity;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.WebManage
{
    public class SysFileBackupMap : EntityTypeConfiguration<SysFileBackupEntity>
    {
		 public SysFileBackupMap()
        {
            this.ToTable("Sys_FileBackup");
            this.HasKey(t => t.F_Id);
        }
    }
}