using Microsoft.EntityFrameworkCore;
using UserService.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace UserService.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Username).IsUnique();

            builder.Property(x => x.Id)
                   .IsRequired();

            builder.Property(x => x.Username)
                   .IsRequired();

            //builder.Property(x => x.Picture)
            //   .IsRequired();

            builder.Property(x => x.Email)
                   .IsRequired();

            builder.Property(x => x.Password)
                   .IsRequired();

            builder.Property(x => x.Role)
                   .IsRequired();
        }
    }
}
