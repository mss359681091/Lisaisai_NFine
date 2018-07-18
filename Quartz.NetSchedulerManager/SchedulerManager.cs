using NFine.Code;
using Quartz.Impl;
using System.Collections.Concurrent;
using System.Collections.Specialized;

namespace Quartz.NetSchedulerManager
{
    public class SchedulerManager
    {
        static readonly object Locker = new object();
        static IScheduler _scheduler;
        static readonly ConcurrentDictionary<string, IScheduler> ConnectionCache = new ConcurrentDictionary<string, IScheduler>();
        const string channelType = "tcp";
        const string localIp = "127.0.0.1";//ip集合
        const string port = "555";
        const string bindName = "QuartzScheduler";
        public static IScheduler Instance
        {
            get
            {
                if (_scheduler == null)
                {
                    lock (Locker)
                    {
                        if (_scheduler == null)
                        {
                            string strip = Configs.GetValue("iplist");//读取配置文件IP集合
                            string curip = string.Empty;
                            if (!string.IsNullOrEmpty(strip))
                            {
                                curip = IpHelper.getCurIp(strip);//获取一个可用ip
                            }
                            else
                            {
                                curip = localIp;//如果配置文件没有配置ip,则使用默认
                            }
                            _scheduler = GetScheduler(curip);
                        }
                    }
                }
                return _scheduler;
            }
        }
        public static IScheduler GetScheduler(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                ip = "127.0.0.1";
            }
            if (!ConnectionCache.ContainsKey(ip))
            {
                var properties = new NameValueCollection();
                properties["quartz.scheduler.proxy"] = "true";
                properties["quartz.scheduler.proxy.address"] = $"{channelType}://{ip}:{port}/{bindName}";
                var schedulerFactory = new StdSchedulerFactory(properties);
                _scheduler = schedulerFactory.GetScheduler();
                ConnectionCache[ip] = _scheduler;
            }
            return ConnectionCache[ip];
        }

    }
}