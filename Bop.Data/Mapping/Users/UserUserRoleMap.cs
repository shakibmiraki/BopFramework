using Bop.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bop.Data.Mapping.Users
{
    /// <summary>
    /// Represents a user-user role mapping configuration
    /// </summary>
    public class UserUserRoleMap : BopEntityTypeConfiguration<UserUserRoleMapping>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UserUserRoleMapping> builder)
        {
            builder.ToTable(BopMappingDefaults.UserUserRoleTable);
            builder.HasKey(mapping => new { mapping.UserId, mapping.UserRoleId });

            builder.Property(mapping => mapping.UserId).HasColumnName("User_Id");
            builder.Property(mapping => mapping.UserRoleId).HasColumnName("UserRole_Id");

            builder.HasOne(mapping => mapping.User)
                .WithMany(customer => customer.UserUserRoleMappings)
                .HasForeignKey(mapping => mapping.UserId)
                .IsRequired();

            builder.HasOne(mapping => mapping.UserRole)
                .WithMany()
                .HasForeignKey(mapping => mapping.UserRoleId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }

        #endregion
    }
}