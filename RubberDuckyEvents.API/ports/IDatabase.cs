using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RubberDuckyEvents.API.Domain;

namespace RubberDuckyEvents.API.Ports
{
    public interface IDatabase
    {
        Task<ReadOnlyCollection<User>> GetAllUsers(string nameStartsWith);
        Task<User> GetUserById(int id);
        Task<User> GetUserByName(string name);
        Task<User> PersistUser(User user);
        Task DeleteUser(int parsedId);
    }
}