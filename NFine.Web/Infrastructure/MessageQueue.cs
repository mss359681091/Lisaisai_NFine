using NFine.Application;
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Redis;

namespace NFine.Web.Infrastructure
{
    public class MessageQueue
    {
        public static void QueueList(string errMsg, string moduleName, string moduleId, DbLogType type, RedisTypeEnum redistype)
        {
            string cacheType = Configs.GetValue("CacheType");//缓存类型
            switch (cacheType)
            {
                case "Redis":
                    errMsg = errMsg.Replace(";", "");
                    errMsg = moduleId + ";" + moduleName + ";" + type.ToString() + ";" + errMsg + ";" + OperatorProvider.Provider.GetCurrent().UserCode + ";" + OperatorProvider.Provider.GetCurrent().UserName;
                    RedisCache.EnqueueItemOnList(redistype.ToString(), errMsg);//操作消息入队   
                    break;
                case "WebCache":
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        new LogApp().WriteDbLog(new LogEntity
                        {
                            F_ModuleId = moduleId,
                            F_ModuleName = moduleName,
                            F_Type = type.ToString(),
                            F_Account = OperatorProvider.Provider.GetCurrent().UserCode,
                            F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                            F_Result = true,
                            F_Description = errMsg,
                        });
                    }
                    break;
                default:
                    break;
            }
        }
    }
}