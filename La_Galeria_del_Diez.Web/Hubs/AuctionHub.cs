using Microsoft.AspNetCore.SignalR;

namespace La_Galeria_del_Diez.Web.Hubs
{
    public class AuctionHub : Hub
    {
        public Task JoinAuctionGroup(string auctionId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, $"auction-{auctionId}");
        }

        public Task LeaveAuctionGroup(string auctionId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, $"auction-{auctionId}");
        }
    }
}
