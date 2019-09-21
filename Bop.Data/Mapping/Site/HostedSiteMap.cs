using Bop.Core.Domain.Site;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bop.Data.Mapping.Site
{
    /// <summary>
    /// Represents a hosted site mapping configuration
    /// </summary>
    public class HostedSiteMap : BopEntityTypeConfiguration<HostedSite>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<HostedSite> builder)
        {
            builder.ToTable(nameof(HostedSite));
            builder.HasKey(site => site.Id);

            builder.Property(site => site.Name).HasMaxLength(400).IsRequired();
            builder.Property(site => site.Url).HasMaxLength(400);
            builder.Property(site => site.Hosts).HasMaxLength(1000);
            builder.Property(site => site.CompanyName).HasMaxLength(1000);
            builder.Property(site => site.CompanyAddress).HasMaxLength(1000);
            builder.Property(site => site.CompanyPhoneNumber).HasMaxLength(1000);

            base.Configure(builder);
        }

        #endregion
    }
}
