using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RubberDuckyEvents.API.Domain;

namespace RubberDuckyEvents.API.Infra
{
    public class VideoStoreContext : DbContext
    {
        public VideoStoreContext(DbContextOptions<VideoStoreContext> ctx) : base(ctx)
        {

        }

        public DbSet<Movie> Movies { get; set; }
    }
}