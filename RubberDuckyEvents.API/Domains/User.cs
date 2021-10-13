using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RubberDuckyEvents.API.Domain
{
    // this is a domain model. It contains the full representation of an entity within our domain.
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }
        public int name { get; set; }
        public string Date_of_Birth { get; set; }
    }
}