using Microsoft.EntityFrameworkCore;
using EventsWebAPI.Context;
using System;

namespace EventsWebAPI.UnitTests.Factory
{
    public class EventsDbContextFactory
    {
        public EventDbContext CreateDbContext()
        {
            return new EventDbContext(
                new DbContextOptionsBuilder<EventDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
        }

    }
}
