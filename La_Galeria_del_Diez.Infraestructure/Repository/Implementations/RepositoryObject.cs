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
    public class RepositoryObject : IRepositoryObject
    {
        private readonly La_Galeria_del_Diez_Context _context;

        public RepositoryObject(La_Galeria_del_Diez_Context context)
        {
            _context = context;
        }

        public async Task<AuctionableObject> FindByIdAsync(int id)
        {
            var @object = await _context.Set<AuctionableObject>()
                                        .Include(o => o.Image)
                                        .Include(o => o.IdCategory)
                                        .Include(o => o.IdStateNavigation)
                                        .Include(o => o.IdUserNavigation)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(o => o.Id == id);
            
            if (@object != null)
            {
                // Cargar auctions separadamente
                @object.Auction = await _context.Set<Auction>()
                    .Where(a => a.IdObject == id)
                    .Include(a => a.IdUserNavigation)
                    .Include(a => a.IdStateNavigation)
                    .Include(a => a.Bidding)
                        .ThenInclude(b => b.IdUserNavigation)
                    .AsNoTracking()
                    .ToListAsync();
            }
            
            return @object!;
        }

        public async Task<ICollection<AuctionableObject>> ListAsync()
        {
            var collection = await _context.Set<AuctionableObject>()
                                        .Include(x => x.Image)
                                        .Include(x => x.IdCategory)
                                        .Include(x => x.IdStateNavigation)
                                        .Include(x => x.IdUserNavigation)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .ToListAsync();
            
            // Cargar auctions en una query separada para evitar cartesian product
            var objectIds = collection.Select(x => x.Id).ToList();
            
            if (objectIds.Any())
            {
                var auctions = await _context.Set<Auction>()
                    .Where(a => objectIds.Contains(a.IdObject))
                    .Include(a => a.IdUserNavigation)
                    .Include(a => a.IdStateNavigation)
                    .Include(a => a.Bidding)
                    .AsNoTracking()
                    .ToListAsync();
                
                // Asignar auctions manualmente
                foreach (var obj in collection)
                {
                    obj.Auction = auctions.Where(a => a.IdObject == obj.Id).ToList();
                }
            }
            
            return collection;
        }

        public async Task<int> CountBidding(int id)
        {
            int count = await _context.Set<Bidding>()
                              .Where(b => b.IdAuction == id)
                              .CountAsync();
            return count;
        }
    }
}