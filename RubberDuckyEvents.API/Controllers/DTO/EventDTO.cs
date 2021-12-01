using System;
using System.Linq;
using RubberDuckyEvents.API.Domain;

namespace RubberDuckyEvents.API.Controllers
{
    public class CreateEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        // Creates an Event object from input
        public Event ToEvent() => new Event { Id = this.Id, Name = this.Name, Description = this.Description, MinAge = this.MinAge, MaxAge = this.MaxAge, StartDate = this.StartDate, EndDate = this.EndDate, StreetName = this.StreetName, StreetNumber = this.StreetNumber, City = this.City, Country = this.Country};
    }

    public class ViewEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public static ViewEvent FromModel(Event event_) => new ViewEvent
        {
            Id = event_.Id,
            Name = event_.Name,
            Description = event_.Description,
            MinAge = event_.MinAge,
            MaxAge = event_.MaxAge,
            StartDate = event_.StartDate,
            EndDate = event_.EndDate,
            StreetName = event_.StreetName,
            StreetNumber = event_.StreetNumber,
            City = event_.City,
            Country = event_.Country,
        };
    }
}