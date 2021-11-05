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
        private RubberDuckyContext _context;

        public SqliteDatabase(RubberDuckyContext context)
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

        public async Task DeleteUserAttendance(int id)
        {
            var user = await _context.Users.FindAsync(id);              //Get user from db
            user.EventId = 0;                                           //Change EventId
            _context.Entry(user).Property("EventId").IsModified = true; //Tell dotnet ef EventId has changed
            await _context.SaveChangesAsync();                          //Save the changes
        }

        public async Task<User> AddUserAttendance(User user, string eventName)
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

        //Event 
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

        public async Task<ReadOnlyCollection<Event>> GetEventsByAgeRange(int age)
        {
            var events = await _context.Events.Where(x => x.MinAge <= age && age <= x.MaxAge).ToArrayAsync();
            return Array.AsReadOnly(events);
        }
        public async Task<Event> PersistEvent(Event event_) //event seems to be a keyword in the file OmniSharpMiscellaneousFiles.csproj
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