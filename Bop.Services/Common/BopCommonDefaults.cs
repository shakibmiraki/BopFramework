using Bop.Core.Caching;

namespace Bop.Services.Common
{
    /// <summary>
    /// Represents default values related to common services
    /// </summary>
    public static partial class BopCommonDefaults
    {

        #region Generic attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : key group
        /// </remarks>
        public static CacheKey GenericAttributeCacheKey => new CacheKey("Bop.genericattribute.{0}-{1}");



        #endregion


        #region Localization

        /// <summary>
        /// path Where the json file be written
        /// </summary>
        public static string JsonOutputPath => "~/ClientApp/src/assets/i18n/";
        

        #endregion

    }
}