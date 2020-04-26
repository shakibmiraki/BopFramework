using Bop.Core.Configuration;

namespace Bop.Core.Domain
{
    /// <summary>
    /// Store information settings
    /// </summary>
    public class HostedSiteInformationSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether mini profiler should be displayed in public store (used for debugging)
        /// </summary>
        public bool DisplayMiniProfilerInPublicStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mini profiler should be displayed only for users with access to the admin area
        /// </summary>
        public bool DisplayMiniProfilerForAdminOnly { get; set; }
    }
}
