using Bop.Core.Caching;
using Bop.Core.Domain.Users;
using Bop.Services.Events;


namespace Bop.Services.Users.Cache
{
    /// <summary>
    /// Customer cache event consumer (used for caching of current customer password)
    /// </summary>
    public partial class UserCacheEventConsumer : IConsumer<UserPasswordChangedEvent>
    {
        #region Fields

        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public UserCacheEventConsumer(IStaticCacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        //password changed
        public void HandleEvent(UserPasswordChangedEvent eventMessage)
        {
            _cacheManager.Remove(string.Format(BopUserServiceDefaults.UserPasswordLifetimeCacheKey, eventMessage.Password.UserId));
        }

        #endregion
    }
}