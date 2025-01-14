using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ProductService
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache cache;
        public RedisCacheService(IDistributedCache _cache)
        {
            cache = _cache;
        }
        public T? GetData<T>(string key)
        {
            var data = cache.GetString(key);

            if (data == null)
            {
                return default(T?);
            }

            return JsonSerializer.Deserialize<T>(data);
        }

        public void RemoveData<T>(string key)
        {
            cache.Remove(key);
        }

        public void SetData<T>(string key, T value)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            cache.SetString(key,JsonSerializer.Serialize(value),options);


        }
    }
}
