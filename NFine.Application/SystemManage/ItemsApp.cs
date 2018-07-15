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
    public class ItemsApp
    {
        public string cacheKey = "itemsCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
        private IItemsRepository service = new ItemsRepository();

        public List<ItemsEntity> GetList()
        {
            var cacheList = cache.GetCache<List<ItemsEntity>>(cacheKey);
            if (cacheList == null)
            {
                var expression = ExtLinq.True<ItemsEntity>();
                cacheList = service.IQueryable(expression).OrderBy(t => t.F_SortCode).ToList();//排序
                cache.WriteCache<List<ItemsEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_Items");
            }
            return cacheList;
        }
        public ItemsEntity GetForm(string keyValue)
        {
            cacheKey = cacheKey + "2_" + keyValue;//拼接有参key值
            var cacheEntity = cache.GetCache<ItemsEntity>(cacheKey);
            if (cacheEntity == null)
            {
                cacheEntity = service.FindEntity(keyValue);
                cache.WriteCache<ItemsEntity>(cacheEntity, cacheKey, "UserCacheDependency", "Sys_Items");
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
                cache.RemoveCache(cacheKey);
            }
        }
        public void SubmitForm(ItemsEntity itemsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsEntity.Modify(keyValue);
                service.Update(itemsEntity);
            }
            else
            {
                itemsEntity.Create();
                service.Insert(itemsEntity);
            }
            cache.RemoveCache(cacheKey);
        }
    }
}
