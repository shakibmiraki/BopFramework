using System;
using System.Linq;
using Bop.Core;
using Bop.Core.Domain.Customers;
using Bop.Services.Events;
using Bop.Services.Localization;
using Bop.Services.Security;

namespace Bop.Services.Customers
{
    /// <summary>
    /// Customer registration service
    /// </summary>
    public partial class CustomerRegistrationService : ICustomerRegistrationService
    {
        #region Fields

        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;


        #endregion

        #region Ctor

        public CustomerRegistrationService(CustomerSettings customerSettings,
            ICustomerService customerService,
            IEncryptionService encryptionService,
            IEventPublisher eventPublisher,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            _customerSettings = customerSettings;
            _customerService = customerService;
            _encryptionService = encryptionService;
            _eventPublisher = eventPublisher;
            _localizationService = localizationService;
            _workContext = workContext;

        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether the entered password matches with a saved one
        /// </summary>
        /// <param name="customerPassword">Customer password</param>
        /// <param name="enteredPassword">The entered password</param>
        /// <returns>True if passwords match; otherwise false</returns>
        protected bool PasswordsMatch(CustomerPassword customerPassword, string enteredPassword)
        {
            if (customerPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (customerPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, customerPassword.PasswordSalt, _customerSettings.HashedPasswordFormat);
                    break;
            }

            if (customerPassword.Password == null)
                return false;

            return customerPassword.Password.Equals(savedPassword);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validate customer
        /// </summary>
        /// <param name="customernameOrEmail">Customername or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual CustomerLoginResults ValidateCustomer(string customernameOrEmail, string password)
        {
            var customer = _customerSettings.UsernameEnabled ?
                _customerService.GetCustomerByCustomername(customernameOrEmail) :
                _customerService.GetCustomerByPhone(customernameOrEmail);

            if (customer == null)
                return CustomerLoginResults.CustomerNotExist;
            if (customer.Deleted)
                return CustomerLoginResults.Deleted;
            if (!customer.Active)
                return CustomerLoginResults.NotActive;
            //only registered can login
            if (!customer.IsRegistered())
                return CustomerLoginResults.NotRegistered;
            //check whether a customer is locked out
            if (customer.CannotLoginUntilDateUtc.HasValue && customer.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return CustomerLoginResults.LockedOut;

            if (!PasswordsMatch(_customerService.GetCurrentPassword(customer.Id), password))
            {
                //wrong password
                customer.FailedLoginAttempts++;
                if (_customerSettings.FailedPasswordAllowedAttempts > 0 &&
                    customer.FailedLoginAttempts >= _customerSettings.FailedPasswordAllowedAttempts)
                {
                    //lock out
                    customer.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_customerSettings.FailedPasswordLockoutMinutes);
                    //reset the counter
                    customer.FailedLoginAttempts = 0;
                }

                _customerService.UpdateCustomer(customer);

                return CustomerLoginResults.WrongPassword;
            }

            //update login details
            customer.FailedLoginAttempts = 0;
            customer.CannotLoginUntilDateUtc = null;
            customer.RequireReLogin = false;
            customer.LastLoginDateUtc = DateTime.UtcNow;
            _customerService.UpdateCustomer(customer);

            return CustomerLoginResults.Successful;
        }

        /// <summary>
        /// Register customer
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Customer == null)
                throw new ArgumentException("Can't load current customer");

            var result = new CustomerRegistrationResult();

            if (request.Customer.IsRegistered())
            {
                result.AddError("Current customer is already registered");
                return result;
            }

            if (string.IsNullOrEmpty(request.Phone))
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.PhoneIsNotProvided"));
                return result;
            }

            if (!CommonHelper.IsValidPhone(request.Phone))
            {
                result.AddError(_localizationService.GetResource("Common.WrongPhoneFormat"));
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.PasswordIsNotProvided"));
                return result;
            }

            //validate unique customer
            if (_customerService.GetCustomerByPhone(request.Phone) != null)
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.PhoneAlreadyExists"));
                return result;
            }


            //at this point request is valid
            request.Customer.Phone = request.Phone;

