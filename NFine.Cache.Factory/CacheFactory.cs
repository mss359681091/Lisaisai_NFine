using System.Configuration;

namespace NFine.Cache.Factory
{
    public class CacheFactory
    {
        /// <summary>
        /// 定义通用的Repository
        /// </summary>
        /// <returns></returns>
        public static ICache Cache()
        {
            //修改为支持Redis
            string cacheType = ConfigurationManager.AppSettings["CacheType"].ToString().Trim();
            switch (cacheType)
            {
                case "Redis":
                    return new Redis.Cache();
                case "WebCache":
                    return new Cache();
                default:
                    return new Cache();
            }
        }
    }
}
