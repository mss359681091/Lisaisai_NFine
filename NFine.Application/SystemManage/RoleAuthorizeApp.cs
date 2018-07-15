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
using NFine.Domain;

namespace NFine.Application.SystemManage
{
    public class RoleAuthorizeApp
    {
        public string cacheKey = "roleAuthorizeCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存
        private IRoleAuthorizeRepository service = new RoleAuthorizeRepository();
        private ModuleApp moduleApp = new ModuleApp();
        private ModuleButtonApp moduleButtonApp = new ModuleButtonApp();

        public List<RoleAuthorizeEntity> GetList(string ObjectId)
        {
            cacheKey = cacheKey + "0_" + ObjectId;//拼接有参key值
            var cacheList = cache.GetCache<List<RoleAuthorizeEntity>>(cacheKey);
            if (cacheList == null)
            {
                cacheList = service.IQueryable(t => t.F_ObjectId == ObjectId).ToList();
                cache.WriteCache<List<RoleAuthorizeEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_RoleAuthorize");
            }
            return cacheList;
        }
        public List<ModuleEntity> GetMenuList(string roleId)
        {
            var data = new List<ModuleEntity>();
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                data = moduleApp.GetList();
            }
            else
            {
                var moduledata = moduleApp.GetList();
                var authorizedata = service.IQueryable(t => t.F_ObjectId == roleId && t.F_ItemType == 1).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleEntity moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId);
                    if (moduleEntity != null)
                    {
                        data.Add(moduleEntity);
                    }
                }
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }
        public List<ModuleButtonEntity> GetButtonList(string roleId)
        {
            var data = new List<ModuleButtonEntity>();
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                data = moduleButtonApp.GetList();
            }
            else
            {
                var buttondata = moduleButtonApp.GetList();
                var authorizedata = service.IQueryable(t => t.F_ObjectId == roleId && t.F_ItemType == 2).ToList();
                foreach (var item in authorizedata)
                {
                    ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId);
                    if (moduleButtonEntity != null)
                    {
                        data.Add(moduleButtonEntity);
                    }
                }
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }
        public bool ActionValidate(string roleId, string moduleId, string action)
        {
            var authorizeurldata = new List<AuthorizeActionModel>();
            var cachedata = CacheFactory.Cache().GetCache<List<AuthorizeActionModel>>("authorizeurldata_" + roleId);
            if (cachedata == null)
            {
                var moduledata = moduleApp.GetList();
                var buttondata = moduleButtonApp.GetList();
                var authorizedata = service.IQueryable(t => t.F_ObjectId == roleId).ToList();
                foreach (var item in authorizedata)
                {
                    if (item.F_ItemType == 1)
                    {
                        ModuleEntity moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId);
                        if (moduleEntity != null)
                        {
                            authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress });
                        }
                       
                    }
                    else if (item.F_ItemType == 2)
                    {
                        ModuleButtonEntity moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId);
                        if (moduleButtonEntity != null)
                        {
                            authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleButtonEntity.F_ModuleId, F_UrlAddress = moduleButtonEntity.F_UrlAddress });
                        }
                    }
                }
                CacheFactory.Cache().WriteCache(authorizeurldata, "authorizeurldata_" + roleId, DateTime.Now.AddMinutes(5));
            }
            else
            {
                authorizeurldata = cachedata;
            }
            authorizeurldata = authorizeurldata.FindAll(t => t.F_Id.Equals(moduleId));
            foreach (var item in authorizeurldata)
            {
                if (!string.IsNullOrEmpty(item.F_UrlAddress))
                {
                    string[] url = item.F_UrlAddress.Split('?');
                    if (item.F_Id.Trim() == moduleId.Trim() && url[0].ToUpper().Trim() == action.ToUpper().Trim())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
