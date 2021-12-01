using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RubberDuckyEvents.API.Domain;

namespace RubberDuckyEvents.API.Infra
{
    public class RubberDuckyContext : DbContext
    {
        public RubberDuckyContext(DbContextOptions<RubberDuckyContext> ctx) : base(ctx)
        {

        }

        // Add users to the context
        public DbSet<User> Users { get; set; }
        // Add events to the context
        public DbSet<Event> Events { get; set; }
    }
}