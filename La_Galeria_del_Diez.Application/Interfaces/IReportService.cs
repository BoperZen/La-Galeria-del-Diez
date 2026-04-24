using La_Galeria_del_Diez.Application.DTOs.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<AuctionSummaryReportDto>> GetAuctionSummaryAsync();
        Task<IEnumerable<MostActiveAuctionDto>> GetMostActiveAuctionsAsync();
        Task<IEnumerable<BidHistoryReportDto>> GetBidHistoryAsync(DateTime? startDate = null, DateTime? endDateExclusive = null);
        Task<IEnumerable<TopValuedItemDto>> GetTopValuedItemsAsync();
    }
}
