using Quartz.Impl;
using Quartz.Impl.Matchers;
using System.Collections.Specialized;
using Topshelf;

namespace Quartz.Net_RemoteServer
{
    public sealed class ServiceRunner : ServiceControl, ServiceSuspend
    {
        private readonly IScheduler scheduler;

        public ServiceRunner()
        {
            var schedulerFactory = new StdSchedulerFactory(GetProperties());
            scheduler = schedulerFactory.GetScheduler();
            scheduler.ListenerManager.AddJobListener(new MyJobListener(), GroupMatcher<JobKey>.AnyGroup());
            scheduler.Start();
        }

        private NameValueCollection GetProperties()
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteServer";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";//端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";//名称
            properties["quartz.scheduler.exporter.channelType"] = "tcp";//通道名
            properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "true";
            properties["quartz.jobStore.clustered"] = "true";//集群配置
            properties["quartz.scheduler.instanceId"] = "AUTO";
            string connString = System.Configuration.ConfigurationManager.AppSettings["connString"];
            //下面为指定quartz持久化数据库的配置
            //string connString = "User ID=CQMS;Password=CQMS;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 124.205.46.149)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = orcl)))";
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz"; //存储类型
            properties["quartz.jobStore.tablePrefix"] = "QRTZ_"; //表明前缀
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
            properties["quartz.jobStore.dataSource"] = "myDS";//数据源名称
            properties["quartz.dataSource.myDS.connectionString"] = connString; //连接字符串
           //properties["quartz.dataSource.myDS.provider"] = "OracleODPManaged-1123-40";//oracle版本
            properties["quartz.dataSource.myDS.provider"] = "SqlServer-20";//sql版本
            return properties;
        }

        public bool Start(HostControl hostControl)
        {
            scheduler.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            scheduler.Shutdown(false);
            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            scheduler.ResumeAll();
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            scheduler.PauseAll();
            return true;
        }


    }
}
