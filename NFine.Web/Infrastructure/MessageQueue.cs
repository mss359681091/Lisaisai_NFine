using NFine.Application;
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Redis;

namespace NFine.Web.Infrastructure
{
    public class MessageQueue
    {
        /// <summary>
        /// 操作日志和错误日志
        /// </summary>
        /// <param name="errMsg">内容</param>
        /// <param name="moduleName">模块路径</param>
        /// <param name="moduleId">模块名称</param>
        /// <param name="type">操作类型</param>
        /// <param name="redistype">队列类型</param>
        public static void QueueList(string errMsg, string moduleName, string moduleId, DbLogType type, RedisTypeEnum redistype)
        {
            string cacheType = Configs.GetValue("CacheType");//缓存类型
            switch (cacheType)
            {
                case "Redis":
                    errMsg = errMsg.Replace(";", "");
                    errMsg = moduleId + ";" + moduleName + ";" + type.ToString() + ";" + errMsg + ";" + "" + ";" + "";
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

                            //F_Account = OperatorProvider.Provider.GetCurrent().UserCode,
                            //F_NickName = OperatorProvider.Provider.GetCurrent().UserName,

                            F_Result = true,
                            F_Description = errMsg,
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 缩略图压缩
        /// </summary>
        /// <param name="oPath">原物理路径</param>
        /// <param name="tPath">目标物理路径</param>
        /// <param name="redistype"></param>
        public static void MakeThumbnailList(string oPath, string tPath, RedisTypeEnum redistype)
        {
            string cacheType = Configs.GetValue("CacheType");//缓存类型
            switch (cacheType)
            {
                case "Redis":
                    string msg = oPath + ";" + tPath;
                    RedisCache.EnqueueItemOnList(redistype.ToString(), msg);//操作消息入队   
                    break;
                case "WebCache":
                    NFine.Code.Common.MakeThumbnail(oPath, tPath, 750, 750, "W", "jpg");//生成缩略图
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// IO文件删除
        /// </summary>
        /// <param name="Path">物理路径</param>
        /// <param name="redistype"></param>
        /// <param name="fileType">文件类型（0文件1文件夹）</param>
        public static void IODel(string Path, RedisTypeEnum redistype, int fileType = 0)
        {
            string cacheType = Configs.GetValue("CacheType");//缓存类型
            switch (cacheType)
            {
                case "Redis":
                    RedisCache.EnqueueItemOnList(redistype.ToString(), fileType + ";" + Path);//操作消息入队   
                    break;
                case "WebCache":
                    if (fileType == 0)
                    {
                        FileHelper.DeleteFile(Path);//物理删除
                    }
                    else
                    {
                        FileHelper.DeleteDirectory(Path);//物理删除
                    }

                    break;
                default:
                    break;
            }
        }
    }
}