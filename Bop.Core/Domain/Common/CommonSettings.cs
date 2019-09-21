using Bop.Core.Configuration;


namespace Bop.Core.Domain.Common
{
    /// <summary>
    /// Common settings
    /// </summary>
    public class CommonSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to compress response (gzip by default). 
        /// You may want to disable it, for example, If you have an active IIS Dynamic Compression Module configured at the server level
        /// </summary>
        public bool UseResponseCompression { get; set; }


        /// <summary>
        /// The length of time, in milliseconds, before the running schedule task times out. Set null to use default value
        /// </summary>
        public int? ScheduleTaskRunTimeout { get; set; }

    }
}