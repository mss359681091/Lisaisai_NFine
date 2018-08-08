using System;
using System.Data;

namespace Quartz.Net_RemoteServer
{
    /// <summary>
    /// 监听类
    /// </summary>
    public class MyJobListener : IJobListener
    {
        log4net.ILog _log = log4net.LogManager.GetLogger(typeof(MyJobListener));//获取一个日志记录器
        public MyJobListener()
        {

        }

        public string Name
        {
            get
            {
                return "customerJobListener";
            }
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {

        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            string jobId = "0";
            string strTriggerState = string.Empty;
            try
            {
                jobId = context.JobDetail.JobDataMap["jobId"].ToString();
                strTriggerState = context.Scheduler.GetTriggerState(context.Trigger.Key).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：{0}", ex.Message);
                _log.Error("任务编号： " + context.JobDetail.JobDataMap["jobId"] + " 执行时间： " + DateTime.Now + " 错误日志：" + ex.Message);
            }
            Console.WriteLine("任务编号{0}；开始执行时间：{1},状态：{2}", jobId, DateTime.Now, strTriggerState);
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            string jobId = "0";
            string strTriggerState = string.Empty;
            int triggerState = 0;

            try
            {
                jobId = context.JobDetail.JobDataMap["jobId"].ToString();
                strTriggerState = context.Scheduler.GetTriggerState(context.Trigger.Key).ToString();
                triggerState = _changeType(context.Scheduler.GetTriggerState(context.Trigger.Key));

                string strQuery = string.Empty;
                if (jobException == null)
                {
                    strQuery = @" UPDATE CUSTOMER_JOBINFO SET TRIGGERSTATE =" + triggerState + " WHERE 1=1 AND F_Id='" + jobId + "' ";
                    Console.WriteLine("任务编号{0}；执行完成时间：{1},状态：{2}", context.JobDetail.JobDataMap["jobId"], DateTime.Now, strTriggerState);
                    _log.Info("任务编号： " + context.JobDetail.JobDataMap["jobId"] + " 执行时间： " + DateTime.Now + "执行状态：" + strTriggerState);
                }
                else
                {
                    strQuery = @" UPDATE CUSTOMER_JOBINFO SET TRIGGERSTATE =" + triggerState + ",EXCEPTION= '" + jobException.Message + "' WHERE 1=1 AND F_Id='" + jobId + "' ";
                    Console.WriteLine("jobId{0}执行失败：{1}", context.JobDetail.JobDataMap["jobId"], jobException.Message);
                    _log.Error("任务编号： " + context.JobDetail.JobDataMap["jobId"] + " 执行时间： " + DateTime.Now + " 错误日志：" + jobException.Message);
                }
                if ((!string.IsNullOrEmpty(jobId)) && jobId != "0" && triggerState != 0)
                {
                    SqlHelper.ExecteNonQuery(CommandType.Text, strQuery);//保存状态
                }

            }
            catch (Exception ex)
            {
                string strQuery = @" UPDATE CUSTOMER_JOBINFO SET TRIGGERSTATE =" + triggerState + ",EXCEPTION= '" + jobException.Message + "' WHERE 1=1 AND F_Id='" + jobId + "' ";
                SqlHelper.ExecteNonQuery(CommandType.Text, strQuery);//保存状态
                Console.WriteLine("异常{0}", ex.Message);
                _log.Error("任务编号： " + context.JobDetail.JobDataMap["jobId"] + " 执行时间： " + DateTime.Now + " 错误日志：" + ex.Message);
            }
        }

        private int _changeType(TriggerState triggerState)
        {
            switch (triggerState)
            {
                case TriggerState.None: return -1;
                case TriggerState.Normal: return 0;
                case TriggerState.Paused: return 1;
                case TriggerState.Complete: return 2;
                case TriggerState.Error: return 3;
                case TriggerState.Blocked: return 4;
                default: return -1;
            }
        }
    }
}
