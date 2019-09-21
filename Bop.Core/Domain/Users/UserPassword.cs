using System;
using Bop.Core;

namespace Bop.Core.Domain.Users
{
    /// <summary>
    /// Represents a user password
    /// </summary>
    public partial class UserPassword : BaseEntity
    {
        public UserPassword()
        {
            PasswordFormat = PasswordFormat.Clear;
        }

        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password format identifier
        /// </summary>
        public int PasswordFormatId { get; set; }

        /// <summary>
        /// Gets or sets the password salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        public PasswordFormat PasswordFormat
        {
            get => (PasswordFormat)PasswordFormatId;
            set => PasswordFormatId = (int)value;
        }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public virtual User User { get; set; }
    }
}