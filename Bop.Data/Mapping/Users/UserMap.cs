using Bop.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Bop.Data.Mapping.Users
{
    /// <summary>
    /// Represents a user mapping configuration
    /// </summary>
    public partial class UserMap : BopEntityTypeConfiguration<User>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(customer => customer.Id);

            builder.Property(user => user.Username).HasMaxLength(1000);
            builder.Property(user => user.Phone).HasMaxLength(1000);
            builder.Property(user => user.PhoneToRevalidate).HasMaxLength(1000);
            builder.Property(user => user.SystemName).HasMaxLength(400);
            builder.Ignore(user => user.UserRoles);

            base.Configure(builder);
        }

        #endregion
    }
}