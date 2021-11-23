using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AttendanzApi.Extensions
{
    public static class SessionExtensions
    {
        public static void SetLong(this ISession session, string key, long value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static long? GetLong(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? null : JsonSerializer.Deserialize<long>(value);
        }
    }

    public static class SessionKeys
    {
        public static readonly string AccountId = "AccountId";
        public static readonly string AccountCardCode = "AccountCardCode";
    }

}
