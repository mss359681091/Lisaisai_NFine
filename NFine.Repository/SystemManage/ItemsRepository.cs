using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;

namespace NFine.Repository.SystemManage
{
    public class ItemsRepository : RepositoryBase<ItemsEntity>, IItemsRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<ItemsEntity>(t => t.F_Id == keyValue);
                db.Delete<ItemsDetailEntity>(t => t.F_ItemId == keyValue);
                db.Commit();
            }
        }
    }
}
