using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace La_Galeria_del_Diez.Web.Services
{
    public class AuctionClosingBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<AuctionHub> _auctionHub;
        private readonly ILogger<AuctionClosingBackgroundService> _logger;

        public AuctionClosingBackgroundService(
            IServiceScopeFactory scopeFactory,
            IHubContext<AuctionHub> auctionHub,
            ILogger<AuctionClosingBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _auctionHub = auctionHub;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var auctionService = scope.ServiceProvider.GetRequiredService<IServiceAuction>();

                    var closedAuctionIds = await auctionService.CloseExpiredAuctionsAsync(DateTime.Now);
                    if (closedAuctionIds.Count > 0)
                    {
                        foreach (var auctionId in closedAuctionIds)
                        {
                            var auction = await auctionService.FindByIdAsync(auctionId);
                            if (auction == null)
                            {
                                continue;
                            }

                            var currentPrice = auction.Biddings?.Any() == true
                                ? auction.Biddings.Max(b => b.Amount)
                                : auction.BasePrice;

                            var leadingBid = auction.Biddings?
                                .OrderByDescending(b => b.Amount)
                                .ThenByDescending(b => b.RegistrationDate)
                                .FirstOrDefault();

                            var bidHistory = auction.Biddings?
                                .OrderByDescending(b => b.RegistrationDate)
                                .Select(b => new
                                {
                                    idUser = b.IdUser,
                                    userName = b.UserName,
                                    amount = b.Amount,
                                    registrationDate = b.RegistrationDate.ToString("dd/MM/yyyy HH:mm:ss")
                                })
                                .Cast<object>()
                                .ToList() ?? new List<object>();

                            await _auctionHub.Clients.Group($"auction-{auctionId}").SendAsync("AuctionClosed", new
                            {
                                auctionId,
                                totalBids = auction.Biddings?.Count ?? 0,
                                currentPrice,
                                leaderUserId = leadingBid?.IdUser ?? 0,
                                leaderUser = !string.IsNullOrWhiteSpace(leadingBid?.UserName)
                                    ? leadingBid!.UserName
                                    : (leadingBid != null ? $"User #{leadingBid.IdUser}" : "No bids yet"),
                                history = bidHistory
                            }, stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while auto-closing auctions.");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
