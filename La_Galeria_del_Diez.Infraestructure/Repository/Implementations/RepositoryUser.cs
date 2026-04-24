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
            var @object = await _context.Set<User>()
                                        .Include(p => p.IdRolNavigation)
                                        .FirstOrDefaultAsync(u => u.Id == id);
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

        public async Task<User> LoginAsync(string id, string password)
        {
            var @object = await _context.Set<User>()
                                        .Include(b => b.IdRolNavigation)
                                        .Where(p => p.Email == id && p.Password == password)
                                        .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task<int> Tally(int id)
        {
            var @object = await _context.Set<User>().FindAsync(id);
            int count = 0;
            if (@object.IdRol == 2)
            {
                count = await _context.Set<Auction>().Where(i => i.IdUser == id).CountAsync();
            }
            else
            {
                if (@object.IdRol == 3)
                {
                    count = await _context.Set<Bidding>().Where(i => i.IdUser == id).CountAsync();
                }
            }
            return count;
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var existing = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existing == null)
            {
                return;
            }

            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.UserState = user.UserState;

            await _context.SaveChangesAsync();
        }
    }
}