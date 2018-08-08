using System;
using System.IO;
using Topshelf;

namespace Quartz.Net_RemoteServer
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            HostFactory.Run(x =>
            {
                x.UseLog4Net();
                x.Service<ServiceRunner>();
                x.SetDescription(Configuration.Description);
                x.SetDisplayName(Configuration.DisplayName);
                x.SetServiceName(Configuration.ServiceName);
                x.EnablePauseAndContinue();
            });
        }
    }
}
