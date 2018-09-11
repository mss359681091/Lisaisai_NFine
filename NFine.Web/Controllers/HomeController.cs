/*******************************************************************************
 * Copyright © 2016 Lee.Framework 版权所有
 * Author: Lss
 * Description: 项目主页控制器
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Application.SystemManage;
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    [HandlerLogin]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public override ActionResult Index()
        {
            if (OperatorProvider.Provider.GetCurrent() == null)
                return RedirectToAction("Index", "Login");
            ViewData["Portrait"] = GetPortrait(OperatorProvider.Provider.GetCurrent().UserId);//获取头像
            return View();
        }

        [HttpGet]
        public ActionResult Default()
        {
            

            //DBConnection.Encrypt = false;
            var a = DESEncrypt.Encrypt(DBConnection.connectionString) ;

            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }
        #region 搜索跳转页面
        /// <summary>
        /// 搜索跳转页面
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetTreeGridJson(string keyword)
        {
            ModuleApp moduleApp = new ModuleApp();
            var data = moduleApp.GetList();
            data = data.TreeWhere(t => t.F_IsMenu.GetValueOrDefault() == true && t.F_FullName == keyword);
            List<Tuple<string, string>> lstClass = new List<Tuple<string, string>>();
            foreach (ModuleEntity item in data)
            {
                if (item.F_UrlAddress != null)
                {
                    lstClass.Add(new Tuple<string, string>(item.F_FullName, item.F_UrlAddress));
                }
            }
            if (lstClass.Count > 1)
            {
                return null;
            }
            var jsonData = lstClass.ToJson();
            return jsonData;
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public string GetTreeJson(string keyword)
        {
            ModuleApp moduleApp = new ModuleApp();
            var data = moduleApp.GetList();
            data = data.TreeWhere(t => t.F_IsMenu.GetValueOrDefault() == true && t.F_FullName.Contains(keyword));
            List<Tuple<string>> lstClass = new List<Tuple<string>>();
            foreach (ModuleEntity item in data)
            {
                if (item.F_UrlAddress != null)
                {
                    lstClass.Add(new Tuple<string>(item.F_FullName));
                }
            }
            var jsonData = lstClass.ToJson();
            return jsonData;
        }
        #endregion

        #region   public FileContentResult CreateShortcutFile() 创建桌面快捷方式
        /// <summary>
        /// 创建桌面快捷方式
        /// </summary>
        /// <returns></returns>
        public FileContentResult CreateShortcutFile()
        {
            string browser = Request.UserAgent.ToUpper();
            string outputFileName = "恒禹软件.url";

            if (browser.Contains("MS") == true && browser.Contains("IE") == true)
            {
                outputFileName = System.Web.HttpUtility.UrlEncode(outputFileName, System.Text.Encoding.UTF8) + ".url";
            }
            else if (browser.Contains("FIREFOX") == true)
            {
                //outputFileName = outputFileName ;
            }
            else
            {
                outputFileName = HttpUtility.UrlEncode(outputFileName);
            }

            string HostAddr = "http://" + Request.Url.Authority;
            string icoPath = HostAddr + "/favicon.ico";//修改此处更改url图标或者图标路径，当前路径为根目录，只用修改相对路径，图标的完整路径由下方会自动生成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[InternetShortcut]");
            sb.AppendLine("URL=" + HostAddr); //快捷方式的外部链接  
            sb.AppendLine("IDList= ");
            sb.AppendLine("IconFile=" + icoPath); //图标文件  
            sb.AppendLine("IconIndex=1 ");
            sb.AppendLine("[{000214A0-0000-0000-C000-000000000046}]");
            sb.AppendLine("Prop3=19,2 ");
            //第一种:使用FileContentResult  
            byte[] fileContents = Encoding.Default.GetBytes(sb.ToString());
            //string fileName = System.Web.HttpUtility.UrlEncode("恒禹软件", System.Text.Encoding.UTF8) + ".url";
            return File(fileContents, "APPLICATION/OCTET-STREAM", outputFileName);
        }
        #endregion


        public ActionResult Avatar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FnAvatar()
        {
            string fieldName = Request.Files.AllKeys[0];
            HttpPostedFileBase file = Request.Files[0];

            //参数
            string filePath = "/Resource/Avatar";//头像存放目录
            //设置存放文件路径，"/UploadFile"存放文件夹名
            //var applicationPath = VirtualPathUtility.ToAbsolute("~") == "/" ? "" : VirtualPathUtility.ToAbsolute("~");
            string urlPath = filePath;
            //新文件名
            string filePathName = string.Empty;
            //存放文件路径
            string localPath = Server.MapPath(filePath);
            try
            {
                //获取到需要上传的文件数量
                if (Request.Files.Count == 0)
                {
                    return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "请选择需要上传的文件" } });
                }
                //获取文件扩展名
                string ex = Path.GetExtension(file.FileName) == "" ? ".jpg" : Path.GetExtension(file.FileName);
                //生成新的文件名
                filePathName = Guid.NewGuid().ToString("N") + ex;
                //判断路径是否存在，不存在就创建
                if (!System.IO.Directory.Exists(localPath))
                {
                    System.IO.Directory.CreateDirectory(localPath);
                }
                //上传文件
                file.SaveAs(Path.Combine(localPath, filePathName));
                //将存储文件及路径存入数据库表中

                UserEntity model = new UserEntity();
                model = new UserApp().GetUserEntity(OperatorProvider.Provider.GetCurrent().UserId);
                model.F_HeadIcon = urlPath + "/" + filePathName;
                new UserApp().UpdateForm(model);//更新
                return Json(new
                {
                    msg = "Success",
                    filePath = urlPath + "/" + filePathName//存放头像路径 
                });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message });
            }
        }

        //获取头像
        public string GetPortrait(string userId)
        {
            string portrait = "";
            UserEntity user = new UserApp().GetUserEntity(userId);
            if (user != null && user.F_HeadIcon != null && user.F_HeadIcon != "")
            {
                portrait = user.F_HeadIcon;
            }
            return portrait;
        }

        #region 快捷操作部分
        /// <summary>
        /// 获取快捷操作
        /// </summary>
        /// <returns></returns>
        public ActionResult GetQuickActionsJson()
        {
            var userId = OperatorProvider.Provider.GetCurrent().UserId;
            SysModuleToUserApp sysModuleToUserApp = new SysModuleToUserApp();
            var data = sysModuleToUserApp.GetList(userId);
            string jsonData = data.ToJson();
            return Content(jsonData);
        }

        /// <summary>
        /// 添加快捷操作页面
        /// </summary>
        /// <returns></returns>
        public override ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 提交快捷操作
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SubmitMenuListForm(string jsonData)
        {
            var userId = OperatorProvider.Provider.GetCurrent().UserId;
            var arrayJsonData = NFine.Code.Json.ToObject<List<SysModuleToUserEntity>>(jsonData);
            SysModuleToUserApp sysModuleToUserApp = new SysModuleToUserApp();
            var data = sysModuleToUserApp.GetList(userId);
            foreach (SysModuleToUserEntity item in data)
            {
                sysModuleToUserApp.SubmitForm(item);
            }
            if (arrayJsonData.Count > 0)
            {
                for (int i = 0; i < arrayJsonData.Count; i++)
                {
                    SysModuleToUserEntity sysModuleToUserEntity = new SysModuleToUserEntity();
                    sysModuleToUserEntity.F_FullName = arrayJsonData[i].F_FullName;
                    sysModuleToUserEntity.F_ModuleId = arrayJsonData[i].F_ModuleId;
                    sysModuleToUserEntity.F_ModuleType = arrayJsonData[i].F_ModuleType;
                    sysModuleToUserEntity.F_UserId = userId;
                    sysModuleToUserApp.SubmitForm(sysModuleToUserEntity);
                }
                base.ErrLog("添加快捷操作：用户ID:" + userId, "/Home/SubmitMenuListForm", "主页", Application.DbLogType.Create);
            }
            return Success("操作成功。");
        }
        #endregion


    }
}
