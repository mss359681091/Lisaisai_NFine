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
    public class ItemsDetailApp
    {
        public string cacheKey = "dataItemCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
        private IItemsDetailRepository service = new ItemsDetailRepository();

        public List<ItemsDetailEntity> GetList(string itemId = "", string keyword = "")
        {
            //cacheKey = cacheKey + "0_" + itemId + "_" + keyword;//拼接有参key值
            //var cacheList = cache.GetCache<List<ItemsDetailEntity>>(cacheKey);
            //if (cacheList == null)
            //{
            var expression = ExtLinq.True<ItemsDetailEntity>();
            if (!string.IsNullOrEmpty(itemId))
            {
                expression = expression.And(t => t.F_ItemId == itemId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_ItemName.Contains(keyword));
                expression = expression.Or(t => t.F_ItemCode.Contains(keyword));
            }
            var cacheList = service.IQueryable(expression).OrderBy(t => t.F_SortCode).ToList();
            //    cache.WriteCache<List<ItemsDetailEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_ItemsDetail");
            //}
            return cacheList;
        }

        public List<ItemsDetailEntity> GetItemList(string enCode)
        {
            //cacheKey = cacheKey + "1_" + enCode;//拼接有参key值
            //var cacheList = cache.GetCache<List<ItemsDetailEntity>>(cacheKey);
            //if (cacheList == null)
            //{
            //    cacheList = service.GetItemList(enCode);
            //    cache.WriteCache<List<ItemsDetailEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_ItemsDetail");
            //}
            return service.GetItemList(enCode);
        }

        public ItemsDetailEntity GetForm(string keyValue)
        {
            //cacheKey = cacheKey + "2_" + keyValue;//拼接有参key值
            //var cacheEntity = cache.GetCache<ItemsDetailEntity>(cacheKey);
            //if (cacheEntity == null)
            //{
            //    cacheEntity = service.FindEntity(keyValue);
            //    cache.WriteCache<ItemsDetailEntity>(cacheEntity, cacheKey, "UserCacheDependency", "Sys_ItemsDetail");
            //}
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

        public void SubmitForm(ItemsDetailEntity itemsDetailEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsDetailEntity.Modify(keyValue);
                service.Update(itemsDetailEntity);
            }
            else
            {
                itemsDetailEntity.Create();
                service.Insert(itemsDetailEntity);
            }
        }

        public void InsertForm(ItemsDetailEntity itemsDetailEntity)
        {
            service.Insert(itemsDetailEntity);
        }

        /// <summary>
        /// 根据enCode获取字典实体
        /// </summary>
        /// <param name="enCode"></param>
        /// <returns></returns>
        public ItemsDetailEntity GetItemByItemCode(string enCode)
        {
            ItemsDetailEntity model = new ItemsDetailEntity();
            List<ItemsDetailEntity> lst = service.GetItemByItemCode(enCode);
            if (lst.Count > 0)
            {
                model = lst[0];
            }
            return model;
        }
    }
}
