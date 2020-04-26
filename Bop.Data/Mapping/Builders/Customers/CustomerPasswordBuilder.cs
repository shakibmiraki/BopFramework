﻿using Bop.Core.Domain.Customers;
using FluentMigrator.Builders.Create.Table;
using Bop.Data.Extensions;

namespace Bop.Data.Mapping.Builders.Customers
{
    /// <summary>
    /// Represents a customer password entity builder
    /// </summary>
    public partial class CustomerPasswordBuilder : BopEntityBuilder<CustomerPassword>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(CustomerPassword.CustomerId)).AsInt32().ForeignKey<Customer>();
        }

        #endregion
    }
}