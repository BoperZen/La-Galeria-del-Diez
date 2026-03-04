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
        Task<AuctionDTO?> FindByIdAsync(int id);
    }
}