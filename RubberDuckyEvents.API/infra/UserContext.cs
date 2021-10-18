using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RubberDuckyEvents.API.Domain;

namespace RubberDuckyEvents.API.Infra
{
    public class userContext : DbContext
    {
        public userContext(DbContextOptions<userContext> ctx) : base(ctx)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}