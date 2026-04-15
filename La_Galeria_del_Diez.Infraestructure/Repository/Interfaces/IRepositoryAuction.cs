using La_Galeria_del_Diez.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryAuction
    {
        Task<ICollection<Auction>> ListAsync();
        Task<ICollection<Auction>> ListActiveAsync(DateTime now);
        Task<ICollection<Auction>> ListFinishedAsync(DateTime now);
        Task<ICollection<Auction>> ListDraftAsync(DateTime now);
        Task<ICollection<Auction>> ListPendingPaymentAsync();
        Task<ICollection<int>> CloseExpiredAuctionsAsync(DateTime now);
        Task<Auction> FindByIdAsync(int id);
        Task AddAsync(Auction auction);
        Task UpdateAsync(Auction auction);
        Task DeleteAsync(Auction auction);
        Task UpdateStateAsync(int id, int stateId);
    }
}