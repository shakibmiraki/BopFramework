using Bop.Core.Domain.Common;
using System;
using System.Linq;

namespace Bop.Core.Domain.Customers
{
    /// <summary>
    /// Customer extensions
    /// </summary>
    public static class CustomerExtensions
    {
        #region Customer role

        /// <summary>
        /// Gets a value indicating whether customer is in a certain customer role
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="customerRoleSystemName">Customer role system name</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsInCustomerRole(this Customer customer,
            string customerRoleSystemName, bool onlyActiveCustomerRoles = true)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (string.IsNullOrEmpty(customerRoleSystemName))
                throw new ArgumentNullException(nameof(customerRoleSystemName));

            var result = customer.CustomerRoles
                .FirstOrDefault(cr => (!onlyActiveCustomerRoles || cr.Active) && cr.SystemName == customerRoleSystemName) != null;
            return result;
        }


        /// <summary>
        /// Gets a value indicating whether customer is administrator
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsAdmin(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, BopCustomerDefaults.AdministratorsRoleName, onlyActiveCustomerRoles);
        }


        /// <summary>
        /// Gets a value indicating whether customer is registered
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, BopCustomerDefaults.RegisteredRoleName, onlyActiveCustomerRoles);
        }


        #endregion


        #region Customer Token

        /// <summary>
        /// Gets a value indicating whether customer is in a certain customer role
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="customerRoleSystemName">Customer role system name</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsValidToken(this Customer customer, GenericAttribute token, string compareToken)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));

            if (token is null ||
                string.IsNullOrEmpty(token.Value) ||
                token.CreatedOrUpdatedDateUTC < DateTime.UtcNow.AddMinutes(-2) ||
                !token.Value.Equals(compareToken, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
