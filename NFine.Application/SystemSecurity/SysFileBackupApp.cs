//-----------------------------------------------------------------------
// <copyright file=" SysFileBackup.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: SysFileBackup.cs
// * history : Created by T4 06/08/2018 09:38:33 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Code;
using NFine.Domain.Entity;
using NFine.Domain.IRepository;
using NFine.Domain.ViewModel;
using NFine.Repository.WebManage;
using System.Collections.Generic;

namespace NFine.Application.SystemSecurity
{
    public class SysFileBackupApp
    {
        private ISysFileBackupRepository service = new SysFileBackupRepository();

        public List<SysFileBackupDto> GetList(Pagination pagination, string queryJson)
        {
            return new SysFileBackupRepository().GetList(queryJson, pagination);
        }

        public SysFileBackupEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(SysFileBackupEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

        public void SubmitForm(SysFileBackupEntity entity, string keyValue)
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
        public void UpdateForm(SysFileBackupEntity entity)
        {
            service.Update(entity);
        }
    }
}