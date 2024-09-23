using Microsoft.EntityFrameworkCore;
using EventsWebAPI.Core.Models;
using EventsWebAPI.Infrastructure.Configurations;

namespace EventsWebAPI.Context
{
    public class EventDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AttendingInfo> Attendings { get; set; }
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EventConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());

        }
    }
}
