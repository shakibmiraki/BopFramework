using Bop.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bop.Data
{
    class BopDesignTimeObjectContext : IDesignTimeDbContextFactory<BopObjectContext>
    {
        public BopObjectContext CreateDbContext(string[] args)
        {
            var dataSettings = DataSettingsManager.LoadSettings();
            var optionsBuilder = new DbContextOptionsBuilder<BopObjectContext>();
            optionsBuilder.UseSqlServer(dataSettings.DataConnectionString);

            return new BopObjectContext(optionsBuilder.Options);
        }
    }
}
