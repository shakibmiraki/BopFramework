using System;
using System.Collections.Generic;
using System.Text;
using Bop.Core.Domain.Users;
using Bop.Services.Tasks;

namespace Bop.Services.Users
{
    /// <summary>
    /// Represents a task for deleting guest customers
    /// </summary>
    public class DeleteGuestsTask : IScheduleTask
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly IUserService _userService;

        #endregion

        #region Ctor

        public DeleteGuestsTask(UserSettings userSettings,
            IUserService userService)
        {
            _userSettings = userSettings;
            _userService = userService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            var olderThanMinutes = _userSettings.DeleteGuestTaskOlderThanMinutes;
            // Default value in case 0 is returned.  0 would effectively disable this service and harm performance.
            olderThanMinutes = olderThanMinutes == 0 ? 1440 : olderThanMinutes;

            _userService.DeleteGuestUsers(null, DateTime.UtcNow.AddMinutes(-olderThanMinutes));
        }

        #endregion
    }
}
