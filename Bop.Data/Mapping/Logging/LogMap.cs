using Bop.Core.Domain.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bop.Data.Mapping
{
    /// <summary>
    /// Represents a log mapping configuration
    /// </summary>
    public class LogMap : BopEntityTypeConfiguration<Log>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable(nameof(Log));
            builder.Property(a => a.Id);
            builder.HasKey(a => a.Id);

            builder.Property(a => a.ShortMessage).IsRequired();
            builder.Property(a => a.IpAddress).HasMaxLength(200);

            builder.Ignore(logItem => logItem.LogLevel);

            base.Configure(builder);
        }

        #endregion
    }
}