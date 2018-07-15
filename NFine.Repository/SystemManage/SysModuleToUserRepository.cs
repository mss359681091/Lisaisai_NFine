using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using System.Collections.Generic;
using System.Text;

namespace NFine.Repository.SystemManage
{
    public class SysModuleToUserRepository : RepositoryBase<SysModuleToUserEntity>, ISysModuleToUserRepository
    {
        public List<SysModuleToUserEntity> GetList(string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM [dbo].[Sys_ModuleToUser]");
            if (!userId.IsEmpty()) {
                strSql.Append(@" WHERE F_UserId = '" + userId + @"'");
            }
            var result = new RepositoryBase<SysModuleToUserEntity>().FindList(strSql.ToString());
            return result;
        }
    }
}