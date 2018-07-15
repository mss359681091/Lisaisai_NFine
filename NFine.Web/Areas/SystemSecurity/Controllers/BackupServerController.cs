using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity;
using NFine.Domain.Entity.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemSecurity.Controllers
{
    public class BackupServerController : ControllerBase
    {
        private SysBackupServerApp app = new SysBackupServerApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = app.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysBackupServerEntity model, string keyValue)
        {
            app.SubmitForm(model, keyValue);
            if (!string.IsNullOrEmpty(keyValue))
            {
                base.ErrLog("编辑备份服务器：F_Id:" + keyValue, "/SystemSecurity/BackupServer/SubmitForm", "备份服务器", Application.DbLogType.Update);
            }
            else
            {
                base.ErrLog("新增备份服务器", "/SystemSecurity/BackupServer/SubmitForm", "备份服务器", Application.DbLogType.Create);
            }
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            app.DeleteForm(keyValue);
            base.ErrLog("删除备份服务器：" + keyValue, "/SystemSecurity/BackupServer/DeleteForm", "备份服务器", Application.DbLogType.Delete);
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledConents(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                List<string> lstid = StringHelper.GetStrArray(keyValue, ',', false);
                for (int i = 0; i < lstid.Count; i++)
                {
                    SysBackupServerEntity model = new SysBackupServerEntity();
                    model.F_Id = lstid[i];
                    model.F_EnabledMark = false;
                    app.UpdateForm(model);
                }
                base.ErrLog("备份服务器禁用：" + keyValue, "/SystemSecurity/BackupServer/DisabledConents", "备份服务器", Application.DbLogType.Submit);
                return Success("禁用成功");
            }
            return Success("请选择禁用项");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledConents(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                List<string> lstid = StringHelper.GetStrArray(keyValue, ',', false);
                for (int i = 0; i < lstid.Count; i++)
                {
                    SysBackupServerEntity model = new SysBackupServerEntity();
                    model.F_Id = lstid[i];
                    model.F_EnabledMark = true;
                    app.UpdateForm(model);
                }
                base.ErrLog("备份服务器启用：" + keyValue, "/SystemSecurity/BackupServer/EnabledConents", "备份服务器", Application.DbLogType.Submit);
                return Success("启用成功");
            }
            return Success("请选择启用项");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult DoConnect(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SysBackupServerEntity model = new SysBackupServerEntity();
                model = app.GetForm(keyValue);

                FTPHelper help = new FTPHelper(model.F_FtpServerIp, "", model.F_FtpUserId, model.F_FtpPassword);
                try
                {
                    help.IsDirectoryExist("test");
                    return Success("连接成功");
                }
                catch (Exception ex)
                {

                }

            }
            return Error("连接失败");
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetServerIpJson()
        {
            var data = app.GetList();
            List<object> list = new List<object>();
            foreach (SysBackupServerEntity item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName });
            }
            return Content(list.ToJson());
        }
    }
}
