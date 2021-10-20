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

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}