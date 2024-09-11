using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventsWebAPI.Models;
using EventsWebAPI.Enums;

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
            modelBuilder.Entity<Event>()
                .Property(p => p.Category)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.BirthDate)
                .HasColumnType("date");

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<int>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue(UserRole.User);

        }
    }
}
