﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bop.Core;
using Bop.Core.Domain.Common;
using Bop.Core.Domain.Users;
using Bop.Services.Common;
using Bop.Services.Events;
using Bop.Services.Localization;


namespace Bop.Services.Messages
{
    /// <summary>
    /// Workflow message service
    /// </summary>
    public partial class WorkflowMessageService : IWorkflowMessageService
    {
        #region Fields

        private readonly CommonSettings _commonSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;

        #endregion

        #region Ctor

        public WorkflowMessageService(CommonSettings commonSettings,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IGenericAttributeService genericAttributeService)
        {
            _commonSettings = commonSettings;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _localizationService = localizationService;
            _workContext = workContext;
            _genericAttributeService = genericAttributeService;
        }

        #endregion

        #region Utilities



        /// <summary>
        /// Ensure language is active
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Return a value language identifier</returns>
        protected virtual int EnsureLanguageIsActive(int languageId)
        {
            //load language by specified ID
            var language = _languageService.GetLanguageById(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = _languageService.GetAllLanguages().FirstOrDefault();
            }

            if (language == null || !language.Published)
            {
                //load any language
                language = _languageService.GetAllLanguages().FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");

            return language.Id;
        }

        /// <summary>
        /// Get active message templates
        /// </summary>
        /// <returns>List of message templates</returns>
        protected virtual string GetActiveMessageTemplates(int languageId)
        {
            var messageTemplates = _localizationService.GetResource("Account.Register.Message.TokenDigit", languageId);
            return messageTemplates;
        }

        #endregion

        #region Methods

        #region Customer workflow

        /// <summary>
        /// Sends an email validation message to a customer
        /// </summary>
        /// <param name="user">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendUserPhoneValidationMessage(User user, int languageId)
        {

            var userPhone = user.Phone;
            var userToken = _genericAttributeService.GetAttribute<string>(user, BopUserDefaults.AccountActivationTokenAttribute);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(userPhone))
                throw new ArgumentNullException(nameof(user.Phone));

            if (string.IsNullOrEmpty(userToken))
                throw new ArgumentNullException(nameof(userToken));

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetActiveMessageTemplates(languageId);

            return SendNotification(messageTemplate, userToken, userPhone);

        }


        #endregion

        #region Misc
        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="token">Tokens</param>
        /// <param name="tophone">Recipient email address</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendNotification(string messageTemplate, string token, string tophone)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            return 1;
        }
        #endregion

        #endregion
    }
}