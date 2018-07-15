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
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemManage
{
    public class OrganizeApp
    {
        public string cacheKey = "organizeCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
        private IOrganizeRepository service = new OrganizeRepository();

        public List<OrganizeEntity> GetList()
        {
            var cacheList = cache.GetCache<List<OrganizeEntity>>(cacheKey);
            if (cacheList == null)
            {
                cacheList = service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
                cache.WriteCache<List<OrganizeEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_Organize");
            }
            return cacheList;
        }
        public OrganizeEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.F_Id == keyValue);
                cache.RemoveCache(cacheKey);//移除缓存
            }
        }
        public void SubmitForm(OrganizeEntity organizeEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                organizeEntity.Modify(keyValue);
                service.Update(organizeEntity);
            }
            else
            {
                organizeEntity.Create();
                service.Insert(organizeEntity);
            }
            cache.RemoveCache(cacheKey);//移除缓存
        }
    }
}
