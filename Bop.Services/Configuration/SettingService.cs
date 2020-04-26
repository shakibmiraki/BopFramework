using Bop.Core;
using Bop.Core.Caching;
using Bop.Core.Configuration;
using Bop.Core.Domain.Configuration;
using Bop.Data;
using Bop.Services.Caching.CachingDefaults;
using Bop.Services.Caching.Extensions;
using Bop.Services.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Bop.Services.Configuration
{
    /// <summary>
    /// Setting manager
    /// </summary>
    public partial class SettingService : ISettingService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Setting> _settingRepository;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public SettingService(IEventPublisher eventPublisher,
            IRepository<Setting> settingRepository,
            IStaticCacheManager cacheManager)
        {
            _eventPublisher = eventPublisher;
            _settingRepository = settingRepository;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        protected virtual IDictionary<string, IList<Setting>> GetAllSettingsDictionary()
        {
            //cache
            return _cacheManager.Get(BopConfigurationCachingDefaults.SettingsAllAsDictionaryCacheKey, () =>
            {
                var settings = GetAllSettings();

                var dictionary = new Dictionary<string, IList<Setting>>();
                foreach (var s in settings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    var settingForCaching = new Setting
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Value = s.Value
                    };
                    if (!dictionary.ContainsKey(resourceName))
                    {
                        //first setting
                        dictionary.Add(resourceName, new List<Setting>
                        {
                            settingForCaching
                        });
                    }
                    else
                    {
                        //already added
                        //most probably it's the setting with the same name but for some certain store (storeId > 0)
                        dictionary[resourceName].Add(settingForCaching);
                    }
                }

                return dictionary;
            });
        }

        /// <summary>
        /// Set setting value
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        protected virtual void SetSetting(Type type, string key, object value, bool clearCache = true)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            key = key.Trim().ToLowerInvariant();
            var valueStr = TypeDescriptor.GetConverter(type).ConvertToInvariantString(value);

            var allSettings = GetAllSettingsDictionary();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                //update
                var setting = GetSettingById(settingForCaching.Id);
                setting.Value = valueStr;
                UpdateSetting(setting, clearCache);
            }
            else
            {
                //insert
                var setting = new Setting
                {
                    Name = key,
                    Value = valueStr
                };
                InsertSetting(setting, clearCache);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void InsertSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            _settingRepository.Insert(setting);

            //cache
            if (clearCache)
                ClearCache();

            //event notification
            _eventPublisher.EntityInserted(setting);
        }

        /// <summary>
        /// Updates a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void UpdateSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            _settingRepository.Update(setting);

            //cache
            if (clearCache)
                ClearCache();

            //event notification
            _eventPublisher.EntityUpdated(setting);
        }

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        public virtual void DeleteSetting(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            _settingRepository.Delete(setting);

            //cache
            ClearCache();

            //event notification
            _eventPublisher.EntityDeleted(setting);
        }

        /// <summary>
        /// Deletes settings
        /// </summary>
        /// <param name="settings">Settings</param>
        public virtual void DeleteSettings(IList<Setting> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _settingRepository.Delete(settings);

            //cache
            ClearCache();

            //event notification
            foreach (var setting in settings)
            {
                _eventPublisher.EntityDeleted(setting);
            }
        }

        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <param name="settingId">Setting identifier</param>
        /// <returns>Setting</returns>
        public virtual Setting GetSettingById(int settingId)
        {
            if (settingId == 0)
                return null;

            return _settingRepository.ToCachedGetById(settingId);
        }

        /// <summary>
        /// Get setting by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all stores) value should be loaded if a value specific for a certain is not found</param>
        /// <returns>Setting</returns>
        public virtual Setting GetSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var settings = GetAllSettingsDictionary();
            key = key.Trim().ToLowerInvariant();
            if (!settings.ContainsKey(key))
                return null;

            var settingsByKey = settings[key];
            var setting = settingsByKey.FirstOrDefault();

            return setting != null ? GetSettingById(setting.Id) : null;
        }


        /// <summary>
        /// Get setting value by key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Setting value</returns>
        public virtual T GetSettingByKey<T>(string key, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsDictionary();
            key = key.Trim().ToLowerInvariant();
            if (!settings.ContainsKey(key))
                return defaultValue;

            var settingsByKey = settings[key];
            var setting = settingsByKey.FirstOrDefault();

            return setting != null ? CommonHelper.To<T>(setting.Value) : defaultValue;
        }

        /// <summary>
        /// Set setting value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void SetSetting<T>(string key, T value, bool clearCache = true)
        {
            SetSetting(typeof(T), key, value, clearCache);
        }

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        public virtual IList<Setting> GetAllSettings()
        {
            var query = from s in _settingRepository.Table
                        orderby s.Name
                        select s;

            var settings = query.ToCachedList(BopConfigurationCachingDefaults.SettingsAllCacheKey);

            return settings;
        }

        /// <summary>
        /// Determines whether a setting exists
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <returns>true -setting exists; false - does not exist</returns>
        public virtual bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            var key = GetSettingKey(settings, keySelector);

            var setting = GetSettingByKey<string>(key);
            return setting != null;
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public virtual T LoadSetting<T>() where T : ISettings, new()
        {
            return (T)LoadSetting(typeof(T));
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <param name="type">Type</param>
        public virtual ISettings LoadSetting(Type type)
        {
            var settings = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = type.Name + "." + prop.Name;
                //load by store
                var setting = GetSettingByKey<string>(key);
                if (setting == null)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                    continue;

                var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                //set property
                prop.SetValue(settings, value, null);
            }

            return settings as ISettings;
        }

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="settings">Setting instance</param>
        public virtual void SaveSetting<T>(T settings) where T : ISettings, new()
        {
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                var key = typeof(T).Name + "." + prop.Name;
                var value = prop.GetValue(settings, null);
                if (value != null)
                    SetSetting(prop.PropertyType, key, value, false);
                else
                    SetSetting(key, string.Empty, false);
            }

            //and now clear cache
            ClearCache();
        }

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="storeId">Store ID</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool clearCache = true) where T : ISettings, new()
        {
            if (!(keySelector.Body is MemberExpression member))
            {
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");
            }

            var key = GetSettingKey(settings, keySelector);
            var value = (TPropType)propInfo.GetValue(settings, null);
            if (value != null)
                SetSetting(key, value, clearCache);
            else
                SetSetting(key, string.Empty, clearCache);
        }

        /// <summary>
        /// Delete all settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public virtual void DeleteSetting<T>() where T : ISettings, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = GetAllSettings();
            foreach (var prop in typeof(T).GetProperties())
            {
                var key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            DeleteSettings(settingsToDelete);
        }

        /// <summary>
        /// Delete settings object
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        public virtual void DeleteSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector) where T : ISettings, new()
        {
            var key = GetSettingKey(settings, keySelector);
            key = key.Trim().ToLowerInvariant();

            var allSettings = GetAllSettingsDictionary();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;
            if (settingForCaching == null)
                return;

            //update
            var setting = GetSettingById(settingForCaching.Id);
            DeleteSetting(setting);
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public virtual void ClearCache()
        {
            _cacheManager.RemoveByPrefix(BopConfigurationCachingDefaults.SettingsPrefixCacheKey);
        }

        /// <summary>
        /// Get setting key (stored into database)
        /// </summary>
        /// <typeparam name="TSettings">Type of settings</typeparam>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <returns>Key</returns>
        public virtual string GetSettingKey<TSettings, T>(TSettings settings, Expression<Func<TSettings, T>> keySelector)
            where TSettings : ISettings, new()
        {
            if (!(keySelector.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");

            if (!(member.Member is PropertyInfo propInfo))
                throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");

            var key = $"{typeof(TSettings).Name}.{propInfo.Name}";

            return key;
        }
        #endregion
    }
}