            var customerPassword = new CustomerPassword
            {
                Customer = request.Customer,
                PasswordFormat = request.PasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    customerPassword.Password = request.Password;
                    break;
                case PasswordFormat.Encrypted:
                    customerPassword.Password = _encryptionService.EncryptText(request.Password);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(BopCustomerServiceDefaults.PasswordSaltKeySize);
                    customerPassword.PasswordSalt = saltKey;
                    customerPassword.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _customerSettings.HashedPasswordFormat);
                    break;
            }

            _customerService.InsertCustomerPassword(customerPassword);

            request.Customer.Active = request.IsApproved;

            //add to 'Registered' role
            var registeredRole = _customerService.GetCustomerRoleBySystemName(BopCustomerDefaults.RegisteredRoleName);
            if (registeredRole == null)
                throw new BopException("'Registered' role could not be loaded");
            //request.Customer.CustomerRoles.Add(registeredRole);
            request.Customer.AddCustomerRoleMapping(new CustomerCustomerRoleMapping { CustomerRole = registeredRole });


            _customerService.UpdateCustomer(request.Customer);

            return result;
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new ChangePasswordResult();
            if (string.IsNullOrWhiteSpace(request.Phone))
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.EmailIsNotProvided"));
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.PasswordIsNotProvided"));
                return result;
            }

            var customer = _customerService.GetCustomerByPhone(request.Phone);
            if (customer == null)
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.EmailNotFound"));
                return result;
            }

            //request isn't valid
            if (request.ValidateRequest && !PasswordsMatch(_customerService.GetCurrentPassword(customer.Id), request.OldPassword))
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.OldPasswordDoesntMatch"));
                return result;
            }

            //check for duplicates
            if (_customerSettings.UnduplicatedPasswordsNumber > 0)
            {
                //get some of previous passwords
                var previousPasswords = _customerService.GetCustomerPasswords(customer.Id, passwordsToReturn: _customerSettings.UnduplicatedPasswordsNumber);

                var newPasswordMatchesWithPrevious = previousPasswords.Any(password => PasswordsMatch(password, request.NewPassword));
                if (newPasswordMatchesWithPrevious)
                {
                    result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.PasswordMatchesWithPrevious"));
                    return result;
                }
            }

            //at this point request is valid
            var customerPassword = new CustomerPassword
            {
                CustomerId = customer.Id,
                PasswordFormat = request.NewPasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.NewPasswordFormat)
            {
                case PasswordFormat.Clear:
                    customerPassword.Password = request.NewPassword;
                    break;
                case PasswordFormat.Encrypted:
                    customerPassword.Password = _encryptionService.EncryptText(request.NewPassword);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(BopCustomerServiceDefaults.PasswordSaltKeySize);
                    customerPassword.PasswordSalt = saltKey;
                    customerPassword.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey,
                        request.HashedPasswordFormat ?? _customerSettings.HashedPasswordFormat);
                    break;
            }

            _customerService.InsertCustomerPassword(customerPassword);

            //publish event
            _eventPublisher.Publish(new CustomerPasswordChangedEvent(customerPassword));

            return result;
        }

        /// <summary>
        /// Sets a customer email
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newPhone">New email</param>
        /// <param name="requireValidation">Require validation of new email address</param>
        public virtual void SetPhone(Customer customer, string newPhone, bool requireValidation)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (newPhone == null)
                throw new BopException("Email cannot be null");

            newPhone = newPhone.Trim();
            var oldEmail = customer.Phone;

            if (!CommonHelper.IsValidPhone(newPhone))
                throw new BopException(_localizationService.GetResource("Account.EmailCustomernameErrors.NewEmailIsNotValid"));

            if (newPhone.Length > 100)
                throw new BopException(_localizationService.GetResource("Account.EmailCustomernameErrors.EmailTooLong"));

            var customer2 = _customerService.GetCustomerByPhone(newPhone);
            if (customer2 != null && customer.Id != customer2.Id)
                throw new BopException(_localizationService.GetResource("Account.EmailCustomernameErrors.EmailAlreadyExists"));

            customer.Phone = newPhone;
            _customerService.UpdateCustomer(customer);

        }

        /// <summary>
        /// Sets a customer customername
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newCustomername">New Customername</param>
        public virtual void SetUsername(Customer customer, string newCustomername)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (!_customerSettings.UsernameEnabled)
                throw new BopException("Customernames are disabled");

            newCustomername = newCustomername.Trim();

            if (newCustomername.Length > BopCustomerServiceDefaults.CustomerCustomernameLength)
                throw new BopException(_localizationService.GetResource("Account.EmailCustomernameErrors.CustomernameTooLong"));

            var customer2 = _customerService.GetCustomerByCustomername(newCustomername);
            if (customer2 != null && customer.Id != customer2.Id)
                throw new BopException(_localizationService.GetResource("Account.EmailCustomernameErrors.CustomernameAlreadyExists"));

            customer.Username = newCustomername;
            _customerService.UpdateCustomer(customer);
        }

        #endregion
    }
}