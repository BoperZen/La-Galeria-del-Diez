using La_Galeria_del_Diez.Infraestructure.Models;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryBidding
    {
        Task AddAsync(Bidding bidding);
    }
}
