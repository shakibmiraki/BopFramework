using Bop.Core.Domain.Common;
using Bop.Core.Domain.Configuration;
using Bop.Core.Domain.Customers;
using Bop.Core.Domain.Localization;
using Bop.Core.Domain.Logging;
using Bop.Core.Domain.Security;
using Bop.Core.Domain.Site;
using Bop.Core.Domain.Tasks;
using FluentMigrator;


namespace Bop.Data.Migrations
{
    [BopMigration("2020/01/31 11:24:16:2551771", "Bop.Data base schema")]
    public class SchemaMigration : AutoReversingMigration
    {
        private readonly IMigrationManager _migrationManager;

        public SchemaMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        /// <summary>
        /// Collect the UP migration expressions
        /// <remarks>
        /// We use an explicit table creation order instead of an automatic one
        /// due to problems creating relationships between tables
        /// </remarks>
        /// </summary>
        public override void Up()
        {

            //common
            _migrationManager.BuildTable<GenericAttribute>(Create);

            //customer
            _migrationManager.BuildTable<Customer>(Create);
            _migrationManager.BuildTable<CustomerPassword>(Create);
            _migrationManager.BuildTable<CustomerRole>(Create);
            _migrationManager.BuildTable<CustomerToken>(Create);
            _migrationManager.BuildTable<CustomerCustomerRoleMapping>(Create);

            //site
            _migrationManager.BuildTable<HostedSite>(Create);

            //localization
            _migrationManager.BuildTable<Language>(Create);
            _migrationManager.BuildTable<LocaleStringResource>(Create);
            _migrationManager.BuildTable<LocalizedProperty>(Create);

            //setting
            _migrationManager.BuildTable<Setting>(Create);

            //activity
            _migrationManager.BuildTable<ActivityLogType>(Create);
            _migrationManager.BuildTable<ActivityLog>(Create);
            _migrationManager.BuildTable<Log>(Create);

            //permission
            _migrationManager.BuildTable<PermissionRecord>(Create);
            _migrationManager.BuildTable<PermissionRecordCustomerRoleMapping>(Create);
            
            //task
            _migrationManager.BuildTable<ScheduleTask>(Create);

        }
    }
}
