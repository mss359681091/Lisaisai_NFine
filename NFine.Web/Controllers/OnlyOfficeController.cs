using NFine.Code;
using NFine.Web.Helpers;
using NFine.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    public class OnlyOfficeController : Controller
    {

        // GET: OnlyOffice
        public ActionResult Index(string filename, string newpath, string username, string viewtype)
        {
            var file = new FileModel
            {
                TypeDesktop = true,
                FileName = HttpUtility.HtmlDecode(filename),
                NewPath = HttpUtility.HtmlDecode(newpath),
                UserName = HttpUtility.HtmlDecode(username),
                ViewType = viewtype,
                FoleName = Server.MapPath(HttpUtility.HtmlDecode(newpath))
            };
            return View("Index", file);
        }

        #region onlyOffice跳转路径
        /// <summary>
        /// onlyOffice跳转路径
        /// </summary>
        /// <param name="filePath">文件路径（包括相对路径和绝对路径）</param>
        /// <param name="viewtype">查看方式（view 和 edit ）</param>
        /// <returns></returns>
        public ActionResult getNewfilePath(string filePath, string viewtype)
        {
            string newPath = string.Empty;//相对路径
            string fileName = string.Empty;//文件名称
            string username = OperatorProvider.Provider.GetCurrent().UserName;//当前操作人
            string tempPath = string.Empty;//文件相对路径
            try
            {
                if (FileHelper.IsExistFile(filePath))
                {
                    //文件参数是绝对路径
                    //foldName = filePath;
                    tempPath = NFine.Code.Common.urlConvertor(filePath);
                    fileName = FileHelper.GetFileName(filePath);
                    newPath = "/OnlyOffice/Index?newpath=" + HttpUtility.UrlEncode(tempPath) + "&filename=" + HttpUtility.UrlEncode(fileName) + "&username=" + HttpUtility.UrlEncode(username) + "&viewtype=" + viewtype;
                }
                else
                {
                    tempPath = filePath;//文件参数是相对路径
                    filePath = Server.MapPath(filePath);
                    if (FileHelper.IsExistFile(filePath))
                    {
                        fileName = FileHelper.GetFileName(filePath);
                        newPath = "/OnlyOffice/Index?newpath=" + HttpUtility.UrlEncode(tempPath) + "&filename=" + HttpUtility.UrlEncode(fileName) + "&username=" + HttpUtility.UrlEncode(username) + "&viewtype=" + viewtype;
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return Content(newPath);

        }
        #endregion

        /// <summary>
        /// 获取pdf
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public ActionResult getNewPdfPath(string filePath)
        {
            string url = "http://" + Request.Url.Host +":"+ Request.Url.Port;//获取当前域名
            url += "/PDF/web/viewer.html?file=" + url + filePath;
            return Content(url);

        }
    }
}