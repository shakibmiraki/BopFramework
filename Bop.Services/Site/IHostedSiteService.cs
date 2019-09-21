using System.Collections.Generic;
using Bop.Core.Domain.Site;


namespace Bop.Services.Site
{
    /// <summary>
    /// Store service interface
    /// </summary>
    public partial interface IHostedSiteService
    {
        /// <summary>
        /// Deletes a store
        /// </summary>
        /// <param name="hostedSite">Store</param>
        void DeleteHostedSite(HostedSite hostedSite);

        /// <summary>
        /// Gets all stores
        /// </summary>
        /// <returns>Stores</returns>
        IList<HostedSite> GetAllHostedSites();

        /// <summary>
        /// Gets a store 
        /// </summary>
        /// <param name="hostedSiteId">Store identifier</param>
        /// <returns>Store</returns>
        HostedSite GetHostedSiteById(int hostedSiteId);

        /// <summary>
        /// Inserts a store
        /// </summary>
        /// <param name="store">Store</param>
        void InsertHostedSite(HostedSite store);

        /// <summary>
        /// Updates the store
        /// </summary>
        /// <param name="store">Store</param>
        void UpdateHostedSite(HostedSite store);

        /// <summary>
        /// Parse comma-separated Hosts
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns>Comma-separated hosts</returns>
        string[] ParseHostValues(HostedSite store);

        /// <summary>
        /// Indicates whether a store contains a specified host
        /// </summary>
        /// <param name="store">Store</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        bool ContainsHostValue(HostedSite store, string host);

        /// <summary>
        /// Returns a list of names of not existing stores
        /// </summary>
        /// <param name="storeIdsNames">The names and/or IDs of the store to check</param>
        /// <returns>List of names and/or IDs not existing stores</returns>
        string[] GetNotExistingHostesSite(string[] storeIdsNames);
    }
}