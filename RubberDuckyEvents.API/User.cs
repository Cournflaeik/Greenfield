using System;

namespace RubberDuckyEvents.API
{
    public class User
    {// We always use small letters when possible and we use capital letters so we can see what datatypes we've created (ourselves) (Datetime is weird exception)
       
        public int Id { get; set; } // or Guid? (more possibilities) but int is enough
        
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Mail { get; set; }

        public Event Attendance { get; set; } 
    }
}
