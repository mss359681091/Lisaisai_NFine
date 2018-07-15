/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Code;
using NFine.Domain;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;

namespace NFine.Application.SystemManage
{
    public class UserApp
    {
        public string cacheKey = "userCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
        private IUserRepository service = new UserRepository();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();

        public List<UserEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<UserEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Account.Contains(keyword));
                expression = expression.Or(t => t.F_RealName.Contains(keyword));
                expression = expression.Or(t => t.F_MobilePhone.Contains(keyword));
            }
            expression = expression.And(t => t.F_Account != "admin");
            return service.FindList(expression, pagination);
        }
        public List<UserEntity> GetList()
        {
            var expression = ExtLinq.True<UserEntity>();
            string sql = "SELECT * FROM [dbo].[Sys_User] WHERE F_Account != 'admin'";
            return service.FindList(sql);
        }
        public List<UserDto> GetListBySql(string organizeId, string roleId, string dutyId, string departmentId, string keywords)
        {
            return new UserRepository().GetListBySql(organizeId, roleId, dutyId, departmentId, keywords);
        }
        public UserEntity GetUserEntity(string keyword)
        {
            cacheKey = cacheKey + "2_" + keyword;//拼接有参key值
            var cacheEntity = cache.GetCache<UserEntity>(cacheKey);
            if (cacheEntity == null)
            {
                cacheEntity = service.FindEntity(t => t.F_Id == keyword);
                cache.WriteCache<UserEntity>(cacheEntity, cacheKey, "UserCacheDependency", "Sys_User");
            }
            return cacheEntity;
        }
        public UserEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
            }
            else
            {
                userEntity.Create();
            }
            service.SubmitForm(userEntity, userLogOnEntity, keyValue);
        }
        public void UpdateForm(UserEntity userEntity)
        {
            service.Update(userEntity);
        }

        public UserEntity CheckLogin(string username, string password)
        {
            UserEntity userEntity = service.FindEntity(t => t.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    UserLogOnEntity userLogOnEntity = userLogOnApp.GetForm(userEntity.F_Id);
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userLogOnEntity.F_UserPassword)
                    {
                        DateTime lastVisitTime = DateTime.Now;
                        int LogOnCount = (userLogOnEntity.F_LogOnCount).ToInt() + 1;
                        if (userLogOnEntity.F_LastVisitTime != null)
                        {
                            userLogOnEntity.F_PreviousVisitTime = userLogOnEntity.F_LastVisitTime.ToDate();
                        }
                        userLogOnEntity.F_LastVisitTime = lastVisitTime;
                        userLogOnEntity.F_LogOnCount = LogOnCount;
                        userLogOnApp.UpdateForm(userLogOnEntity);
                        return userEntity;
                    }
                    else
                    {
                        throw new Exception("密码不正确，请重新输入");
                    }
                }
                else
                {
                    throw new Exception("账户被系统锁定,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }
        }

        public string GetUserNames(string userIds)
        {
            string result = string.Empty;
            string sql = @"select F_RealName= STUFF( ( SELECT ',' + CAST(F_RealName AS NVARCHAR(MAX))  FROM[dbo].[Sys_User] where F_Id in ("+ userIds + ") FOR XML PATH('')),1,1,'')";
            result = SqlHelper.ExecuteScalarText(sql, null).ToString();
            return result;
        }
    }
}
