using Bop.Core.Domain.Logging;
using FluentMigrator.Builders.Create.Table;
using Bop.Data.Extensions;
using Bop.Core.Domain.Customers;

namespace Bop.Data.Mapping.Builders.Logging
{
    /// <summary>
    /// Represents a log entity builder
    /// </summary>
    public partial class LogBuilder : BopEntityBuilder<Log>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Log.ShortMessage)).AsString(int.MaxValue).NotNullable()
                .WithColumn(nameof(Log.IpAddress)).AsString(200).Nullable()
                .WithColumn(nameof(Log.CustomerId)).AsInt32().Nullable().ForeignKey<Customer>();
        }

        #endregion
    }
}