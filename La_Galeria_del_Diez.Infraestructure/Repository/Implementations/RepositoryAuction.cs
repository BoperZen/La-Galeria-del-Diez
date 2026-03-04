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
    }
}