using System;
namespace NFine.Domain.Entity.SystemManage
{
    public class SysModuleToUserEntity : IEntity<SysModuleToUserEntity>
    {
        public String F_Id { get; set; }
        public String F_ModuleId { get; set; }
        public String F_UserId { get; set; }
        public String F_ModuleType { get; set; }
        public String F_FullName { get; set; }
    }
}