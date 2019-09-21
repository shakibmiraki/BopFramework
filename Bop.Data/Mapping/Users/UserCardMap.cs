using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bop.Core.Domain.Users;

namespace Bop.Data.Mapping.Users
{
    /// <summary>
    /// Represents a card mapping configuration
    /// </summary>
    public class UserCardMap : BopEntityTypeConfiguration<UserCard>
    {
        public EntityTypeBuilder<UserCard> EntityTypeBuilder { get; set; }

        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UserCard> builder)
        {
            builder.ToTable(nameof(UserCard));
            builder.HasKey(card => card.Id);


            builder.HasOne(usercard => usercard.User)
                .WithMany(user => user.UserCards)
                .HasForeignKey(token => token.UserId)
                .IsRequired()
                .Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
                

            EntityTypeBuilder = builder;

            
            builder.Property(token => token.Cardno).HasMaxLength(19).IsRequired();
            builder.Property(token => token.CreatedOn).IsRequired();

            base.Configure(builder);

        }

        #endregion
    }
}
