using NFine.Cache;
using System;
using System.Web;
using System.Web.Caching;

namespace NFine.Redis
{
    /// <summary>
    /// 版 本 0.1
    /// 创建人：李赛赛
    /// 日 期：2017.10.24 10:45
    /// 描 述：定义缓存接口
    /// </summary>
    public class Cache : ICache
    {
        private static System.Web.Caching.Cache cache = HttpRuntime.Cache;

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public T GetCache<T>(string cacheKey) where T : class
        {
            return RedisCache.Get<T>(cacheKey);
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        public void WriteCache<T>(T value, string cacheKey) where T : class
        {
            //RedisCache.Set(cacheKey, value);
            //配置成与webcache相同时间
            WriteCache(value, cacheKey, DateTime.Now.AddMinutes(10));
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        public void WriteCache<T>(T value, string cacheKey, DateTime expireTime) where T : class
        {
            RedisCache.Set(cacheKey, value, expireTime);
        }

        /// <summary>
        /// 数据库缓存依赖
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">存储值</param>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="database">数据库名称</param>
        /// <param name="table">表名称</param>
        public void WriteCache<T>(T value, string cacheKey, string database, string table) where T : class
        {
            //制定缓存策略
            SqlCacheDependency scd = new SqlCacheDependency(database, table);
            //插入缓存
            cache.Insert(cacheKey, value, scd);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public void RemoveCache(string cacheKey)
        {
            RedisCache.Remove(cacheKey);
        }
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public void RemoveCache()
        {
            RedisCache.RemoveAll();
        }
    }
}
