using La_Galeria_del_Diez.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.Services.Interfaces
{
    public interface IServiceObject
    {
        Task<ICollection<Auctionable_ObjectDTO>> ListAsync();
        Task<ICollection<Auctionable_ObjectDTO>> ListAsyncNoAuction();
        Task<Auctionable_ObjectDTO?> FindByIdAsync(int id);
        Task AddAsync(Auctionable_ObjectDTO dto);
        Task UpdateAsync(Auctionable_ObjectDTO dto);
        Task DeleteAsync(int id);
        Task<bool> CanEditAsync(int id);
        Task<bool> HasActiveAuctionAsync(int id);
    }
}
