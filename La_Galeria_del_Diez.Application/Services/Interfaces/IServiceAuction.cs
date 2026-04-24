using La_Galeria_del_Diez.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.Services.Interfaces
{
    public interface IServiceAuction
    {
        Task<ICollection<AuctionDTO>> ListAsync();
        Task<ICollection<AuctionDTO>> ListActiveAsync(DateTime now);
        Task<ICollection<AuctionDTO>> ListFinishedAsync(DateTime now);
        Task<ICollection<AuctionDTO>> ListDraftAsync(DateTime now);
        Task<ICollection<AuctionDTO>> ListPendingPaymentByWinnerAsync(int winnerUserId);
        Task<ICollection<int>> CloseExpiredAuctionsAsync(DateTime now);
        Task<AuctionDTO?> FindByIdAsync(int id);
        Task AddAsync(AuctionDTO dto);
        Task UpdateAsync(int id, AuctionDTO dto);
        Task DeleteAsync(int id);
        Task UpdateStateAsync(int id, int stateId);
    }
}