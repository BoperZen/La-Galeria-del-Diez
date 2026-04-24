using La_Galeria_del_Diez.Application.DTOs.Reports;
using La_Galeria_del_Diez.Application.Interfaces;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace La_Galeria_del_Diez.Application.Services.Implementatios
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IReportRepository reportRepository, ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<AuctionSummaryReportDto>> GetAuctionSummaryAsync()
        {
            _logger.LogInformation("Starting GetAuctionSummaryAsync");
            try
            {
                var auctions = await _reportRepository.GetAuctionSummaryAsync();
                var result = auctions.Select(a => new AuctionSummaryReportDto
                {
                    AuctionTitle = a.IdObjectNavigation.Name,
                    ObjectName = a.IdObjectNavigation.Name,
                    Status = a.IdStateNavigation.Description,
                    BasePrice = a.BasePrice,
                    CurrentPrice = a.Bidding.Any() ? a.Bidding.Max(b => b.Amount) : a.BasePrice,
                    TotalBids = a.Bidding.Count,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate
                });

                _logger.LogInformation("Finished GetAuctionSummaryAsync with {Count} records", result.Count());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAuctionSummaryAsync");
                throw;
            }
        }

        public async Task<IEnumerable<MostActiveAuctionDto>> GetMostActiveAuctionsAsync()
        {
            _logger.LogInformation("Starting GetMostActiveAuctionsAsync");
            try
            {
                var auctions = await _reportRepository.GetMostActiveAuctionsAsync();

                var ordered = auctions
                    .OrderByDescending(a => a.Bidding.Count)
                    .ThenByDescending(a => a.StartDate)
                    .ToList();

                var result = ordered.Select((a, index) =>
                {
                    var highestBid = a.Bidding.OrderByDescending(b => b.Amount).FirstOrDefault();
                    return new MostActiveAuctionDto
                    {
                        Rank = index + 1,
                        AuctionTitle = a.IdObjectNavigation.Name,
                        ObjectName = a.IdObjectNavigation.Name,
                        Status = a.IdStateNavigation.Description,
                        TotalBids = a.Bidding.Count,
                        HighestBid = highestBid?.Amount ?? a.BasePrice,
                        LastBidder = highestBid?.IdUserNavigation.Username ?? "-"
                    };
                });

                _logger.LogInformation("Finished GetMostActiveAuctionsAsync with {Count} records", result.Count());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMostActiveAuctionsAsync");
                throw;
            }
        }

        public async Task<IEnumerable<BidHistoryReportDto>> GetBidHistoryAsync(DateTime? startDate = null, DateTime? endDateExclusive = null)
        {
            _logger.LogInformation("Starting GetBidHistoryAsync");
            try
            {
                var bids = await _reportRepository.GetBidHistoryAsync(startDate, endDateExclusive);
                var maxBidByAuctionId = bids
                    .GroupBy(b => b.IdAuction)
                    .ToDictionary(g => g.Key, g => g.Max(x => x.Amount));

                var result = bids
                    .OrderByDescending(b => b.RegistrationDate)
                    .Select(b =>
                    {
                        var auction = b.IdAuctionNavigation;
                        var currentMax = maxBidByAuctionId.TryGetValue(b.IdAuction, out var maxBid)
                            ? maxBid
                            : auction.BasePrice;
                        var status = b.Amount == currentMax ? "Ganadora actual" : "Superada";

                        return new BidHistoryReportDto
                        {
                            AuctionTitle = auction.IdObjectNavigation?.Name ?? $"Subasta #{auction.Id}",
                            ObjectName = auction.IdObjectNavigation?.Name ?? "-",
                            Bidder = b.IdUserNavigation?.Username ?? $"Usuario #{b.IdUser}",
                            Amount = b.Amount,
                            Timestamp = b.RegistrationDate,
                            BidStatus = status
                        };
                    });

                _logger.LogInformation("Finished GetBidHistoryAsync with {Count} records", result.Count());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBidHistoryAsync");
                throw;
            }
        }

        public async Task<IEnumerable<TopValuedItemDto>> GetTopValuedItemsAsync()
        {
            _logger.LogInformation("Starting GetTopValuedItemsAsync");
            try
            {
                var objects = await _reportRepository.GetTopValuedItemsAsync();
                var now = DateTime.Now;

                var result = objects.Select(o =>
                {
                    var auctions = o.Auction;
                    var maxPrice = auctions.Any()
                        ? auctions.Max(a => a.Bidding.Any() ? a.Bidding.Max(b => b.Amount) : a.BasePrice)
                        : 0m;

                    var basePrice = auctions.Any() ? auctions.Min(a => a.BasePrice) : 0m;
                    var isActive = auctions.Any(a => a.IdState == 2 && a.EndDate > now);

                    return new TopValuedItemDto
                    {
                        ItemName = o.Name,
                        Description = o.Description ?? string.Empty,
                        BasePrice = basePrice,
                        MaxPrice = maxPrice,
                        AuctionCount = auctions.Count,
                        CurrentStatus = isActive ? "En subasta" : "Fuera de subasta"
                    };
                })
                .OrderByDescending(x => x.MaxPrice)
                .Select((x, index) =>
                {
                    x.Rank = index + 1;
                    return x;
                });

                _logger.LogInformation("Finished GetTopValuedItemsAsync with {Count} records", result.Count());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTopValuedItemsAsync");
                throw;
            }
        }
    }
}
