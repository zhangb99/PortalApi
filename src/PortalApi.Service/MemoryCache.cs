using System;
using System.Runtime.Caching;

namespace PortalApi.Service
{
    public interface ICacheService
    {
        T Get<T>(string cacheId, Func<T> getItemCallback, int cacheMinutes = 5) where T : class;
        T Get<T>(string cacheId, Func<T> getItemCallback, out bool cached, int cacheMinutes = 5) where T : class;
        T GetFromCache<T>(string cacheId) where T : class;
        void UpdateCache(string cacheId, object item, int cacheMinutes = 5);
        void Remove(string cacheId);

    }

    public class MemoryCacheService : ICacheService
    {
        public T Get<T>(string cacheId, Func<T> getItemCallback, int cacheMinutes = 5)
            where T : class
        {
            bool cached = false;
            return Get<T>(cacheId, getItemCallback, out cached, cacheMinutes);
        }

        public T Get<T>(string cacheId, Func<T> getItemCallback, out bool cached, int cacheMinutes = 5) where T : class
        {
            T item = MemoryCache.Default.Get(cacheId) as T;
            cached = item != null;

            if (item == null)
            {
                item = getItemCallback();

                MemoryCache.Default.Add(cacheId, item,
                    new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddMinutes(cacheMinutes) });
            }
            return item;
        }

        public T GetFromCache<T>(string cacheId) where T : class
        {
            return MemoryCache.Default.Get(cacheId) as T;
        }

        public void UpdateCache(string cacheId, object item, int cacheMinutes = 5)
        {
            MemoryCache.Default.Remove(cacheId);
            MemoryCache.Default.Add(cacheId, item,
                    new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddMinutes(cacheMinutes) });
        }

        public void Remove(string cacheId)
        {
            MemoryCache.Default.Remove(cacheId);
        }
    }
}
