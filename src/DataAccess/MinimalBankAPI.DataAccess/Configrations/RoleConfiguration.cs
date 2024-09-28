using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MinimalBankAPI.Domain.Entites.Auth;

namespace MinimalBankAPI.DataAccess.Configrations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(r => r.Alias)
                   .HasMaxLength(50);

            builder.Property(r => r.Description)
                   .HasMaxLength(250);

            //builder.Property(r => r.Status)
            //       .IsRequired();

            builder.HasMany(r => r.UserRoles)
                   .WithOne(ur => ur.Role)
                   .HasForeignKey(ur => ur.RoleId);

            builder.HasMany(r => r.RoleOperationClaims)
                   .WithOne(roc => roc.Role)
                   .HasForeignKey(roc => roc.RoleId);
        }
    }
}
