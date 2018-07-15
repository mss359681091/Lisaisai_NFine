using NFine.Code;
using NFine.Data;
using NFine.Domain;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace NFine.Repository.SystemManage
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<UserEntity>(t => t.F_Id == keyValue);
                db.Delete<UserLogOnEntity>(t => t.F_UserId == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(userEntity);
                }
                else
                {
                    userLogOnEntity.F_Id = userEntity.F_Id;
                    userLogOnEntity.F_UserId = userEntity.F_Id;
                    userLogOnEntity.F_UserSecretkey = Md5.md5(Common.CreateNo(), 16).ToLower();
                    userLogOnEntity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userLogOnEntity.F_UserPassword, 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    db.Insert(userEntity);
                    db.Insert(userLogOnEntity);
                }
                db.Commit();
            }
        }

        public List<UserDto> GetListBySql(string organizeId, string roleId, string dutyId, string departmentId, string keywords)
        {
            string strsql = @" select t1.F_Id as UserId,
                t1.F_RealName,
                t1.F_HeadIcon,
                t1.F_Gender,
                t1.F_MobilePhone,
                t2.F_FullName as DutyName,
                t3.F_FullName as RoleName,
                t4.F_FullName as OrganizeName,
                t5.F_FullName as DepartmentName

                from [dbo].[Sys_User] t1 
                left join [dbo].[Sys_Role] t2 --岗位
                on t1.F_DutyId=t2.F_Id
                
                left join [dbo].[Sys_Role] t3 --角色
                on t1.F_RoleId=t3.F_Id
                
                left join  [dbo].[Sys_Organize] t4 --组织
                on t1.F_OrganizeId=t4.F_Id
                
                left join  [dbo].[Sys_Organize] t5 --部门
                on t1.F_DepartmentId=t5.F_Id
                
                where 1=1 
                and ( t1.F_EnabledMark  is null or t1.F_EnabledMark ='True' )  
                and ( t1.F_DeleteMark  is null or t1.F_DeleteMark ='False' ) ";

            DbParameter[] parameter = new DbParameter[]
            {
                new SqlParameter("@F_OrganizeId", organizeId),new SqlParameter("@F_RoleId", roleId), new SqlParameter("@F_DepartmentId", departmentId),new SqlParameter("@F_DutyId", dutyId),new SqlParameter("@keywords", keywords)
            };

            if (!string.IsNullOrEmpty(organizeId))
            {
                strsql += " and  t1.F_OrganizeId =@F_OrganizeId ";
            }
            if (!string.IsNullOrEmpty(roleId))
            {
                strsql += " and  t1.F_RoleId =@F_RoleId ";
            }

            if (!string.IsNullOrEmpty(departmentId))
            {
                strsql += " and ( t1.F_DepartmentId =@F_DepartmentId or t1.F_OrganizeId = @F_DepartmentId)";
            }

            if (!string.IsNullOrEmpty(dutyId))
            {
                strsql += " and  t1.F_DutyId =@F_DutyId ";
            }

            if (!string.IsNullOrEmpty(keywords))
            {
                strsql += "  and ( t1.F_RealName LIKE  '%'+ @keywords +'%'  or t1.F_Account LIKE  '%'+ @keywords +'%' or t1.F_NickName LIKE  '%'+ @keywords +'%'  )";
            }

            var result = new RepositoryBase<UserDto>().FindList(strsql, parameter);
            return result;
        }
    }
}
