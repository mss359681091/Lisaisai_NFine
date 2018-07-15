using System;
using System.IO;
using System.Net;
using System.Web;

namespace NFine.Code
{
    public class ShortMsgHelper
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string PostUrl { get; set; }

        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="phones">电话</param>
        /// <returns></returns>
        public static string sendMsg(string msg, string phones)
        {
            string un = Configs.GetValue("Account").Trim();
            string pw = Configs.GetValue("Password").Trim();
            string PostUrl = Configs.GetValue("PostUrl").Trim();
            string Msgtitle = Configs.GetValue("Msgtitle").Trim();
            string content = Msgtitle + HttpContext.Current.Server.UrlEncode(msg);
            string postJsonTpl = "\"account\":\"{0}\",\"password\":\"{1}\",\"phone\":\"{2}\",\"report\":\"true\",\"msg\":\"{3}\"";
            string jsonBody = string.Format(postJsonTpl, un, pw, phones, content);
            string result = doPostMethodToObj(PostUrl, "{" + jsonBody + "}");//请求地址请登录zz.253.com获取
            return result;
        }

        /// <summary>
        /// 创蓝短信接口发送
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="jsonBody">内容信息</param>
        /// <returns>返回短信发送状态json</returns>
        private static string doPostMethodToObj(string url, string jsonBody)
        {
            string result = String.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            // Create NetworkCredential Object 
            NetworkCredential admin_auth = new NetworkCredential("username", "password");

            // Set your HTTP credentials in your request header
            httpWebRequest.Credentials = admin_auth;

            // callback for handling server certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonBody);
                streamWriter.Flush();
                streamWriter.Close();
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            return result;
        }
    }
}
