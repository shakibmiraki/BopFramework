using System;

namespace Bop.Core.Domain.Users
{
    public class UserCard : BaseEntity
    {
        /// <summary>
        /// Gets or sets the card no. of user
        /// </summary>
        public string Cardno { get; set; }


        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

    }
}
