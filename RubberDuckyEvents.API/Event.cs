using System;

namespace RubberDuckyEvents.API
{
    public class Event
    {
        public int Id { get; set; } // or Guid? (more possibilities) but int is enough
        
        public string Name { get; set; }

        public DateTime MinAge {get; set; }

        public DateTime MaxAge { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string StreetName { get; set; }

        public int StreetNumber { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

    }
}
