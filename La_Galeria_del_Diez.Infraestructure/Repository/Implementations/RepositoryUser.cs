using La_Galeria_del_Diez.Infraestructure.Data;
using La_Galeria_del_Diez.Infraestructure.Models;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Implementations
{
    public class RepositoryUser : IRepositoryUser
    {
        private readonly La_Galeria_del_Diez_Context _context;

        public RepositoryUser(La_Galeria_del_Diez_Context context)
        {
            _context = context;
        }

        public async Task<User> FindByIdAsync(int id)
        {
            var @object = await _context.Set<User>().FindAsync(id);
            return @object!;
        }

        public async Task<ICollection<User>> ListAsync()
        {
            //Select * from Autor 
            var collection = await _context.Set<User>()
                                          .Include(p => p.IdRolNavigation)
                                          .ToListAsync();
            return collection;
        }
    }
}
