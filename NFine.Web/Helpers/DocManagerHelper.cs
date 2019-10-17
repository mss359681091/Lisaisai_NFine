
using NFine.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace NFine.Web.Helpers
{
    public class DocManagerHelper
    {
        public static long MaxFileSize
        {
            get
            {
                long size;
                long.TryParse(WebConfigurationManager.AppSettings["filesize-max"], out size);
                return size > 0 ? size : 5 * 1024 * 1024;
            }
        }

        public static List<string> FileExts
        {
            get { return ViewedExts.Concat(EditedExts).Concat(ConvertExts).ToList(); }
        }

        public static List<string> ViewedExts
        {
            get { return (WebConfigurationManager.AppSettings["files.docservice.viewed-docs"] ?? "").Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(); }
        }

        public static List<string> EditedExts
        {
            get { return (WebConfigurationManager.AppSettings["files.docservice.edited-docs"] ?? "").Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(); }
        }

        public static List<string> ConvertExts
        {
            get { return (WebConfigurationManager.AppSettings["files.docservice.convert-docs"] ?? "").Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(); }
        }

        public static string CurUserHostAddress(string userAddress = null)
        {
            return Regex.Replace(userAddress ?? HttpContext.Current.Request.UserHostAddress, "[^0-9a-zA-Z.=]", "_");
        }

        public static string StoragePath(string fileName, string userAddress = null)
        {
            var directory = HttpRuntime.AppDomainAppPath + CurUserHostAddress(userAddress) + "\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory + fileName;
        }

        public static string GetCorrectName(string fileName)
        {
            var baseName = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);
            var name = baseName + ext;

            for (var i = 1; File.Exists(StoragePath(name)); i++)
            {
                name = baseName + " (" + i + ")" + ext;
            }
            return name;
        }

        public static string CreateDemo(string fileExt)
        {
            var demoName = "sample." + fileExt;

            var fileName = GetCorrectName(demoName);

            File.Copy(HttpRuntime.AppDomainAppPath + "app_data\\" + demoName, StoragePath(fileName));

            return fileName;
        }

        public static string GetFileUri(string fileName)
        {
            var uri = new UriBuilder(HttpContext.Current.Request.Url)
            {
                Path = HttpRuntime.AppDomainAppVirtualPath + "/"
                           + CurUserHostAddress() + "/"
                           + fileName,
                Query = ""
            };

            return uri.ToString();
        }

        public static string GetCallback(string NewPath)
        {
            var callbackUrl = new UriBuilder(HttpContext.Current.Request.Url)
            {
                Path = HttpRuntime.AppDomainAppVirtualPath + (HttpRuntime.AppDomainAppVirtualPath.EndsWith("/") ? "" : "/")
                    + "webeditor.ashx",
                Query = "fileName=" + HttpUtility.UrlEncode(NewPath)

            };
            return callbackUrl.ToString();
        }

        public static string GetInternalExtension(FileUtility.FileType fileType)
        {
            switch (fileType)
            {
                case FileUtility.FileType.Text:
                    return ".docx";
                case FileUtility.FileType.Spreadsheet:
                    return ".xlsx";
                case FileUtility.FileType.Presentation:
                    return ".pptx";
                default:
                    return ".docx";
            }
        }
    }
}