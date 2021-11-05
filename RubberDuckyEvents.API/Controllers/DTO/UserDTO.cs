using System;
using System.Linq;
using RubberDuckyEvents.API.Domain;

namespace RubberDuckyEvents.API.Controllers
{
    public class CreateUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mail { get; set; }
        public int EventId { get; set; }
        public User ToUser() => new User { Name = this.Name, DateOfBirth = this.DateOfBirth, Mail = this.Mail, EventId = this.EventId,  Id = this.Id };
    }

    public class ViewUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mail { get; set; }
        public int EventId { get; set; }

        public static ViewUser FromModel(User user) => new ViewUser
        {
            Id = user.Id,
            Name = user.Name,
            DateOfBirth = user.DateOfBirth,
            Mail = user.Mail,
            EventId = user.EventId,
        };
    }
}