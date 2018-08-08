using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace NFine.Code
{
    /// <summary>
    /// 共用工具类
    /// </summary>
    public static class IpHelper
    {
        #region Ip(获取Ip)
        /// <summary>
        /// 获取Ip
        /// </summary>
        public static string GetUserIp()
        {

            string result = string.Empty;
            if (HttpContext.Current != null)
                result = GetWebClientIp();
            if (string.IsNullOrEmpty(result))
                result = GetLanIp();
            return result;

        }

        /// <summary>
        /// 获取Web客户端的Ip
        /// </summary>
        private static string GetWebClientIp()
        {
            var ip = GetWebRemoteIp();
            foreach (var hostAddress in Dns.GetHostAddresses(ip))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取Web远程Ip
        /// </summary>
        private static string GetWebRemoteIp()
        {
            return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        /// 获取局域网IP
        /// </summary>
        private static string GetLanIp()
        {
            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }
            return string.Empty;
        }

        #endregion

        #region Host(获取主机名)

        /// <summary>
        /// 获取主机名
        /// </summary>
        public static string GetLocalHost()
        {
            return HttpContext.Current == null ? Dns.GetHostName() : GetWebClientHostName();
        }

        /// <summary>
        /// 获取Web客户端主机名
        /// </summary>
        private static string GetWebClientHostName()
        {
            if (!HttpContext.Current.Request.IsLocal)
                return string.Empty;
            var ip = GetWebRemoteIp();
            var result = Dns.GetHostEntry(IPAddress.Parse(ip)).HostName;
            if (result == "localhost.localdomain")
                result = Dns.GetHostName();
            return result;
        }

        #endregion

        #region 获取mac地址
        /// <summary>
        /// 返回描述本地计算机上的网络接口的对象(网络接口也称为网络适配器)。
        /// </summary>
        /// <returns></returns>
        public static NetworkInterface[] NetCardInfo()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }
        ///<summary>
        /// 通过NetworkInterface读取网卡Mac
        ///</summary>
        ///<returns></returns>
        public static List<string> GetMacByNetworkInterface()
        {
            List<string> macs = new List<string>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                macs.Add(ni.GetPhysicalAddress().ToString());
            }
            return macs;
        }
        #endregion

        #region Ip城市(获取Ip城市)
        /// <summary>
        /// 获取IP地址信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        //public static string GetLocation(string ip)
        //{
        //    string res = "";
        //    try
        //    {
        //        string url = "http://apis.juhe.cn/ip/ip2addr?ip=" + ip + "&dtype=json&key=b39857e36bee7a305d55cdb113a9d725";
        //        res = HttpMethods.HttpGet(url);
        //        var resjson = res.ToObject<objex>();
        //        res = resjson.result.area + " " + resjson.result.location;
        //    }
        //    catch
        //    {
        //        res = "";
        //    }
        //    if (!string.IsNullOrEmpty(res))
        //    {
        //        return res;
        //    }
        //    try
        //    {
        //        string url = "https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?query=" + ip + "&resource_id=6006&ie=utf8&oe=gbk&format=json";
        //        res = HttpMethods.HttpGet(url, Encoding.GetEncoding("GBK"));
        //        var resjson = res.ToObject<obj>();
        //        res = resjson.data[0].location;
        //    }
        //    catch
        //    {
        //        res = "";
        //    }
        //    return res;
        //}
        /// <summary>
        /// 百度接口
        /// </summary>
        public class obj
        {
            public List<dataone> data { get; set; }
        }
        public class dataone
        {
            public string location { get; set; }
        }
        /// <summary>
        /// 聚合数据
        /// </summary>
        public class objex
        {
            public string resultcode { get; set; }
            public dataoneex result { get; set; }
            public string reason { get; set; }
            public string error_code { get; set; }
        }
        public class dataoneex
        {
            public string area { get; set; }
            public string location { get; set; }
        }
        #endregion

        #region Browser(获取浏览器信息)
        /// <summary>
        /// 获取浏览器信息
        /// </summary>
        public static string Browser
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var browser = HttpContext.Current.Request.Browser;
                return string.Format("{0} {1}", browser.Browser, browser.Version);
            }
        }
        #endregion



        /// <summary>  
        /// 验证IP地址字符串的正确性  
        /// </summary>  
        /// <param name="strIP">要验证的IP地址字符串</param>  
        /// <returns>格式是否正确，正确为 true 否则为 false</returns>  
        public static bool CheckIPAddr(string strIP)
        {
            if (string.IsNullOrEmpty(strIP))
            {
                return false;
            }
            // 保持要返回的信息  
            bool bRes = true;
            int iTmp = 0;    // 保持每个由“.”分隔的整型  
            string[] ipSplit = strIP.Split('.');
            if (ipSplit.Length < 4 || string.IsNullOrEmpty(ipSplit[0]) ||
                string.IsNullOrEmpty(ipSplit[1]) ||
                string.IsNullOrEmpty(ipSplit[2]) ||
                string.IsNullOrEmpty(ipSplit[3]))
            {
                bRes = false;
            }
            else
            {
                for (int i = 0; i < ipSplit.Length; i++)
                {
                    if (!int.TryParse(ipSplit[i], out iTmp) || iTmp < 0 || iTmp > 255)
                    {
                        bRes = false;
                        break;
                    }
                }
            }

            return bRes;
        }

        /// <summary>  
        /// 验证某个IP是否可ping通  
        /// </summary>  
        /// <param name="strIP">要验证的IP</param>  
        /// <returns>是否可连通  是：true 否：false</returns>  
        public static bool TestNetConnectity(string strIP)
        {
            if (!CheckIPAddr(strIP))
            {
                return false;
            }
            // Windows L2TP VPN和非Windows VPN使用ping VPN服务端的方式获取是否可以连通  
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,  
            // but change the fragmentation behavior.  
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.  
            string data = "testtesttesttesttesttesttesttest";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(strIP, timeout, buffer, options);

            return (reply.Status == IPStatus.Success);
        }

        /// <summary>  
        /// 连续几次查看是否某个IP可以PING通  
        /// </summary>  
        /// <param name="strIP">ping的IP地址</param>  
        /// <param name="WaitSecond">每次间隔时间，单位：秒</param>  
        /// <param name="iTestTimes">测试次数</param>  
        /// <returns>是否可以连通</returns>  
        public static bool TestNetConnected(string strIP, int WaitSecond, int iTestTimes)
        {
            for (int i = 0; i < iTestTimes - 1; i++)
            {
                if (TestNetConnectity(strIP))
                {
                    return true;
                }
                Thread.Sleep(WaitSecond * 1000);
            }

            return TestNetConnectity(strIP);
        }

        /// <summary>
        /// 获取ip集合中可用ip中的第一个
        /// </summary>
        /// <param name="ip_arr"></param>
        /// <returns></returns>
        public static string getCurIp(string ip_arr,int port)
        {
            List<string> ip_lst = StringHelper.GetStrArray(ip_arr, ',', true);
            for (int i = 0; i < ip_lst.Count; i++)
            {
                //bool b = IpHelper.TestNetConnectity(ip_lst[i]);//ping ip
                bool b = IpHelper.TestConnection(ip_lst[i], port, 500);
                if (b)
                {
                    return ip_lst[i];
                }
            }
            return "";
        }

        #region 采用Socket方式，测试服务器连接 
        /// <summary> 
        /// 采用Socket方式，测试服务器连接 
        /// </summary> 
        /// <param name="host">服务器主机名或IP</param> 
        /// <param name="port">端口号</param> 
        /// <param name="millisecondsTimeout">等待时间：毫秒</param> 
        /// <returns></returns> 
        public static bool TestConnection(string host, int port, int millisecondsTimeout)
        {
            TcpClient client = new TcpClient();
            try
            {
                var ar = client.BeginConnect(host, port, null, null);
                ar.AsyncWaitHandle.WaitOne(millisecondsTimeout);
                return client.Connected;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                client.Close();
            }
        }
        #endregion
    }
}
