using La_Galeria_del_Diez.Application.DTOs.Api;

using La_Galeria_del_Diez.Application.DTOs.Api;

namespace La_Galeria_del_Diez.Application.Services.Interfaces.Api
{
    public interface IAuctionApiService
    {
        Task<ICollection<AuctionApiDto>> ListAsync();
        Task<AuctionApiDto?> FindByIdAsync(int id);
        Task AddAsync(AuctionApiDto dto);
        Task UpdateAsync(int id, AuctionApiDto dto);
        Task DeleteAsync(int id);
    }
}
