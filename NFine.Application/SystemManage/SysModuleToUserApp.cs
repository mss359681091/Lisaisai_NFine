//-----------------------------------------------------------------------
// <copyright file=" SysModuleToUser.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: SysModuleToUser.cs
// * history : Created by T4 06/27/2018 14:30:32 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NFine.Application.SystemManage
{
    public class SysModuleToUserApp
    {
		private ISysModuleToUserRepository service = new SysModuleToUserRepository();

		public List<SysModuleToUserEntity> GetList(string userId)
        {
            return new SysModuleToUserRepository().GetList(userId);
        }

	    public SysModuleToUserEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(SysModuleToUserEntity entity)
        {
            service.Delete(entity);
        }

	    public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

		public void SubmitForm(SysModuleToUserEntity entity)
        {
            if (entity.F_Id.IsEmpty()) {
                entity.F_Id = Common.GuId();
                service.Insert(entity);
            } else {
                service.Delete(entity);
            }
        }
	    public void UpdateForm(SysModuleToUserEntity entity)
        {
            service.Update(entity);
        }
    }
}