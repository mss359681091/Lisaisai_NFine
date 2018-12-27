using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
namespace NFine.Repository.SystemManage
{
    public class ModuleRepository : RepositoryBase<ModuleEntity>, IModuleRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<ModuleEntity>(t => t.F_Id == keyValue);
                db.Delete<ModuleButtonEntity>(t => t.F_ModuleId == keyValue);
                db.Commit();
            }
        }
    }
}
