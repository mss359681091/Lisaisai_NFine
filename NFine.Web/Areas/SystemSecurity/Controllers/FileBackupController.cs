using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity;
using NFine.Domain.Entity.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemSecurity.Controllers
{
    public class FileBackupController : ControllerBase
    {
        private SysFileBackupApp app = new SysFileBackupApp();

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
        public ActionResult SubmitForm(SysFileBackupEntity entity, string keyValue, string chkDrop)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                base.OperateLog("编辑文件备份：F_Id:" + keyValue, "/SystemSecurity/FileBackup/SubmitForm", "文件备份", Application.DbLogType.Update);
            }
            else
            {
                base.OperateLog("新增文件备份：" + entity.F_BackupName, "/SystemSecurity/FileBackup/SubmitForm", "文件备份", Application.DbLogType.Create);
            }

            if (!string.IsNullOrEmpty(entity.F_ServerIp))
            {
                SysBackupServerEntity model = new SysBackupServerApp().GetForm(entity.F_ServerIp);

                string virtualPath = string.Empty;
                string strPath = string.Empty;

                switch (entity.F_BackupType)
                {
                    case "0": //图片
                        virtualPath = "/Upload/Images/";
                        entity.F_FileName = Common.CreateNo() + "_Images.zip";
                        strPath = Server.MapPath("/Upload/" + entity.F_FileName);
                        break;
                    case "1": //源文件
                        virtualPath = "/Upload/files/";
                        entity.F_FileName = Common.CreateNo() + "_Files.zip";
                        strPath = Server.MapPath("/Upload/" + entity.F_FileName);
                        break;
                    case "2": //全部
                        virtualPath = "/Upload/";
                        entity.F_FileName = Common.CreateNo() + "_Upload.zip";
                        strPath = Server.MapPath("/Upload/" + entity.F_FileName);
                        break;
                    default:
                        break;
                }
                FtpTrans(model, entity.F_BackupPath, virtualPath, strPath, chkDrop);//备份操作,异步操作
                app.SubmitForm(entity, keyValue);
                return Success("操作成功。");
            }
            return Error("ServerIp地址不存在。");
        }

        private void FtpTrans(SysBackupServerEntity model, string backupPath, string virtualPath, string strPath, string chkDrop)
        {
            Task.Run(() =>
           {
               if (model != null)
               {
                   FTPHelper ftp = new FTPHelper(model.F_FtpServerIp, "", model.F_FtpUserId, model.F_FtpPassword);
                   if (!ftp.IsDirectoryExist(backupPath))
                   {
                       ftp.CreateDirectory(backupPath);
                   }
                   ftp = new FTPHelper(model.F_FtpServerIp, backupPath, model.F_FtpUserId, model.F_FtpPassword);

                   ZipFloClass Zc = new ZipFloClass();   //将文件夹进行GZip压缩
                   Zc.ZipFile(Server.MapPath(virtualPath), strPath);//生成压缩包

                   if (chkDrop == "true")
                   {
                       //遍历删除当前目录文件
                       List<FileStruct> ListFiles = ftp.ListFiles();
                       if (ListFiles.Count > 0)
                       {
                           foreach (var item in ListFiles)
                           {
                               ftp.DeleteFile(item.Name);
                           }
                       }
                   }
                   ftp.Upload(strPath);//通过ftp传到服务器
                   FileHelper.DeleteFile(strPath);//删除临时压缩包
               }
           });
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            app.DeleteForm(keyValue);
            base.OperateLog("删除文件备份：" + keyValue, "/SystemSecurity/FileBackup/DeleteForm", "文件备份", Application.DbLogType.Delete);
            return Success("删除成功。");
        }

    }
}
