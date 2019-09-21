using Bop.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Bop.Data.Mapping.Users
{
    /// <summary>
    /// Represents a user role mapping configuration
    /// </summary>
    public partial class UserRoleMap : BopEntityTypeConfiguration<UserRole>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(nameof(UserRole));
            builder.HasKey(role => role.Id);

            builder.Property(role => role.Name).HasMaxLength(255).IsRequired();
            builder.Property(role => role.SystemName).HasMaxLength(255);

            base.Configure(builder);
        }

        #endregion
    }
}