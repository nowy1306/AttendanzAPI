using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using AttendanzApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AttendanzApi.Hubs
{
    public static class ConnectionStorage
    {
        private static readonly ConcurrentDictionary<string, string> _connections =
            new ConcurrentDictionary<string, string>();

        public static void AddConnectionMapping(string cardCode, string connectionId)
        {
            _connections.TryAdd(cardCode, connectionId);
        }

        public static void RemoveConnectionMapping(string cardCode)
        {
            _connections.TryRemove(cardCode, out var id);
        }

        public static string GetConnectionId(string cardCode)
        {
            var res = _connections.TryGetValue(cardCode, out var connectionId);
            return connectionId;
        }
    }
}
