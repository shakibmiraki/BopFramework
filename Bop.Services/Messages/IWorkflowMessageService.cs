using System.Collections.Generic;
using Bop.Core.Domain.Users;


namespace Bop.Services.Messages
{
    /// <summary>
    /// Workflow message service
    /// </summary>
    public partial interface IWorkflowMessageService
    {
        #region Customer workflow


        /// <summary>
        /// Sends an phone validation message to a customer
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendUserPhoneValidationMessage(User user, int languageId);

        #endregion
    }
}