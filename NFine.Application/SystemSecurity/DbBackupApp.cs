/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Code;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Domain.IRepository.SystemSecurity;
using NFine.Domain.ViewModel;
using NFine.Repository.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemSecurity
{
    public class DbBackupApp
    {
        private IDbBackupRepository service = new DbBackupRepository();

        public List<DbBackupEntity> GetList(string queryJson)
        {
            var expression = ExtLinq.True<DbBackupEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyword = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "DbName":
                        expression = expression.And(t => t.F_DbName.Contains(keyword));
                        break;
                    case "FileName":
                        expression = expression.And(t => t.F_FileName.Contains(keyword));
                        break;
                }
            }
            return service.IQueryable(expression).OrderByDescending(t => t.F_BackupTime).ToList();
        }

        public List<DbBackupEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<DbBackupEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                string condition = string.Empty;
                if (queryParam["condition"] != null)
                {
                    condition = queryParam["condition"].ToString().Trim();
                }
                if (condition == "DbName")
                {
                    expression = expression.And(t => t.F_DbName.Contains(keyword));
                }
                else
                {
                    expression = expression.And(t => t.F_FileName.Contains(keyword));
                }
            }
            return service.FindList(expression, pagination);
        }


        public string GetMaxCreatorTime()
        {
            List<DbBackupDto> lst = new DbBackupRepository().GetMaxCreatorTime();
            string result = string.Empty;
            if (lst.Count > 0)
            {
                result = lst.Select(e => e.F_BackupTime).ToList()[0]?.ToString("D");//最后备份日期
            }
            return result;
        }

        public DbBackupEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void SubmitForm(DbBackupEntity dbBackupEntity)
        {
            dbBackupEntity.F_Id = Common.GuId();
            dbBackupEntity.F_EnabledMark = true;
            dbBackupEntity.F_BackupTime = DateTime.Now;
            service.ExecuteDbBackup(dbBackupEntity);
        }
    }
}
