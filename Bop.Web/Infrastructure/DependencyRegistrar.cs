using Autofac;
using Bop.Core.Configuration;
using Bop.Core.Infrastructure;
using Bop.Core.Infrastructure.DependencyManagement;
using Bop.Web.Areas.Admin.Factories;



namespace Bop.Web.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, BopConfig config)
        {
            //factories
            builder.RegisterType<CustomerModelFactory>().As<ICustomerModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.CustomerModelFactory>().As<Factories.ICustomerModelFactory>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 2;
    }
}
