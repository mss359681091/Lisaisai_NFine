using System;
using System.IO;
using System.Web;


namespace NFine.Code
{
    public class Base64
    {
        /// <summary>
        /// 将字符串转图片
        /// </summary>
        public static string Base64String(string str, string dir, string dir1)
        {
            //站点文件目录
            string fileDir = HttpContext.Current.Server.MapPath(dir);
            string fileDir1 = HttpContext.Current.Server.MapPath(dir1);
            //文件名称guid
            string fileName = Common.GuidTo16String();
            //保存文件所在站点位置
            string filePath = Path.Combine(fileDir, fileName);
            string filePath1 = Path.Combine(fileDir1, fileName);
            if (!System.IO.Directory.Exists(fileDir))
                System.IO.Directory.CreateDirectory(fileDir);
            if (!System.IO.Directory.Exists(fileDir1))
                System.IO.Directory.CreateDirectory(fileDir1);


            byte[] arr2 = Convert.FromBase64String(str);
            using (MemoryStream ms2 = new MemoryStream(arr2))
            {
                System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(ms2);
                ////只有把当前的图像复制一份，然后把旧的Dispose掉，那个文件就不被锁住了，
                ////这样就可以放心覆盖原始文件，否则GDI+一般性错误(A generic error occurred in GDI+)
                //System.Drawing.Bitmap bmpNew = new System.Drawing.Bitmap(bmp2);
                //bmp2.Dispose();
                //bmp2 = null;
                bmp2.Save(filePath + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //bmp2.Save(filePath + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //bmp2.Save(filePath + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                //bmp2.Save(filePath + ".gif", System.Drawing.Imaging.ImageFormat.Gif);
                //bmp2.Save(filePath + ".png", System.Drawing.Imaging.ImageFormat.Png);
                bmp2.Dispose();
                NFine.Code.Common.MakeThumbnail(filePath + ".jpg", filePath1 + ".jpg", 600, 350, "W", "jpg");//生成缩略图
            }

            return fileName;
        }
    }
}
