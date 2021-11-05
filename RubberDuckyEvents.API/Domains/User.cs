using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RubberDuckyEvents.API.Domain
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mail { get; set; }

        // REMARK this implies that an user can only have one event that (s)he plans to attend. Is this 
        // what you want?
        public Event Attendance { get; set; } 
    }
}