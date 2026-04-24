using La_Galeria_del_Diez.Infraestructure.Models;

using La_Galeria_del_Diez.Infraestructure.Models;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUser
    {
        Task<ICollection<User>> ListAsync();
        Task<User> FindByIdAsync(int id);
        Task<int> Tally(int id);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task<User> LoginAsync(string id, string password);
    }
}
