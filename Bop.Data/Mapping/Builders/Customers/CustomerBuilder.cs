using Bop.Core.Domain.Customers;
using FluentMigrator.Builders.Create.Table;


namespace Bop.Data.Mapping.Builders.Customers
{
    /// <summary>
    /// Represents a customer entity builder
    /// </summary>
    public partial class CustomerBuilder : BopEntityBuilder<Customer>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Customer.Username)).AsString(1000).Nullable()
                .WithColumn(nameof(Customer.Phone)).AsString(1000).Nullable()
                .WithColumn(nameof(Customer.PhoneToRevalidate)).AsString(1000).Nullable()
                .WithColumn(nameof(Customer.SystemName)).AsString(400).Nullable();
        }

        #endregion
    }
}