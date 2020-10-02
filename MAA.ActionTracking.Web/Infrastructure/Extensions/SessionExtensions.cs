using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Infrastructure.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }

        public static async Task SetAsync<T>(this ISession session, string key, T value)
        {
            if (!session.IsAvailable)
                await session.LoadAsync();

            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static async Task<T> GetAsync<T>(this ISession session, string key)
        {
            if (!session.IsAvailable)
                await session.LoadAsync();

            var value = session.GetString(key);

            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }

        //public static async Task RemoveAsync<T>(this ISession session, string key)
        //{
        //    if (session.IsAvailable)
        //        await session.RemoveAsync(key);
        //}
    }
}
