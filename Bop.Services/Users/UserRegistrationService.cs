using System;
using System.Linq;
using Bop.Core;
using Bop.Core.Domain.Users;
using Bop.Services.Events;
using Bop.Services.Localization;
using Bop.Services.Security;


namespace Bop.Services.Users
{
    /// <summary>
    /// User registration service
    /// </summary>
    public partial class UserRegistrationService : IUserRegistrationService
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;


        #endregion

        #region Ctor

        public UserRegistrationService(UserSettings userSettings,
            IUserService userService,
            IEncryptionService encryptionService,
            IEventPublisher eventPublisher,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            _userSettings = userSettings;
            _userService = userService;
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
        /// <param name="userPassword">User password</param>
        /// <param name="enteredPassword">The entered password</param>
        /// <returns>True if passwords match; otherwise false</returns>
        protected bool PasswordsMatch(UserPassword userPassword, string enteredPassword)
        {
            if (userPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (userPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, userPassword.PasswordSalt, _userSettings.HashedPasswordFormat);
                    break;
            }

            if (userPassword.Password == null)
                return false;

            return userPassword.Password.Equals(savedPassword);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual UserLoginResults ValidateUser(string usernameOrEmail, string password)
        {
            var user = _userSettings.UsernamesEnabled ?
                _userService.GetUserByUsername(usernameOrEmail) :
                _userService.GetUserByPhone(usernameOrEmail);

            if (user == null)
                return UserLoginResults.UserNotExist;
            if (user.Deleted)
                return UserLoginResults.Deleted;
            if (!user.Active)
                return UserLoginResults.NotActive;
            //only registered can login
            if (!user.IsRegistered())
                return UserLoginResults.NotRegistered;
            //check whether a user is locked out
            if (user.CannotLoginUntilDateUtc.HasValue && user.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return UserLoginResults.LockedOut;

            if (!PasswordsMatch(_userService.GetCurrentPassword(user.Id), password))
            {
                //wrong password
                user.FailedLoginAttempts++;
                if (_userSettings.FailedPasswordAllowedAttempts > 0 &&
                    user.FailedLoginAttempts >= _userSettings.FailedPasswordAllowedAttempts)
                {
                    //lock out
                    user.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_userSettings.FailedPasswordLockoutMinutes);
                    //reset the counter
                    user.FailedLoginAttempts = 0;
                }

                _userService.UpdateUser(user);

                return UserLoginResults.WrongPassword;
            }

            //update login details
            user.FailedLoginAttempts = 0;
            user.CannotLoginUntilDateUtc = null;
            user.RequireReLogin = false;
            user.LastLoginDateUtc = DateTime.UtcNow;
            _userService.UpdateUser(user);

            return UserLoginResults.Successful;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual UserRegistrationResult RegisterUser(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.User == null)
                throw new ArgumentException("Can't load current user");

            var result = new UserRegistrationResult();

            if (request.User.IsRegistered())
            {
                result.AddError("Current user is already registered");
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

            //validate unique user
            if (_userService.GetUserByPhone(request.Phone) != null)
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.PhoneAlreadyExists"));
                return result;
            }


            //at this point request is valid
            request.User.Phone = request.Phone;

            var userPassword = new UserPassword
            {
                User = request.User,
                PasswordFormat = request.PasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.Password;
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.Password);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(BopUserServiceDefaults.PasswordSaltKeySize);
                    userPassword.PasswordSalt = saltKey;
                    userPassword.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _userSettings.HashedPasswordFormat);
                    break;
            }

            _userService.InsertUserPassword(userPassword);

            request.User.Active = request.IsApproved;

            //add to 'Registered' role
            var registeredRole = _userService.GetUserRoleBySystemName(BopUserDefaults.RegisteredRoleName);
            if (registeredRole == null)
                throw new BopException("'Registered' role could not be loaded");
            //request.User.UserRoles.Add(registeredRole);
            request.User.AddUserRoleMapping(new UserUserRoleMapping { UserRole = registeredRole });


            _userService.UpdateUser(request.User);

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

            var user = _userService.GetUserByPhone(request.Phone);
            if (user == null)
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.EmailNotFound"));
                return result;
            }

            //request isn't valid
            if (request.ValidateRequest && !PasswordsMatch(_userService.GetCurrentPassword(user.Id), request.OldPassword))
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.OldPasswordDoesntMatch"));
                return result;
            }

            //check for duplicates
            if (_userSettings.UnduplicatedPasswordsNumber > 0)
            {
                //get some of previous passwords
                var previousPasswords = _userService.GetUserPasswords(user.Id, passwordsToReturn: _userSettings.UnduplicatedPasswordsNumber);

                var newPasswordMatchesWithPrevious = previousPasswords.Any(password => PasswordsMatch(password, request.NewPassword));
                if (newPasswordMatchesWithPrevious)
                {
                    result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.PasswordMatchesWithPrevious"));
                    return result;
                }
            }

            //at this point request is valid
            var userPassword = new UserPassword
            {
                User = user,
                PasswordFormat = request.NewPasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.NewPasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.NewPassword;
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.NewPassword);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(BopUserServiceDefaults.PasswordSaltKeySize);
                    userPassword.PasswordSalt = saltKey;
                    userPassword.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey,
                        request.HashedPasswordFormat ?? _userSettings.HashedPasswordFormat);
                    break;
            }

            _userService.InsertUserPassword(userPassword);

            //publish event
            _eventPublisher.Publish(new UserPasswordChangedEvent(userPassword));

            return result;
        }

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newPhone">New email</param>
        /// <param name="requireValidation">Require validation of new email address</param>
        public virtual void SetPhone(User user, string newPhone, bool requireValidation)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (newPhone == null)
                throw new BopException("Email cannot be null");

            newPhone = newPhone.Trim();
            var oldEmail = user.Phone;

            if (!CommonHelper.IsValidPhone(newPhone))
                throw new BopException(_localizationService.GetResource("Account.EmailUsernameErrors.NewEmailIsNotValid"));

            if (newPhone.Length > 100)
                throw new BopException(_localizationService.GetResource("Account.EmailUsernameErrors.EmailTooLong"));

            var user2 = _userService.GetUserByPhone(newPhone);
            if (user2 != null && user.Id != user2.Id)
                throw new BopException(_localizationService.GetResource("Account.EmailUsernameErrors.EmailAlreadyExists"));

            user.Phone = newPhone;
            _userService.UpdateUser(user);

        }

        /// <summary>
        /// Sets a user username
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newUsername">New Username</param>
        public virtual void SetUsername(User user, string newUsername)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!_userSettings.UsernamesEnabled)
                throw new BopException("Usernames are disabled");

            newUsername = newUsername.Trim();

            if (newUsername.Length > BopUserServiceDefaults.UserUsernameLength)
                throw new BopException(_localizationService.GetResource("Account.EmailUsernameErrors.UsernameTooLong"));

            var user2 = _userService.GetUserByUsername(newUsername);
            if (user2 != null && user.Id != user2.Id)
                throw new BopException(_localizationService.GetResource("Account.EmailUsernameErrors.UsernameAlreadyExists"));

            user.Username = newUsername;
            _userService.UpdateUser(user);
        }

        #endregion
    }
}