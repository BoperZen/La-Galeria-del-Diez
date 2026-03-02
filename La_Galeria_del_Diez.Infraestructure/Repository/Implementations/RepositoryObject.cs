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
                                        .Include(o => o.Auction)
                                            .ThenInclude(a => a.IdUserNavigation)
                                        .Include(o => o.Auction)
                                            .ThenInclude(a => a.Winner)
                                        .Include(o => o.Auction)
                                            .ThenInclude(a => a.IdStateNavigation)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(o => o.Id == id);
            return @object!;
        }

        public async Task<ICollection<AuctionableObject>> ListAsync()
        {
            var collection = await _context.Set<AuctionableObject>()
                                        .Include(x => x.Image)
                                        .AsNoTracking()
                                        .ToListAsync();
            return collection;
        }
    }
}