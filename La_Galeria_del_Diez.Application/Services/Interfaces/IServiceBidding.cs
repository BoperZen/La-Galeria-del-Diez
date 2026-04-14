using La_Galeria_del_Diez.Application.DTOs;

namespace La_Galeria_del_Diez.Application.Services.Interfaces
{
    public interface IServiceBidding
    {
        Task AddAsync(BiddingDTO dto);
    }
}
