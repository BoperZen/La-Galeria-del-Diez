using La_Galeria_del_Diez.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryObject
    {
        Task<ICollection<AuctionableObject>> ListAsync();
        Task<ICollection<AuctionableObject>> ListAsyncNoAuction();
        Task<AuctionableObject> FindByIdAsync(int id);
        Task AddAsync(AuctionableObject auctionableObject);
        Task UpdateAsync(AuctionableObject auctionableObject);
        Task DeleteAsync(int id);
        Task<bool> HasActiveAuctionAsync(int id);
        Task<bool> CanEditAsync(int id);
    }
}