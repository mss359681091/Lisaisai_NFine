/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.SystemSecurity;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemSecurity.Controllers
{
    public class DbBackupController : ControllerBase
    {
        private DbBackupApp dbBackupApp = new DbBackupApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = dbBackupApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }


        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(DbBackupEntity dbBackupEntity)
        {
            string DBBasePath = System.Configuration.ConfigurationManager.AppSettings["CTDBBasePath"];
            dbBackupEntity.F_FilePath = DBBasePath + dbBackupEntity.F_FileName + ".bak";
            FileHelper.CreateDirectory(DBBasePath);
            dbBackupEntity.F_FileName = dbBackupEntity.F_FileName + ".bak";
            dbBackupApp.SubmitForm(dbBackupEntity);
            base.ErrLog("数据库备份", "/SystemSecurity/DbBackup/SubmitForm", "数据备份", Application.DbLogType.Other);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            dbBackupApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpPost]
        [HandlerAuthorize]
        public void DownloadBackup(string keyValue)
        {
            if ((keyValue != "undefined") && !string.IsNullOrEmpty(keyValue))
            {
                var data = dbBackupApp.GetForm(keyValue);
                string filename = Server.UrlDecode(data.F_FileName);
                string filepath = data.F_FilePath;
                if (FileDownHelper.FileExists(filepath))
                {
                    FileDownHelper.DownLoadold(filepath, filename);
                }
                base.ErrLog("据库备份下载", "/SystemSecurity/DbBackup/DownloadBackup", "备份下载", Application.DbLogType.Other);
            }

        }
    }
}
