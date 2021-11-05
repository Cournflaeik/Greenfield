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
        Task<User> PersistUser(User user);
        Task<User> DeleteUserAttendance(User user);
        Task<User> AddUserAttendance(User user, string eventName);
        Task DeleteUser(int parsedId);

        Task<ReadOnlyCollection<Event>> GetAllEvents(string nameStartsWith);
        Task<Event> GetEventById(int id);
        Task<Event> GetEventsByAgeRange(DateTime minAge, DateTime maxAge);
        Task<Event> PersistEvent(Event event_);
        Task DeleteEvent(int id);
    }
}