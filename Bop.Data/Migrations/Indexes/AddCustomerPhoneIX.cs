using FluentMigrator;
using Bop.Core.Domain.Customers;

namespace Bop.Data.Migrations.Indexes
{
    [BopMigration("2020/03/13 09:36:08:9037681")]
    public class AddCustomerPhoneIX : AutoReversingMigration
    {
        #region Methods   

        public override void Up()
        {
            Create.Index("IX_Customer_Phone").OnTable(nameof(Customer))
                .OnColumn(nameof(Customer.Phone)).Ascending()
                .WithOptions().NonClustered();
        }

        #endregion
    }
}