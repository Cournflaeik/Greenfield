using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RubberDuckyEvents.API.Domain;

namespace RubberDuckyEvents.API.Ports
{
    public interface IDatabase
    {
        Task<ReadOnlyCollection<User>> GetAllUsers(string users);
        Task<User> GetUserById(int id);
        Task<User> PersistUser(User user);
        Task DeleteUserAttendance(int id);
        Task<User> AddUserAttendance(User user, string eventName);
        Task DeleteUser(int parsedId);

        Task<ReadOnlyCollection<Event>> GetAllEvents(string events);
        Task<Event> GetEventById(int id);
        Task<ReadOnlyCollection<Event>> GetEventsByAgeRange(int age);
        Task<Event> PersistEvent(Event event_);
        Task DeleteEvent(int id);
    }
}