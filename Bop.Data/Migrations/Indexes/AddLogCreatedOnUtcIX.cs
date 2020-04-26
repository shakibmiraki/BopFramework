using FluentMigrator;
using Bop.Core.Domain.Logging;

namespace Bop.Data.Migrations.Indexes
{
    [BopMigration("2020/03/13 09:36:08:9037680")]
    public class AddLogCreatedOnUtcIX : AutoReversingMigration
    {
        #region Methods          

        public override void Up()
        {
            Create.Index("IX_Log_CreatedOnUtc").OnTable(nameof(Log))
                .OnColumn(nameof(Log.CreatedOnUtc)).Descending()
                .WithOptions().NonClustered();
        }

        #endregion
    }
}