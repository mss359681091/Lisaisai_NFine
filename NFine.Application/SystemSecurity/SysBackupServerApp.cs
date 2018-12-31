//-----------------------------------------------------------------------
// <copyright file=" SysBackupServer.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: SysBackupServer.cs
// * history : Created by T4 06/08/2018 09:38:22 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Cache;
using NFine.Cache.Factory;
using NFine.Code;
using NFine.Domain.Entity;
using NFine.Domain.IRepository;
using NFine.Repository;
using NFine.Repository.WebManage;
using System.Collections.Generic;
using System.Linq;
namespace NFine.Application.SystemSecurity
{
    public class SysBackupServerApp
    {
        public string cacheKey = "sysBackupServerCache";//缓存键值
        ICache cache = CacheFactory.Cache();//实例化缓存，默认自带缓存

        private ISysBackupServerRepository service = new SysBackupServerRepository();

		public List<SysBackupServerEntity> GetList(Pagination pagination, string queryJson)
        {
		    var expression = ExtLinq.True<SysBackupServerEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_FullName.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

	    public SysBackupServerEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(SysBackupServerEntity entity)
        {
            service.Delete(entity);
        }

	    public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

		public void SubmitForm(SysBackupServerEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
            else
            {
                entity.Create();
                service.Insert(entity);
            }
        }

	    public void UpdateForm(SysBackupServerEntity entity)
        {
            service.Update(entity);
        }

        public List<SysBackupServerEntity> GetList()
        {
            var cacheList = cache.GetCache<List<SysBackupServerEntity>>(cacheKey);
            if (cacheList == null)
            {
                cacheList = service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
                cache.WriteCache<List<SysBackupServerEntity>>(cacheList, cacheKey, "UserCacheDependency", "Sys_BackupServer");
            }
            return cacheList;
        }
    }
}