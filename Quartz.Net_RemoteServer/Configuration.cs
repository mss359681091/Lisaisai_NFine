using System.Configuration;

namespace Quartz.Net_RemoteServer
{
    public class Configuration
    {
        public static string Description { get { return ConfigurationManager.AppSettings["Description"]; } }

        public static string DisplayName { get { return ConfigurationManager.AppSettings["DisplayName"]; } }

        public static string ServiceName { get { return ConfigurationManager.AppSettings["ServiceName"]; } }
    }
}
