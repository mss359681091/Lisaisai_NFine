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
                x.SetDescription("Quartz.Net_RemoteServer服务描述");
                x.SetDisplayName("Quartz.Net_RemoteServer服务显示名称");
                x.SetServiceName("Quartz.Net_RemoteServer服务名称");
                x.EnablePauseAndContinue();
            });
        }
    }
}
