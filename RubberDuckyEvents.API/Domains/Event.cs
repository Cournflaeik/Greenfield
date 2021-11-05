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
        public int MinAge {get; set; }
        public int MaxAge { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // REMARK this way of structuring your code doesn't really promote re-use. I'd use an Address class and a property called "EventAddress"
        //Nice to have
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}