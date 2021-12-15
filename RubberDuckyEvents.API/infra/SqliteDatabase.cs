using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RubberDuckyEvents.API.Domain;
using RubberDuckyEvents.API.Ports;

namespace RubberDuckyEvents.API.Infra
{
    // SqliteDatabase inherits the interface IDatabase
    public class SqliteDatabase : IDatabase
    {
        private RubberDuckyContext _context;

        public SqliteDatabase(RubberDuckyContext context)
        {
            _context = context;
        }

        public async Task DeleteUser(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        // Task<ReadOnlyCollection<User>> loosley translates to Array<User>
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

        // User updating
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

        public async Task DeleteUserAttendance(int id)
        {
            // Get user from db
            var user = await _context.Users.FindAsync(id);
            // Change EventId          
            user.EventId = 0;
            // Tell dotnet ef EventId has changed
            _context.Entry(user).Property("EventId").IsModified = true;
            // Save the changes
            await _context.SaveChangesAsync();
        }

        public async Task AddUserAttendance(int userEventId, int userId, int eventId)
        {
            var userEvent = new UserEvent();
            userEvent = await _context.UserEvent.AddAsync(userEvent);

            userEvent.UserId = userId;
            userEvent.EventId = eventId;

            await _context.SaveChangesAsync();
        }

        // Event 
        public async Task DeleteEvent(int id)
        {
            var event_ = await _context.Events.FindAsync(id);
            _context.Events.Remove(event_);
            await _context.SaveChangesAsync();
        }

        public async Task<ReadOnlyCollection<Event>> GetAllEvents(string nameStartsWith)
        {
            var events = await _context.Events.Where(x => EF.Functions.Like(x.Name, $"{nameStartsWith}%")).ToArrayAsync();
            return Array.AsReadOnly(events);
        }

        public async Task<Event> GetEventById(int parsedId)
        {
            return await _context.Events.FindAsync(parsedId);
        }
        
        public async Task<Event[]> GetEventsByAgeRange(int age)
        {
            var events = await _context.Events.Where(x => x.MinAge <= age && age <= x.MaxAge).ToArrayAsync();
            return events;
        }

        // Event seems to be a keyword in the file OmniSharpMiscellaneousFiles.csproj || Updating events
        public async Task<Event> PersistEvent(Event event_)
        {
            if (event_.Id == 0)
            {
                await _context.Events.AddAsync(event_);
            }
            else
            {
                _context.Events.Update(event_);
            }
            await _context.SaveChangesAsync();
            return event_;
        }
    }
}