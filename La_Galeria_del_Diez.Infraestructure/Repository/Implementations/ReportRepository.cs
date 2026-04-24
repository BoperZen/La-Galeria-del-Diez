using La_Galeria_del_Diez.Infraestructure.Data;
using La_Galeria_del_Diez.Infraestructure.Models;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly La_Galeria_del_Diez_Context _context;

        public ReportRepository(La_Galeria_del_Diez_Context context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Auction>> GetAuctionSummaryAsync()
        {
            return await _context.Set<Auction>()
                .Include(a => a.IdObjectNavigation)
                .Include(a => a.IdStateNavigation)
                .Include(a => a.Bidding)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Auction>> GetMostActiveAuctionsAsync()
        {
            return await _context.Set<Auction>()
                .Include(a => a.IdObjectNavigation)
                .Include(a => a.IdStateNavigation)
                .Include(a => a.Bidding)
                    .ThenInclude(b => b.IdUserNavigation)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Bidding>> GetBidHistoryAsync(DateTime? startDate = null, DateTime? endDateExclusive = null)
        {
            var query = _context.Set<Bidding>()
                .Include(b => b.IdAuctionNavigation)
                    .ThenInclude(a => a.IdObjectNavigation)
                .Include(b => b.IdUserNavigation)
                .AsSplitQuery()
                .AsNoTracking();

            if (startDate.HasValue)
            {
                query = query.Where(b => b.RegistrationDate >= startDate.Value);
            }

            if (endDateExclusive.HasValue)
            {
                query = query.Where(b => b.RegistrationDate < endDateExclusive.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<AuctionableObject>> GetTopValuedItemsAsync()
        {
            return await _context.Set<AuctionableObject>()
                .Include(o => o.Auction)
                    .ThenInclude(a => a.Bidding)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
