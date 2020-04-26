﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bop.Core.Caching;
using Bop.Core.Domain.Localization;
using Bop.Data;
using Bop.Services.Caching.CachingDefaults;
using Bop.Services.Caching.Extensions;
using Bop.Services.Configuration;
using Bop.Services.Events;

namespace Bop.Services.Localization
{
    /// <summary>
    /// Language service
    /// </summary>
    public partial class LanguageService : ILanguageService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Language> _languageRepository;
        private readonly ISettingService _settingService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public LanguageService(IEventPublisher eventPublisher,
            IRepository<Language> languageRepository,
            ISettingService settingService,
            IStaticCacheManager cacheManager,
            LocalizationSettings localizationSettings)
        {
            _eventPublisher = eventPublisher;
            _languageRepository = languageRepository;
            _settingService = settingService;
            _cacheManager = cacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void DeleteLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));
            
            //update default admin area language (if required)
            if (_localizationSettings.DefaultAdminLanguageId == language.Id)
            {
                foreach (var activeLanguage in GetAllLanguages())
                {
                    if (activeLanguage.Id == language.Id) 
                        continue;

                    _localizationSettings.DefaultAdminLanguageId = activeLanguage.Id;
                    _settingService.SaveSetting(_localizationSettings);
                    break;
                }
            }

            _languageRepository.Delete(language);
            
            //event notification
            _eventPublisher.EntityDeleted(language);
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Languages</returns>
        public virtual IList<Language> GetAllLanguages(bool showHidden = false, int storeId = 0)
        {
            var query = _languageRepository.Table;
            if (!showHidden) query = query.Where(l => l.Published);
            query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);

            //cacheable copy
            var key = BopLocalizationCachingDefaults.LanguagesAllCacheKey.FillCacheKey(storeId, showHidden);
            
            var languages = _cacheManager.Get(key, () =>
            {
                var allLanguages = query.ToList();

                //store mapping
                if (storeId > 0)
                {
                    allLanguages = allLanguages
                        .ToList();
                }

                return allLanguages;
            });

            return languages;
        }

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Language</returns>
        public virtual Language GetLanguageById(int languageId)
        {
            if (languageId == 0)
                return null;

            return _languageRepository.ToCachedGetById(languageId);
        }

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void InsertLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            _languageRepository.Insert(language);

            //event notification
            _eventPublisher.EntityInserted(language);
        }

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void UpdateLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            //update language
            _languageRepository.Update(language);

            //event notification
            _eventPublisher.EntityUpdated(language);
        }

        /// <summary>
        /// Get 2 letter ISO language code
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>ISO language code</returns>
        public virtual string GetTwoLetterIsoLanguageName(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (string.IsNullOrEmpty(language.LanguageCulture))
                return "en";

            var culture = new CultureInfo(language.LanguageCulture);
            var code = culture.TwoLetterISOLanguageName;

            return string.IsNullOrEmpty(code) ? "en" : code;
        }

        #endregion
    }
}