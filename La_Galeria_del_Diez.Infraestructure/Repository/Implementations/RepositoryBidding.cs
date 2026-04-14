using La_Galeria_del_Diez.Infraestructure.Data;
using La_Galeria_del_Diez.Infraestructure.Models;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Implementations
{
    public class RepositoryBidding : IRepositoryBidding
    {
        private readonly La_Galeria_del_Diez_Context _context;

        public RepositoryBidding(La_Galeria_del_Diez_Context context)
        {
            _context = context;
        }

        public async Task AddAsync(Bidding bidding)
        {
            _context.Set<Bidding>().Add(bidding);
            await _context.SaveChangesAsync();
        }
    }
}
