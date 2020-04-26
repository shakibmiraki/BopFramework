using Bop.Core;

namespace Bop.Web.Areas.Admin.Factories
{
    public class CustomerModelFactory : ICustomerModelFactory
    {
        private readonly IWorkContext _workContext;

        public CustomerModelFactory(IWorkContext workContext)
        {
            _workContext = workContext;
        }
    }
}
