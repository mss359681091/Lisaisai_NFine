using Client.Common;
using Server.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace Quartz.Net_JobBase
{
    public class JobBase : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ThreadPool.QueueUserWorkItem(delegate (Object o)
            {
                try
                {
                    CallUrl(context);//Token约定方式调用WebApi服务
                }
                catch (Exception ex)
                {
                    JobExecutionException e2 = new JobExecutionException(ex);
                    e2.RefireImmediately = true;
                    throw e2;
                }
            });
        }

        #region 常规方式调用Url  CallUrl(IJobExecutionContext context)
        /// <summary>
        /// 常规方式调用Url
        /// </summary>
        /// <param name="context"></param>
        private static void CallUrl(IJobExecutionContext context)
        {
            HttpClient hc = new HttpClient();
            hc.GetAsync(context.JobDetail.JobDataMap["requestUrl"].ToString());
        }
        #endregion

        #region Token约定方式调用WebApi服务  CallWebApi(IJobExecutionContext context)
        /// <summary>
        /// Token约定方式调用WebApi服务  
        /// </summary>
        /// <param name="context"></param>
        private static void CallWebApi(IJobExecutionContext context)
        {
            var url = context.JobDetail.JobDataMap["requestUrl"].ToString();//服务url
            var api = context.JobDetail.JobDataMap["webApi"].ToString();//webapi
            var id = "10000";//密钥
                             //规范url格式
            if (!url.Contains("http"))
            {
                url = "http://" + url;
            }
            //规范api格式
            if (!api.Contains("http"))
            {
                api = "http://" + api;
            }
            var tokenResult = WebApiHelper.GetSignToken(id, api);//调用api拿到token参数
            Dictionary<string, string> parames = StringHelper.getUrlParams(url);//获取url参数
            Tuple<string, string> parameters = WebApiHelper.GetQueryString(parames);//拼接get参数
            WebApiHelper.Get<object>(url, parameters.Item1, parameters.Item2, id, api);//token约定方式调用webapi
        }
        #endregion
    }
}
