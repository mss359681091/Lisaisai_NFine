using System;
namespace NFine.Code
{
    public interface ICache
    {
        T GetCache<T>(string cacheKey) where T : class;
        void WriteCache<T>(T value, string cacheKey) where T : class;
        void WriteCache<T>(T value, string cacheKey, DateTime expireTime) where T : class;
        void WriteCache<T>(T value, string cacheKey, string database, string table) where T : class;
        void WriteCache<T>(T value, string cacheKey, string folder) where T : class;
        void RemoveCache(string cacheKey);
        void RemoveCache();
    }
}
