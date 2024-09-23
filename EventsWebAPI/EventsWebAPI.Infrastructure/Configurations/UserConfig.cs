using EventsWebAPI.Core.Models;
using EventsWebAPI.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsWebAPI.Infrastructure.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(u => u.BirthDate)
                .HasColumnType("date");

            builder
                .Property(u => u.Role)
                .HasConversion<int>();

            builder
                .Property(u => u.Role)
                .HasDefaultValue(UserRole.User);
        }
    }
}
