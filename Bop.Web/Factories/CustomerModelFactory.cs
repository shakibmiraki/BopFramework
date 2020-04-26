using System;
using Bop.Core.Domain.Customers;
using Bop.Core.Domain.Security;
using Bop.Web.Models.Customers;


namespace Bop.Web.Factories
{
    public class CustomerModelFactory : ICustomerModelFactory
    {

        public LoginRequest PrepareLoginModel()
        {
            var model = new LoginRequest();
            return model;
        }

                /// <summary>
        /// Prepare the customer register model
        /// </summary>
        /// <param name="model">Customer register model</param>
        /// <returns>Customer register model</returns>
        public virtual RegisterRequest PrepareRegisterModel(RegisterRequest model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            return model;
        }
    }
}
