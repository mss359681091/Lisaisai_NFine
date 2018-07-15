/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemManage
{
    public class RoleApp
    {
        public string cacheKey = "roleCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存

        private IRoleRepository service = new RoleRepository();
        private ModuleApp moduleApp = new ModuleApp();
        private ModuleButtonApp moduleButtonApp = new ModuleButtonApp();

        public List<RoleEntity> GetList(string keyword = "")
        {
            cacheKey = cacheKey + "0_" + keyword;//拼接有参key值
            var cacheList = cache.GetCache<List<RoleEntity>>(cacheKey);
            if (cacheList == null)
            {
                var expression = ExtLinq.True<RoleEntity>();
                if (!string.IsNullOrEmpty(keyword))
                {
                    expression = expression.And(t => t.F_FullName.Contains(keyword));
                    expression = expression.Or(t => t.F_EnCode.Contains(keyword));
                  
                }
                expression = expression.And(t => t.F_FullName.Trim() != "超级管理员");
                expression = expression.And(t => t.F_Category == 1);
                cacheList = service.IQueryable(expression).OrderBy(t => t.F_SortCode).ToList();
                cache.WriteCache<List<RoleEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_Role");
            }
            return cacheList;
        }
        public RoleEntity GetForm(string keyValue)
        {
            cacheKey = cacheKey + "2_" + keyValue;//拼接有参key值
            var cacheEntity = cache.GetCache<RoleEntity>(cacheKey);
            if (cacheEntity == null)
            {
                cacheEntity = service.FindEntity(keyValue);
                cache.WriteCache<RoleEntity>(cacheEntity, cacheKey, "UserCacheDependency", "Sys_Role");
            }
            return cacheEntity;
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(RoleEntity roleEntity, string[] permissionIds, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.F_Id = keyValue;
            }
            else
            {
                roleEntity.F_Id = Common.GuId();
            }
            var moduledata = moduleApp.GetList();
            var buttondata = moduleButtonApp.GetList();
            List<RoleAuthorizeEntity> roleAuthorizeEntitys = new List<RoleAuthorizeEntity>();
            foreach (var itemId in permissionIds)
            {
                RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                roleAuthorizeEntity.F_Id = Common.GuId();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = roleEntity.F_Id;
                roleAuthorizeEntity.F_ItemId = itemId;
                if (moduledata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 1;
                }
                if (buttondata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 2;
                }
                roleAuthorizeEntitys.Add(roleAuthorizeEntity);
            }
            service.SubmitForm(roleEntity, roleAuthorizeEntitys, keyValue);

            CacheFactory.Cache().RemoveCache("authorizeurldata_" + roleEntity.F_Id);//删除缓存
        }
    }
}
