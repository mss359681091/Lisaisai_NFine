

using NFine.Web.Helpers;
using System.IO;

namespace NFine.Web.Models
{
    public class FileModel
    {
        public bool TypeDesktop { get; set; }

        public string FileUri
        {
            get { return DocManagerHelper.GetFileUri(FileName); }
        }

        public string FileName { get; set; }
        public string FoleName { get; set; }
        public string NewPath { get; set; }
        public string UserName { get; set; }
        public string ViewType { get; set; }

        public string DocumentType
        {
            get { return FileUtility.GetFileType(FileName).ToString().ToLower(); }
        }
        public string Key
        {
            get { return ServiceConverter.GenerateRevisionId(FoleName + "/" + File.GetLastWriteTime(FoleName).GetHashCode()); }
        }

        public string CallbackUrl
        {
            get
            {
                return DocManagerHelper.GetCallback(NewPath);
            }
        }
    }

   
}