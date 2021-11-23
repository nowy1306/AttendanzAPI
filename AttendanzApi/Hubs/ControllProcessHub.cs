using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using AttendanzApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AttendanzApi.Hubs
{
    public class ControllProcessHub : Hub
    {
        
        private readonly ILogger<ControllProcessHub> _logger;

        public ControllProcessHub(ILogger<ControllProcessHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            string accountCardCode = GetAccountCardCode();

            ConnectionStorage.AddConnectionMapping(accountCardCode, Context.ConnectionId);
            _logger.LogInformation($"{accountCardCode} {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception e)
        {
            string accountCardCode = GetAccountCardCode();

            ConnectionStorage.RemoveConnectionMapping(accountCardCode);
            return base.OnDisconnectedAsync(e);
        }

        private string GetAccountCardCode()
        {
            return Context.GetHttpContext()
                .Session.GetString(SessionKeys.AccountCardCode);
        }

    }
}
