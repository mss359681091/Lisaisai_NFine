using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Redis;
using NFine.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NFine.Web
{
    public class MessageQueueConfig
    {
        /// <summary>
        /// 开启线程不停的监听消息队列是否有消息未处理
        /// </summary>
        public static void RegisterExceptionLogQueue()
        {
            //通过线程池开启线程，不停地从队列中获取异常信息并将其写入日志文件
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    try
                    {
                        //从队列中出队，获取错误日志
                        if (RedisCache.GetListCount(RedisTypeEnum.ExceptionLog.ToString()) > 0)
                        {
                            string errorMsg = RedisCache.DequeueItemFromList(RedisTypeEnum.ExceptionLog.ToString());
                            WriteDbLog(errorMsg);
                        }
                        else if (RedisCache.GetListCount(RedisTypeEnum.OperateLog.ToString()) > 0)
                        {
                            //从队列中出队，获取操作日志
                            string operateMsg = RedisCache.DequeueItemFromList(RedisTypeEnum.OperateLog.ToString());
                            WriteDbLog(operateMsg);
                        }

                        else if (RedisCache.GetListCount(RedisTypeEnum.Email.ToString()) > 0)
                        {
                            //从队列中出队，邮件发送
                            string operateMsg = RedisCache.DequeueItemFromList(RedisTypeEnum.Email.ToString());
                        }
                        else if (RedisCache.GetListCount(RedisTypeEnum.SMS.ToString()) > 0)
                        {
                            //从队列中出队，短信发送
                            string operateMsg = RedisCache.DequeueItemFromList(RedisTypeEnum.SMS.ToString());
                        }
                        else if (RedisCache.GetListCount(RedisTypeEnum.IMGConversion.ToString()) > 0)
                        {
                            //从队列中出队，图片转换
                            string operateMsg = RedisCache.DequeueItemFromList(RedisTypeEnum.IMGConversion.ToString());
                        }
                        else if (RedisCache.GetListCount(RedisTypeEnum.LoginLog.ToString()) > 0)
                        {
                            //从队列中出队，登录日志
                            string loginMsg = RedisCache.DequeueItemFromList(RedisTypeEnum.LoginLog.ToString());
                            WriteDbLog(loginMsg);
                        }
                        else
                        {
                            Thread.Sleep(1000); //为避免CPU空转，在队列为空时休息1秒
                        }
                    }
                    catch (Exception ex)
                    {
                        //SendMail(ex.ToString());//发送报告
                        RedisCache.EnqueueItemOnList(RedisTypeEnum.ExceptionLog.ToString(), ex.ToString());
                    }
                }
            });
        }

        /// <summary>
        /// 写入数据库日志
        /// </summary>
        /// <param name="errorMsg">拼接字符串</param>
        private static void WriteDbLog(string errorMsg)
        {
            if (!string.IsNullOrEmpty(errorMsg))
            {
                if (errorMsg.Contains(";"))
                {
                    string[] strMsg = errorMsg.Split(';');
                    new LogApp().WriteDbLog(new LogEntity
                    {
                        F_ModuleId = strMsg[0],
                        F_ModuleName = strMsg[1],
                        F_Type = strMsg[2],
                        F_Result = true,
                        F_Description = strMsg[3],
                        F_Account = strMsg[4],
                        F_NickName = strMsg[5],
                    });
                }
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        private static void SendMail(string body)
        {
            string ErrorToMail = Configs.GetValue("ErrorToMail");
            if (ErrorToMail == "true")
            {
                MailHelper mail = new MailHelper();
                mail.MailServer = Configs.GetValue("MailHost");
                mail.MailUserName = Configs.GetValue("MailUserName");
                mail.MailPassword = Configs.GetValue("MailPassword");
                mail.MailName = Configs.GetValue("SoftName");
                mail.Send("359681091@qq.com", "消息队列", body);
            }
        }
    }
}