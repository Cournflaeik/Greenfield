using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RubberDuckyEvents.API.Domain;
using RubberDuckyEvents.API.Ports;

namespace RubberDuckyEvents.API.Infra
{
    public class SqliteDatabase : IDatabase
    {
        private userContext _context;

        public SqliteDatabase(userContext context)
        {
            _context = context;
        }
        public async Task DeleteUser(int parsedId)
        {
            var user = await _context.Users.FindAsync(parsedId);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ReadOnlyCollection<User>> GetAllUsers(string nameStartsWith)
        {

            var users = await _context.Users.Where(x => EF.Functions.Like(x.Name, $"{nameStartsWith}%")).ToArrayAsync();
            return Array.AsReadOnly(users);
        }

        public async Task<User> GetUserById(int parsedId)
        {
            return await _context.Users.FindAsync(parsedId);
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _context.Users.FindAsync(name);
        }
        public async Task<User> PersistUser(User user)
        {
            if (user.Id == 0)
            {
                await _context.Users.AddAsync(user);
            }
            else
            {
                _context.Users.Update(user);
            }
            await _context.SaveChangesAsync();
            return user;
        }
    }
}