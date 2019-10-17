 
using System.Collections.Generic;
using System.IO;

namespace NFine.Web.Models
{
    public static class FileUtility
    {
        public enum FileType
        {
            Text,
            Spreadsheet,
            Presentation
        }

        public static FileType GetFileType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();

            if (ExtsDocument.Contains(ext)) return FileType.Text;
            if (ExtsSpreadsheet.Contains(ext)) return FileType.Spreadsheet;
            if (ExtsPresentation.Contains(ext)) return FileType.Presentation;

            return FileType.Text;
        }

        public static readonly List<string> ExtsDocument = new List<string>
            {
                ".doc", ".docx", ".docm",
                ".dot", ".dotx", ".dotm",
                ".odt", ".fodt", ".ott", ".rtf", ".txt",
                ".html", ".htm", ".mht",
                ".pdf", ".djvu", ".fb2", ".epub", ".xps"
            };

        public static readonly List<string> ExtsSpreadsheet = new List<string>
            {
                ".xls", ".xlsx", ".xlsm",
                ".xlt", ".xltx", ".xltm",
                ".ods", ".fods", ".ots", ".csv"
            };

        public static readonly List<string> ExtsPresentation = new List<string>
            {
                ".pps", ".ppsx", ".ppsm",
                ".ppt", ".pptx", ".pptm",
                ".pot", ".potx", ".potm",
                ".odp", ".fodp", ".otp"
            };
    }
}