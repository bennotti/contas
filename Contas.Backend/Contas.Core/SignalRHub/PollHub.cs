using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contas.Core.SignalRHub
{
    public class PollHub : Hub
    {
        private IHubCallerClients _clientsSignalR;
        private IMemoryCache _cache;
        private const string cacheUserHubs = "cache:userHubs";

        public PollHub(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public List<HubUsers> GetConnectionsHubs()
        {
            List<HubUsers> cacheEntry;
            if (!_cache.TryGetValue(cacheUserHubs, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = new List<HubUsers>();


                var cacheEntryOptions = new MemoryCacheEntryOptions()
              // Keep in cache for this time, reset time if accessed.
              .SetSlidingExpiration(TimeSpan.FromDays(3));
                // Save data in cache.
                _cache.Set(cacheUserHubs, cacheEntry, cacheEntryOptions);
            }
            return cacheEntry;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public List<HubUsers> ConnectionsHubs(string connectionId = null, string userId = null, IHubCallerClients _clientsSignalR = null, bool isRemove = false)
        {
            List<HubUsers> cacheEntry;
            if (!_cache.TryGetValue(cacheUserHubs, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = new List<HubUsers>();

                if (!string.IsNullOrEmpty(connectionId))
                    cacheEntry.Add(new HubUsers(connectionId, userId, _clientsSignalR));

                SaveCacheConnectionHubs(cacheEntry);
            }

            if (!string.IsNullOrEmpty(connectionId))
            {
                if (!isRemove)
                {
                    if (!cacheEntry.Any(x => x.connectionId.Equals(connectionId)))
                    {
                        cacheEntry.Add(new HubUsers(connectionId, userId, _clientsSignalR));
                        SaveCacheConnectionHubs(cacheEntry);
                    }
                    else if (!string.IsNullOrEmpty(userId))
                    {
                        var _remove = cacheEntry.FirstOrDefault(x => x.connectionId.Equals(connectionId));
                        cacheEntry.Remove(_remove);
                        cacheEntry.Add(new HubUsers(connectionId, userId, _clientsSignalR));
                        SaveCacheConnectionHubs(cacheEntry);
                    }
                }
                else
                {
                    var _remove = cacheEntry.FirstOrDefault(x => x.connectionId.Equals(connectionId));
                    cacheEntry.Remove(_remove);
                    SaveCacheConnectionHubs(cacheEntry);
                }
            }

            return cacheEntry;
        }
        public void SaveCacheConnectionHubs(List<HubUsers> cacheEntry)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(3));
            _cache.Set(cacheUserHubs, cacheEntry, cacheEntryOptions);
        }


        public override async Task OnConnectedAsync()
        {

            ConnectionsHubs(Context.ConnectionId, string.Empty, Clients);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ConnectionsHubs(Context.ConnectionId, null, Clients, true);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task StartConnectionHub(string user)
        {
            _clientsSignalR = Clients;

            var _usersSignal = ConnectionsHubs(Context.ConnectionId, user, Clients);

            await _clientsSignalR.Client(Context.ConnectionId).SendAsync("ReceiveStartConnectionHub", user, "conectado!");
        }
    }
}
