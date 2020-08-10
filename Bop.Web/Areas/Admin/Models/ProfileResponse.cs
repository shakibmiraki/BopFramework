using Bop.Web.Framework.Models;
using System;

namespace Bop.Web.Areas.Admin.Models
{
    public class ProfileResponse : BaseResponseModel
    {

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string NationalCode { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public DateTimeOffset BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public bool Gender { get; set; }
    }
}
