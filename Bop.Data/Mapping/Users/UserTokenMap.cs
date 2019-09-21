using Bop.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bop.Data.Mapping.Users
{
    /// <summary>
    /// Represents a user token mapping configuration
    /// </summary>
    public class UserTokenMap : BopEntityTypeConfiguration<UserToken>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable(nameof(UserToken));
            builder.HasKey(token => token.Id);

            builder.HasOne(token => token.User)
                .WithMany(user=>user.UserTokens)
                .HasForeignKey(token => token.UserId)
                .IsRequired();

            builder.Property(token => token.RefreshTokenIdHash).HasMaxLength(450).IsRequired();
            builder.Property(token => token.RefreshTokenIdHashSource).HasMaxLength(450);

            base.Configure(builder);
        }

        #endregion
    }
}
