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
        Task AddUserAttendance(int userEventId, int userId, int eventId);
        Task DeleteUser(User user);

        Task<ReadOnlyCollection<Event>> GetAllEvents(string events);
        Task<Event> GetEventById(int id);
        Task<Event[]> GetEventsByAgeRange(int age);
        Task<Event> PersistEvent(Event event_);
        Task DeleteEvent(int id);
    }
}