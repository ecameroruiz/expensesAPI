using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.Dtos;

namespace Infrastructure.Persistence.Configurations
{
    public class DbUserConfiguration : IEntityTypeConfiguration<UserDbDto>
    {
        public void Configure(EntityTypeBuilder<UserDbDto> builder)
        {
            builder.ToTable("User", "ContainerDb");
        }
    }
}