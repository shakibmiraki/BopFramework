﻿using System;
using System.Collections.Generic;
using Bop.Core;
using Bop.Core.Domain.Customers;


namespace Bop.Services.Customers
{
    /// <summary>
    /// Customer service interface
    /// </summary>
    public partial interface ICustomerService
    {
        #region Customers

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="customerRoleIds">A list of customer role identifiers to filter by (at least one match); pass null or empty list in order to load all customers; </param>
        /// <param name="email">Email; null to load all customers</param>
        /// <param name="customername">Customername; null to load all customers</param>
        /// <param name="firstName">First name; null to load all customers</param>
        /// <param name="lastName">Last name; null to load all customers</param>
        /// <param name="ipAddress">IP address; null to load all customers</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        /// <returns>Customers</returns>
        IPagedList<Customer> GetAllCustomers(DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
            int[] customerRoleIds = null,
            string email = null, string customername = null, string firstName = null, string lastName = null, string ipAddress = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        /// <summary>
        /// Gets online customers
        /// </summary>
        /// <param name="lastActivityFromUtc">Customer last activity date (from)</param>
        /// <param name="customerRoleIds">A list of customer role identifiers to filter by (at least one match); pass null or empty list in order to load all customers; </param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Customers</returns>
        IPagedList<Customer> GetOnlineCustomers(DateTime lastActivityFromUtc,
            int[] customerRoleIds, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="customer">Customer</param>
        void DeleteCustomer(Customer customer);


        /// <summary>
        /// Delete guest customer records
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <returns>Number of deleted customer</returns>
        int DeleteGuestCustomers(DateTime? createdFromUtc, DateTime? createdToUtc);


        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        Customer GetCustomerById(int customerId);

        /// <summary>
        /// Get customers by identifiers
        /// </summary>
        /// <param name="customerIds">Customer identifiers</param>
        /// <returns>Customers</returns>
        IList<Customer> GetCustomersByIds(int[] customerIds);

        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="customerGuid">Customer GUID</param>
        /// <returns>A customer</returns>
        Customer GetCustomerByGuid(Guid customerGuid);

        /// <summary>
        /// Get customer by email
        /// </summary>
        /// <param name="phone">Email</param>
        /// <returns>Customer</returns>
        Customer GetCustomerByPhone(string phone);

        /// <summary>
        /// Get customer by customername
        /// </summary>
        /// <param name="customername">Customername</param>
        /// <returns>Customer</returns>
        Customer GetCustomerByCustomername(string customername);

        /// <summary>
        /// Insert a guest customer
        /// </summary>
        /// <returns>Customer</returns>
        Customer InsertGuestCustomer();

        /// <summary>
        /// Insert a customer
        /// </summary>
        /// <param name="customer">Customer</param>
        void InsertCustomer(Customer customer);

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="customer">Customer</param>
        void UpdateCustomer(Customer customer);


        #endregion

        #region Customer roles

        /// <summary>
        /// Delete a customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        void DeleteCustomerRole(CustomerRole customerRole);

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <returns>Customer role</returns>
        CustomerRole GetCustomerRoleById(int customerRoleId);

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="systemName">Customer role system name</param>
        /// <returns>Customer role</returns>
        CustomerRole GetCustomerRoleBySystemName(string systemName);

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false);

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        void InsertCustomerRole(CustomerRole customerRole);

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        void UpdateCustomerRole(CustomerRole customerRole);

        #endregion

        #region Customer passwords

        /// <summary>
        /// Gets customer passwords
        /// </summary>
        /// <param name="customerId">Customer identifier; pass null to load all records</param>
        /// <param name="passwordFormat">Password format; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of customer passwords</returns>
        IList<CustomerPassword> GetCustomerPasswords(int? customerId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null);

        /// <summary>
        /// Get current customer password
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Customer password</returns>
        CustomerPassword GetCurrentPassword(int customerId);

        /// <summary>
        /// Insert a customer password
        /// </summary>
        /// <param name="customerPassword">Customer password</param>
        void InsertCustomerPassword(CustomerPassword customerPassword);

        /// <summary>
        /// Update a customer password
        /// </summary>
        /// <param name="customerPassword">Customer password</param>
        void UpdateCustomerPassword(CustomerPassword customerPassword);


        /// <summary>
        /// Check whether customer password is expired 
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>True if password is expired; otherwise false</returns>
        bool PasswordIsExpired(Customer customer);

        #endregion
    }
}