/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Cache;
using NFine.Cache.Factory;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace NFine.Application.SystemManage
{
    public class ModuleApp
    {
        public string cacheKey = "moduleCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
        private IModuleRepository service = new ModuleRepository();

        public List<ModuleEntity> GetList()
        {
            var cacheList = cache.GetCache<List<ModuleEntity>>(cacheKey);
            if (cacheList == null)
            {
                cacheList = service.IQueryable().OrderBy(t => t.F_SortCode).ToList();
                cache.WriteCache<List<ModuleEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_Module");
            }
            return cacheList;
        }
        public List<ModuleEntity> GetList(string roleId)
        {
            cacheKey = cacheKey + "_" + roleId;//拼接有参key值
            var cacheList = cache.GetCache<List<ModuleEntity>>(cacheKey);
            if (cacheList == null)
            {
                string sql = @"select * from Sys_Module t1
                                        left join  Sys_RoleAuthorize t2
                                        on t1.F_Id=t2.F_ItemId
                                        where 1=1
                                        and t1.F_EnabledMark=1
                                        and t2.F_ItemType=1
                                        and t2.F_ObjectType=1 ";
                sql += " and t2.F_ObjectId=@F_ObjectId ";
                sql += " order by t1.F_SortCode asc";

                DbParameter[] parameter ={
                 new SqlParameter("@F_ObjectId",roleId)
               };
                cacheList = service.FindList(sql, parameter);
                cache.WriteCache<List<ModuleEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_RoleAuthorize");
            }
            return cacheList;
        }

        public ModuleEntity GetForm(string keyValue)
        {
            cacheKey = cacheKey + "2_" + keyValue;//拼接有参key值
            var cacheEntity = cache.GetCache<ModuleEntity>(cacheKey);
            if (cacheEntity == null)
            {
                cacheEntity = service.FindEntity(keyValue);
                cache.WriteCache<ModuleEntity>(cacheEntity, cacheKey, "UserCacheDependency", "Sys_Module");
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
                service.DeleteForm(keyValue);//级联删除
            }
        }
        public void SubmitForm(ModuleEntity moduleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleEntity.Modify(keyValue);
                service.Update(moduleEntity);
            }
            else
            {
                moduleEntity.Create();
                service.Insert(moduleEntity);
            }
        }
    }
}
