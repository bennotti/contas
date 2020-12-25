using Microsoft.AspNetCore.SignalR;

namespace Contas.Core.SignalRHub
{
    public class HubUsers
    {
        public HubUsers()
        {

        }

        public HubUsers(string connectionId, string userId, IHubCallerClients clientsSignalR)
        {
            this.connectionId = connectionId;
            this.userId = userId;
            this.clientsSignalR = clientsSignalR;
        }

        public string connectionId { get; set; }
        public string userId { get; set; }
        public IHubCallerClients clientsSignalR { get; set; }
    }
}
