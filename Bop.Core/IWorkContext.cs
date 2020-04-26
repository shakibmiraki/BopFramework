using Bop.Core.Domain.Customers;
using Bop.Core.Domain.Localization;
using Bop.Core.Domain.Site;


namespace Bop.Core
{
    /// <summary>
    /// Represents work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        Customer CurrentCustomer { get; set; }

        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        Language WorkingLanguage { get; set; }

        /// <summary>
        /// Gets the current store
        /// </summary>
        HostedSite CurrentSite { get; }

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }
    }
}
