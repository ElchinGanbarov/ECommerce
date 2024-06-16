using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Extensions
{
    public static class CacheExtensions
    {
        public static void Set(this IMemoryCache cache, object key, object content, int durationInSeconds, int? slidingInSeconds = null)
        {
            cache.Set(key, content, DateTimeOffset.Now + TimeSpan.FromSeconds(durationInSeconds), slidingInSeconds);
        }

        public static void Set(this IMemoryCache cache, object key, object content, DateTimeOffset? absoluteExpiration = null, int? slidingInSeconds = null)
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = (absoluteExpiration ?? ((DateTimeOffset)(DateTime.Now + TimeSpan.FromSeconds(60.0)))),
                SlidingExpiration = (slidingInSeconds.HasValue ? new TimeSpan?(TimeSpan.FromSeconds(slidingInSeconds.Value)) : null),
                Priority = CacheItemPriority.Low
            };
            cache.Set(key, content, options);
        }

        public static T TryGetValue<T>(this IMemoryCache cache, string key, T defaultValue = default(T))
        {
            if (cache.TryGetValue(key, out object value))
            {
                return (T)value;
            }

            return defaultValue;
        }

        public static async Task<TItem> GetOrCreateAsync<TItem>(this IMemoryCache cache, object key, Func<ICacheEntry, Task<TItem>> factory, bool bypass, int durationInSeconds, int? slidingInSeconds = null)
        {
            cache.RemoveIfBypass(key, bypass);
            if (!cache.TryGetValue(key, out object value))
            {
                ICacheEntry entry = cache.CreateEntry(key);
                entry.AbsoluteExpiration = DateTimeOffset.Now + TimeSpan.FromSeconds(durationInSeconds);
                if (slidingInSeconds.HasValue)
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(slidingInSeconds.Value);
                }

                value = await factory(entry);
                entry.SetValue(value);
                entry.Dispose();
            }

            return (TItem)value;
        }

        public static void RemoveIfBypass(this IMemoryCache cache, object key, bool bypass = true)
        {
            if (bypass)
            {
                cache.Remove(key);
            }
        }
    }

}
