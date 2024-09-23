using Microsoft.EntityFrameworkCore;
using EventsWebAPI.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsWebAPI.Infrastructure.Configurations
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .Property(p => p.Category)
                .HasConversion<string>();
        }
    }
}
