using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bop.Core.Caching;
using Bop.Core.Data;
using Bop.Core.Domain.Localization;
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

            if (language is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

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

            //cache
            _cacheManager.RemoveByPrefix(BopLocalizationDefaults.LanguagesPrefixCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(language);
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Languages</returns>
        public virtual IList<Language> GetAllLanguages(bool loadCacheableCopy = true)
        {
            IList<Language> LoadLanguagesFunc()
            {
                var query = _languageRepository.Table;
                query = query.Where(l => l.Published);
                query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);
                return query.ToList();
            }

            IList<Language> languages;
            if (loadCacheableCopy)
            {
                //cacheable copy
                var key = string.Format(BopLocalizationDefaults.LanguagesAllCacheKey);
                languages = _cacheManager.Get(key, () =>
                {
                    var result = new List<Language>();
                    foreach (var language in LoadLanguagesFunc())
                        result.Add(new LanguageForCaching(language));
                    return result;
                });
            }
            else
            {
                languages = LoadLanguagesFunc();
            }

            return languages;
        }

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Language</returns>
        public virtual Language GetLanguageById(int languageId, bool loadCacheableCopy = true)
        {
            if (languageId == 0)
                return null;

            Language LoadLanguageFunc()
            {
                return _languageRepository.GetById(languageId);
            }

            if (!loadCacheableCopy) 
                return LoadLanguageFunc();

            //cacheable copy
            var key = string.Format(BopLocalizationDefaults.LanguagesByIdCacheKey, languageId);
            return _cacheManager.Get(key, () =>
            {
                var language = LoadLanguageFunc();
                return language == null ? null : new LanguageForCaching(language);
            });
        }

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="languageCulture">Language Culture</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Language</returns>
        public virtual Language GetLanguageByLanguageCulture(string languageCulture, bool loadCacheableCopy = true)
        {
            if (string.IsNullOrEmpty(languageCulture))
                return null;

            Language LoadLanguageFunc()
            {
                return _languageRepository.TableNoTracking.SingleOrDefault(language =>
                    language.LanguageCulture == languageCulture);
            }

            if (!loadCacheableCopy) 
                return LoadLanguageFunc();

            //cacheable copy
            var key = string.Format(BopLocalizationDefaults.LanguagesByIdCacheKey, languageCulture);
            return _cacheManager.Get(key, () =>
            {
                var language = LoadLanguageFunc();
                return language == null ? null : new LanguageForCaching(language);
            });
        }

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void InsertLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (language is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _languageRepository.Insert(language);

            //cache
            _cacheManager.RemoveByPrefix(BopLocalizationDefaults.LanguagesPrefixCacheKey);

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

            if (language is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            //update language
            _languageRepository.Update(language);

            //cache
            _cacheManager.RemoveByPrefix(BopLocalizationDefaults.LanguagesPrefixCacheKey);

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