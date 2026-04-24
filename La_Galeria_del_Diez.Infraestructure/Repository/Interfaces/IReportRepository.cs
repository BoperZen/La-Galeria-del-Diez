using La_Galeria_del_Diez.Infraestructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Interfaces
{
    public interface IReportRepository
    {
        Task<IReadOnlyList<Auction>> GetAuctionSummaryAsync();
        Task<IReadOnlyList<Auction>> GetMostActiveAuctionsAsync();
        Task<IReadOnlyList<Bidding>> GetBidHistoryAsync(DateTime? startDate = null, DateTime? endDateExclusive = null);
        Task<IReadOnlyList<AuctionableObject>> GetTopValuedItemsAsync();
    }
}
