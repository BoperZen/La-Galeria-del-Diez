using La_Galeria_del_Diez.Infraestructure.Models;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUser
    {
        Task<ICollection<User>> ListAsync();
        Task<User> FindByIdAsync(int id);
        Task<int> Tally(int id);
        Task UpdateAsync(User user);
    }
}
