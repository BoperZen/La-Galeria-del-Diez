using La_Galeria_del_Diez.Application.DTOs;

namespace La_Galeria_del_Diez.Application.Services.Interfaces
{
    public interface IServiceUser
    {
        Task<ICollection<UserDTO>> ListAsync();
        Task<UserDTO?> FindByIdAsync(int id);
        Task<UserDTO> AddAsync(UserDTO dto);
        Task UpdateAsync(UserDTO dto);
    }
}
