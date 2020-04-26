using Bop.Core.Domain.Site;
using Bop.Data.Mapping.Builders;
using FluentMigrator.Builders.Create.Table;

namespace Bop.Data.Mapping.Site
{
    /// <summary>
    /// Represents a hosted site mapping configuration
    /// </summary>
    public class HostedSiteMap : BopEntityBuilder<HostedSite>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HostedSite.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(HostedSite.Url)).AsString(400).Nullable()
                .WithColumn(nameof(HostedSite.Hosts)).AsString(1000).Nullable()
                .WithColumn(nameof(HostedSite.CompanyName)).AsString(400).Nullable()
                .WithColumn(nameof(HostedSite.CompanyAddress)).AsString(400).Nullable()
                .WithColumn(nameof(HostedSite.CompanyPhoneNumber)).AsString(400).Nullable();
        }

        #endregion
    }
}
