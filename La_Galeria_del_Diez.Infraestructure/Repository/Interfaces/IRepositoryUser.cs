using La_Galeria_del_Diez.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUser
    {
        Task<ICollection<User>> ListAsync();
        Task<User> FindByIdAsync(int id);
    }
}
