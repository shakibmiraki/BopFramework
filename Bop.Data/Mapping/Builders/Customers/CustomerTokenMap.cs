using Bop.Core.Domain.Customers;
using FluentMigrator.Builders.Create.Table;

namespace Bop.Data.Mapping.Builders.Customers
{
    /// <summary>
    /// Represents a customer token mapping configuration
    /// </summary>
    public class CustomerTokenMap : BopEntityBuilder<CustomerToken>
    {

        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(CustomerToken.RefreshTokenIdHash)).AsString(450).NotNullable()
                .WithColumn(nameof(CustomerToken.RefreshTokenIdHashSource)).AsString(450).Nullable();
        }

        #endregion
    }
}
