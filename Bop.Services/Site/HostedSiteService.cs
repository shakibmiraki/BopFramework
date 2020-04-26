using System;
using System.Collections.Generic;
using System.Linq;
using Bop.Core.Caching;
using Bop.Data;
using Bop.Core.Domain.Site;
using Bop.Services.Events;


namespace Bop.Services.Site
{
    /// <summary>
    /// Store service
    /// </summary>
    public partial class HostedSiteService : IHostedSiteService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<HostedSite> _hostedSiteRepository;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public HostedSiteService(IEventPublisher eventPublisher,
            IRepository<HostedSite> hostedSiteRepository,
            IStaticCacheManager cacheManager)
        {
            _eventPublisher = eventPublisher;
            _hostedSiteRepository = hostedSiteRepository;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a store
        /// </summary>
        /// <param name="hostedSite">Store</param>
        public virtual void DeleteHostedSite(HostedSite hostedSite)
        {
            if (hostedSite == null)
                throw new ArgumentNullException(nameof(hostedSite));

            var allStores = GetAllHostedSites();
            if (allStores.Count == 1)
                throw new Exception("You cannot delete the only configured store");

            _hostedSiteRepository.Delete(hostedSite);

            _cacheManager.RemoveByPrefix(BopHostedSiteDefaults.HostedSitesPrefixCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(hostedSite);
        }

        /// <summary>
        /// Gets all stores
        /// </summary>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Stores</returns>
        public virtual IList<HostedSite> GetAllHostedSites()
        {
            IList<HostedSite> LoadStoresFunc()
            {
                var query = from s in _hostedSiteRepository.Table orderby s.DisplayOrder, s.Id select s;
                return query.ToList();
            }

            return LoadStoresFunc();
        }

        /// <summary>
        /// Gets a store 
        /// </summary>
        /// <param name="hostedSiteId">Store identifier</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Store</returns>
        public virtual HostedSite GetHostedSiteById(int hostedSiteId)
        {
            if (hostedSiteId == 0)
                return null;

            HostedSite LoadStoreFunc()
            {
                return _hostedSiteRepository.GetById(hostedSiteId);
            }

            return LoadStoreFunc();
        }

        /// <summary>
        /// Inserts a store
        /// </summary>
        /// <param name="store">Store</param>
        public virtual void InsertHostedSite(HostedSite store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            _hostedSiteRepository.Insert(store);

            _cacheManager.RemoveByPrefix(BopHostedSiteDefaults.HostedSitesPrefixCacheKey);

            //event notification
            _eventPublisher.EntityInserted(store);
        }

        /// <summary>
        /// Updates the store
        /// </summary>
        /// <param name="store">Store</param>
        public virtual void UpdateHostedSite(HostedSite store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            _hostedSiteRepository.Update(store);

            _cacheManager.RemoveByPrefix(BopHostedSiteDefaults.HostedSitesPrefixCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(store);
        }

        /// <summary>
        /// Parse comma-separated Hosts
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns>Comma-separated hosts</returns>
        public virtual string[] ParseHostValues(HostedSite store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            var parsedValues = new List<string>();
            if (string.IsNullOrEmpty(store.Hosts))
                return parsedValues.ToArray();

            var hosts = store.Hosts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var host in hosts)
            {
                var tmp = host.Trim();
                if (!string.IsNullOrEmpty(tmp))
                    parsedValues.Add(tmp);
            }

            return parsedValues.ToArray();
        }

        /// <summary>
        /// Indicates whether a store contains a specified host
        /// </summary>
        /// <param name="store">Store</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        public virtual bool ContainsHostValue(HostedSite store, string host)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            if (string.IsNullOrEmpty(host))
                return false;

            var contains = ParseHostValues(store).Any(x => x.Equals(host, StringComparison.InvariantCultureIgnoreCase));

            return contains;
        }

        /// <summary>
        /// Returns a list of names of not existing stores
        /// </summary>
        /// <param name="storeIdsNames">The names and/or IDs of the store to check</param>
        /// <returns>List of names and/or IDs not existing stores</returns>
        public string[] GetNotExistingHostesSite(string[] storeIdsNames)
        {
            if (storeIdsNames == null)
                throw new ArgumentNullException(nameof(storeIdsNames));

            var query = _hostedSiteRepository.Table;
            var queryFilter = storeIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = query.Select(store => store.Name).Where(store => queryFilter.Contains(store)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (!queryFilter.Any())
                return queryFilter.ToArray();

            //filtering by IDs
            filter = query.Select(store => store.Id.ToString()).Where(store => queryFilter.Contains(store)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            return queryFilter.ToArray();
        }

        #endregion
    }
}