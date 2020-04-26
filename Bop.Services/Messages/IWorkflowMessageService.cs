using Bop.Core.Domain.Customers;


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
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerPhoneValidationMessage(Customer customer, int languageId);

        #endregion
    }
}