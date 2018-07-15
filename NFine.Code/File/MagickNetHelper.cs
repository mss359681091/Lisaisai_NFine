using System.IO;
using System.Web;

namespace NFine.Code
{
    public class MagickNetHelper
    {

        /// <summary>
        /// 转换图片格式
        /// </summary>
        /// <param name="oldPath">原图片路径</param>
        /// <param name="newPath">新图片路径</param>
        public static bool changeFormat(string oldPath, string newPath)
        {
            try
            {
                //** psd convert to jpg 、gift、png
                MagickNet.Magick.Init();
                //** find old path
                //** eg:"~/Images/yz1309.psd"
                MagickNet.Image img = new MagickNet.Image(HttpContext.Current.Server.MapPath(oldPath));
                //** set new image size
                System.Drawing.Size size = new System.Drawing.Size(img.BaseColumns.ToInt() / 10, img.BaseRows.ToInt() / 10);
                img.Resize(size);
                string path = newPath.Substring(0, newPath.LastIndexOf("/"));
                //** create directory if not exist
                string savePath = HttpContext.Current.Server.MapPath(path);
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
                //** save new file 
                //** eg:"~/Images/yz1309.jpg"
                img.Write(HttpContext.Current.Server.MapPath(newPath));
                MagickNet.Magick.Term();
                return true;
            }
            catch { return false; }
        }
    }
}
