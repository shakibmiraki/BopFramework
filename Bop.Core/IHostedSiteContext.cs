using Bop.Core.Domain.Site;

namespace Bop.Core
{
    /// <summary>
    /// Store context
    /// </summary>
    public interface IHostedSiteContext
    {
        /// <summary>
        /// Gets the current hosted site
        /// </summary>
        HostedSite CurrentHostedSite { get; }
    }
}
