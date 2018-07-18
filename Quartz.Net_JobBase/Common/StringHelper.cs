
using System;
using System.Collections.Generic;

namespace Server.Common
{
    public class StringHelper
    {
        /// <summary>
        /// 获取url参数，并以键值对形式存放在字典中
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Dictionary<string, string> getUrlParams(string url)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            try
            {
                var uri = new Uri(url);
                var queryString = uri.Query;
                if (!string.IsNullOrEmpty(queryString))
                {
                    queryString = queryString.Substring(1, queryString.Length - 1);
                    if (queryString.Contains("&"))
                    {
                        string[] arr = queryString.Split('&');
                        if (arr.Length > 0)
                        {
                            for (int i = 0; i < arr.Length; i++)
                            {
                                string[] arrc = arr[i].Split('=');
                                if (arrc.Length > 0)
                                {
                                    parames.Add(arrc[0], arrc[1]);
                                }
                            }
                        }
                    }
                    else
                    {
                        string[] arrc = queryString.Split('=');
                        if (arrc.Length > 0)
                        {
                            parames.Add(arrc[0], arrc[1]);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return parames;
        }
    }
}
