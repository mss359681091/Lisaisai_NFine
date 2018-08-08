using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Quartz.Net_Infrastructure.IPUtil
{
    public static class IPHelper
    {
        static string locationIp = string.Empty;
        public static string IpAddress
        {
            get
            {
                if (string.IsNullOrEmpty(locationIp))
                {

                    locationIp = _getIP();
                }

                return locationIp;
            }

        }

        private static string _getIP()
        {

            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.SingleOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
    }
}
