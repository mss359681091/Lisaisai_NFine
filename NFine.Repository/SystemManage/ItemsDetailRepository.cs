using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace NFine.Repository.SystemManage
{
    public class ItemsDetailRepository : RepositoryBase<ItemsDetailEntity>, IItemsDetailRepository
    {
        public List<ItemsDetailEntity> GetItemList(string enCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.*
                            FROM    Sys_ItemsDetail d
                                    INNER  JOIN Sys_Items i ON i.F_Id = d.F_ItemId
                            WHERE   1 = 1
                                    AND i.F_EnCode = @enCode
                                    AND (d.F_EnabledMark = 1 or d.F_EnabledMark is null)
                                    AND (d.F_DeleteMark = 0 or d.F_DeleteMark is null)
                            ORDER BY d.F_SortCode ASC");
            DbParameter[] parameter = 
            {
                 new SqlParameter("@enCode",enCode)
            };
            return this.FindList(strSql.ToString(), parameter);
        }

        public List<ItemsDetailEntity> GetItemByItemCode(string itemCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.*
                            FROM    Sys_ItemsDetail d
                            WHERE   1 = 1
                                    AND  d.F_ItemCode = @ItemCode
                                    AND (d.F_EnabledMark = 1 or d.F_EnabledMark is null)
                                    AND (d.F_DeleteMark = 0 or d.F_DeleteMark is null)");
            DbParameter[] parameter =
            {
                 new SqlParameter("@ItemCode",itemCode)
            };
            return this.FindList(strSql.ToString(), parameter);
        }
    }
}
