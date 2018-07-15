
using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class SysModuleToUserMap : EntityTypeConfiguration<SysModuleToUserEntity>
    {
		 public SysModuleToUserMap()
        {
            this.ToTable("Sys_ModuleToUser");
            this.HasKey(t => t.F_Id);
        }
    }
}