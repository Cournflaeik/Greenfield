using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RubberDuckyEvents.API.Domain
{
    public class UserEvent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // No composite key because it can only be used in a fluent API
        // We focus only on the essence of this course
        public int UserEventId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; } //TODO: Only one event per person
    }
}