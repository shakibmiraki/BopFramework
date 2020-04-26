using System;
using System.Collections.Generic;
using System.Linq;

namespace Bop.Core.Domain.Customers
{
    /// <summary>
    /// Represents a customer
    /// </summary>
    public class Customer : BaseEntity
    {

        private ICollection<CustomerCustomerRoleMapping> _customerCustomerRoleMappings;
        private IList<CustomerRole> _customerRoles;

        /// <summary>
        /// Ctor
        /// </summary>
        public Customer()
        {
            this.CustomerGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the customer GUID
        /// </summary>
        public Guid CustomerGuid { get; set; }

        /// <summary>
        /// Gets or sets the customername
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the phone that should be re-validated. Used in scenarios when a customer is already registered and wants to change an email address.
        /// </summary>
        public string PhoneToRevalidate { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the customer is required to re-login
        /// </summary>
        public bool RequireReLogin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating number of failed login attempts (wrong password)
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// Gets or sets the date and time until which a customer cannot login (locked out)
        /// </summary>
        public DateTime? CannotLoginUntilDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the customer system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        #region Navigation properties



        /// <summary>
        /// Gets or sets customer roles
        /// </summary>
        public virtual IList<CustomerRole> CustomerRoles => _customerRoles ?? (_customerRoles = CustomerCustomerRoleMappings.Select(mapping => mapping.CustomerRole).ToList());


        /// <summary>
        /// Gets or sets customer tokens
        /// </summary>
        public virtual ICollection<CustomerToken> CustomerTokens { get; set; }

        /// <summary>
        /// Gets or sets customer-customer role mappings
        /// </summary>
        public virtual ICollection<CustomerCustomerRoleMapping> CustomerCustomerRoleMappings
        {
            get => _customerCustomerRoleMappings ?? (_customerCustomerRoleMappings = new List<CustomerCustomerRoleMapping>());
            protected set => _customerCustomerRoleMappings = value;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Add customer role and reset customer roles cache
        /// </summary>
        /// <param name="role">Role</param>
        public void AddCustomerRoleMapping(CustomerCustomerRoleMapping role)
        {
            CustomerCustomerRoleMappings.Add(role);
            _customerRoles = null;
        }

        /// <summary>
        /// Remove customer role and reset customer roles cache
        /// </summary>
        /// <param name="role">Role</param>
        public void RemoveCustomerRoleMapping(CustomerCustomerRoleMapping role)
        {
            CustomerCustomerRoleMappings.Remove(role);
            _customerRoles = null;
        }

        #endregion
    }
}