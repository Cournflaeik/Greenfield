using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RubberDuckyEvents.API.Domain
{
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // or Guid? (more possibilities) but int is enough
        public string Name { get; set; }
        public string Description { get; set; }
        // REMARK are you sure you want to make this a datetime and not a simple int? 
        // .NET has methods to work with timespans https://docs.microsoft.com/en-us/dotnet/api/system.timespan?view=net-5.0.
        public DateTime MinAge {get; set; }
        public DateTime MaxAge { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // REMARK this way of structuring your code doesn't really promote re-use. I'd use an Address class and a property called "EventAddress"
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}