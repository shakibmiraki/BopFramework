using System;
using System.Collections.Generic;
using System.Linq;

namespace Bop.Core.Domain.Users
{
    /// <summary>
    /// Represents a user
    /// </summary>
    public class User : BaseEntity
    {

        private ICollection<UserUserRoleMapping> _userUserRoleMappings;
        private IList<UserRole> _userRoles;
        private IList<UserCard> _userCards;


        /// <summary>
        /// Ctor
        /// </summary>
        public User()
        {
            this.UserGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the user GUID
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the phone that should be re-validated. Used in scenarios when a user is already registered and wants to change an email address.
        /// </summary>
        public string PhoneToRevalidate { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the user is required to re-login
        /// </summary>
        public bool RequireReLogin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating number of failed login attempts (wrong password)
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// Gets or sets the date and time until which a user cannot login (locked out)
        /// </summary>
        public DateTime? CannotLoginUntilDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the user system name
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
        /// Gets or sets user roles
        /// </summary>
        public virtual IList<UserRole> UserRoles => _userRoles ?? (_userRoles = UserUserRoleMappings.Select(mapping => mapping.UserRole).ToList());


        /// <summary>
        /// Gets or sets user tokens
        /// </summary>
        public virtual IList<UserCard> UserCards => _userCards;


        /// <summary>
        /// Gets or sets user tokens
        /// </summary>
        public virtual ICollection<UserToken> UserTokens { get; set; }

        /// <summary>
        /// Gets or sets user-user role mappings
        /// </summary>
        public virtual ICollection<UserUserRoleMapping> UserUserRoleMappings
        {
            get => _userUserRoleMappings ?? (_userUserRoleMappings = new List<UserUserRoleMapping>());
            protected set => _userUserRoleMappings = value;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Add user role and reset user roles cache
        /// </summary>
        /// <param name="role">Role</param>
        public void AddUserRoleMapping(UserUserRoleMapping role)
        {
            UserUserRoleMappings.Add(role);
            _userRoles = null;
        }

        /// <summary>
        /// Remove user role and reset user roles cache
        /// </summary>
        /// <param name="role">Role</param>
        public void RemoveCustomerRoleMapping(UserUserRoleMapping role)
        {
            UserUserRoleMappings.Remove(role);
            _userRoles = null;
        }

        #endregion
    }
}