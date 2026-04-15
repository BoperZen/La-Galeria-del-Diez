using La_Galeria_del_Diez.Infraestructure.Data;
using La_Galeria_del_Diez.Infraestructure.Models;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Implementations
{
    public class RepositoryAuction : IRepositoryAuction
    {
        private readonly La_Galeria_del_Diez_Context _context;

        public RepositoryAuction(La_Galeria_del_Diez_Context context)
        {
            _context = context;
        }

        public async Task<Auction> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Auction>()
                                        .Include(a => a.IdObjectNavigation)
                                            .ThenInclude(o => o.Image)
                                        .Include(a => a.IdUserNavigation)
                                        .Include(a => a.IdStateNavigation)
                                        .Include(a => a.Winner)
                                        .Include(a => a.Bidding)
                                            .ThenInclude(b => b.IdUserNavigation)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(a => a.Id == id);
            return @object!;
        }

        public async Task<ICollection<Auction>> ListAsync()
        {
            var collection = await _context.Set<Auction>()
                                           .Include(a => a.IdObjectNavigation)
                                               .ThenInclude(o => o.Image)
                                           .Include(a => a.IdUserNavigation)
                                           .Include(a => a.IdStateNavigation)
                                           .Include(a => a.Bidding)
                                           .AsSplitQuery()
                                           .AsNoTracking()
                                           .ToListAsync();
            return collection;
        }

        public async Task<ICollection<Auction>> ListActiveAsync(DateTime now)
        {
            var collection = await _context.Set<Auction>()
                                           .Include(a => a.IdObjectNavigation)
                                               .ThenInclude(o => o.Image)
                                           .Include(a => a.IdUserNavigation)
                                           .Include(a => a.IdStateNavigation)
                                           .Include(a => a.Bidding)
                                           .AsSplitQuery()
                                           .AsNoTracking()
                                           .Where(a => a.EndDate > now && a.IdState == 2)
                                           .ToListAsync();
            return collection;
        }

        public async Task<ICollection<Auction>> ListDraftAsync(DateTime now)
        {
            var collection = await _context.Set<Auction>()
                                           .Include(a => a.IdObjectNavigation)
                                               .ThenInclude(o => o.Image)
                                           .Include(a => a.IdUserNavigation)
                                           .Include(a => a.IdStateNavigation)
                                           .Include(a => a.Bidding)
                                           .AsSplitQuery()
                                           .AsNoTracking()
                                           .Where(a => a.StartDate >= now && a.IdState == 1)
                                           .ToListAsync();
            return collection;
        }

        public async Task<ICollection<Auction>> ListPendingPaymentAsync()
        {
            var collection = await _context.Set<Auction>()
                                           .Include(a => a.IdObjectNavigation)
                                               .ThenInclude(o => o.Image)
                                           .Include(a => a.IdUserNavigation)
                                           .Include(a => a.IdStateNavigation)
                                           .Include(a => a.Winner)
                                           .Include(a => a.Bidding)
                                               .ThenInclude(b => b.IdUserNavigation)
                                           .AsSplitQuery()
                                           .AsNoTracking()
                                           .Where(a => a.IdState == 3)
                                           .ToListAsync();
            return collection;
        }

        public async Task<ICollection<Auction>> ListFinishedAsync(DateTime now)
        {
            var collection = await _context.Set<Auction>()
                                           .Include(a => a.IdObjectNavigation)
                                               .ThenInclude(o => o.Image)
                                           .Include(a => a.IdUserNavigation)
                                           .Include(a => a.IdStateNavigation)
                                           .Include(a => a.Bidding)
                                           .AsSplitQuery()
                                           .AsNoTracking()
                                           .Where(a => a.EndDate <= now)
                                           .ToListAsync();
            return collection;
        }

        public async Task<ICollection<int>> CloseExpiredAuctionsAsync(DateTime now)
        {
            var finalizedStateId = await _context.Set<State>()
                .Where(s => EF.Functions.Like(s.Description, "%Final%"))
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            if (finalizedStateId <= 0)
            {
                finalizedStateId = 3;
            }

            var expiredAuctions = await _context.Set<Auction>()
                .Include(a => a.Bidding)
                .Where(a => a.IdState == 2 && a.EndDate <= now)
                .ToListAsync();

            if (!expiredAuctions.Any())
            {
                return Array.Empty<int>();
            }

            foreach (var auction in expiredAuctions)
            {
                auction.IdState = finalizedStateId;

                var winnerBid = auction.Bidding
                    .OrderByDescending(b => b.Amount)
                    .ThenByDescending(b => b.RegistrationDate)
                    .FirstOrDefault();

                auction.AuctionWinner = winnerBid?.IdUser;
            }

            await _context.SaveChangesAsync();

            return expiredAuctions.Select(a => a.Id).ToList();
        }

        public async Task AddAsync(Auction auction)
        {
            _context.Set<Auction>().Add(auction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Auction auction)
        {
            if (_context.Entry(auction).State == EntityState.Detached)
            {
                _context.Attach(auction);
            }

            // Si el mapping ya actualizó propiedades escalares, esto garantiza update
            _context.Entry(auction).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Auction auction)
        {
            if (_context.Entry(auction).State == EntityState.Detached)
            {
                _context.Attach(auction);
            }

            _context.Entry(auction).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateStateAsync(int id, int stateId)
        {
            var entity = new Auction { Id = id, IdState = stateId };

            _context.Attach(entity);
            _context.Entry(entity).Property(a => a.IdState).IsModified = true;

            await _context.SaveChangesAsync();
        }
    }
}