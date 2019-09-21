namespace Bop.Services.Installation
{
    /// <summary>
    /// Installation service
    /// </summary>
    public partial interface IInstallationService
    {
        /// <summary>
        /// Install required data
        /// </summary>
        /// <param name="defaultUserPhone">Default user email</param>
        /// <param name="defaultUserPassword">Default user password</param>
        void InstallRequiredData(string defaultUserPhone, string defaultUserPassword);
        
    }
}
