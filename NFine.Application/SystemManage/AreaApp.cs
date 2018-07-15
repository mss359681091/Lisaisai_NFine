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
    public class AreaApp
    {
        public string cacheKey = "areaCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
        private IAreaRepository service = new AreaRepository();

        public List<AreaEntity> GetList()
        {
            var cacheList = cache.GetCache<List<AreaEntity>>(cacheKey);
            if (cacheList == null)
            {
                cacheList = service.IQueryable().ToList();
                cache.WriteCache<List<AreaEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_Area");
            }
            return cacheList;
        }
        public AreaEntity GetForm(string keyValue)
        {
            cacheKey = cacheKey + "2_" + keyValue;//拼接有参key值
            var cacheEntity = cache.GetCache<AreaEntity>(cacheKey);
            if (cacheEntity == null)
            {
                cacheEntity = service.FindEntity(keyValue);
                cache.WriteCache<AreaEntity>(cacheEntity, cacheKey, "UserCacheDependency", "Sys_Area");
            }
            return cacheEntity;
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
            }
        }
        public void SubmitForm(AreaEntity areaEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                areaEntity.Modify(keyValue);
                service.Update(areaEntity);
            }
            else
            {
                areaEntity.Create();
                service.Insert(areaEntity);
            }
        }
    }
}
