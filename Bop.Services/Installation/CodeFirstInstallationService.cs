using System;
using System.Collections.Generic;
using System.Linq;
using Bop.Core;
using Bop.Core.Data;
using Bop.Core.Domain.Common;
using Bop.Core.Domain.Localization;
using Bop.Core.Domain.Security;
using Bop.Core.Domain.Site;
using Bop.Core.Domain.Tasks;
using Bop.Core.Domain.Users;
using Bop.Core.Infrastructure;
using Bop.Services.Configuration;
using Bop.Services.Security;
using Bop.Services.Users;


namespace Bop.Services.Installation
{
    /// <summary>
    /// Code first installation service
    /// </summary>
    public partial class CodeFirstInstallationService : IInstallationService
    {
        #region Fields


        private readonly IRepository<User> _customerRepository;
        private readonly IRepository<UserPassword> _customerPasswordRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
        private readonly IRepository<HostedSite> _hostedSiteRepository;
        private readonly IWebHelper _webHelper;
        private readonly IPermissionService _permissionService;




        #endregion

        #region Ctor

        public CodeFirstInstallationService(IRepository<User> customerRepository,
            IRepository<UserPassword> customerPasswordRepository,
            IRepository<UserRole> userRoleRepository,
            IRepository<Language> languageRepository,
            IRepository<ScheduleTask> scheduleTaskRepository,
            IRepository<HostedSite> hostedSiteRepository,
            IWebHelper webHelper, IPermissionService permissionService)
        {
            _customerRepository = customerRepository;
            _customerPasswordRepository = customerPasswordRepository;
            _userRoleRepository = userRoleRepository;
            _languageRepository = languageRepository;
            _scheduleTaskRepository = scheduleTaskRepository;
            _webHelper = webHelper;
            _permissionService = permissionService;
            _hostedSiteRepository = hostedSiteRepository;
        }

        #endregion

        #region Utilities

        protected virtual void InstallHostedSite()
        {
            var storeUrl = _webHelper.GetStoreLocation(false);
            var stores = new List<HostedSite>
            {
                new HostedSite
                {
                    Name = "Zeipt",
                    Url = storeUrl,
                    SslEnabled = false,
                    Hosts = "digital-receipt.com,www.digital-receipt.com",
                    DisplayOrder = 1,
                    CompanyName = "Iran Argham",
                    CompanyAddress = "Iran, Tehran",
                    CompanyPhoneNumber = "(98) 456-78901",
                    DefaultLanguageId = 0
                }
            };

            _hostedSiteRepository.Insert(stores);
        }


        protected virtual void InstallLanguages()
        {
            var languages = new List<Language>
            {
                new Language
                {
                    Name = "Persian",
                    LanguageCulture = "fa-IR",
                    UniqueSeoCode = "fa",
                    FlagImageFileName = "fa.png",
                    Published = true,
                    DisplayOrder = 1
                },
                new Language
                {
                    Name = "English",
                    LanguageCulture = "en-US",
                    UniqueSeoCode = "en",
                    FlagImageFileName = "us.png",
                    Published = true,
                    DisplayOrder = 2
                }
                
            };
            _languageRepository.Insert(languages);
        }

        //protected virtual void InstallLocaleResources()
        //{
        //    //'English' language
        //    var language = _languageRepository.Table.Single(l => l.Name == "English");

        //    //save resources
        //    var directoryPath = _fileProvider.MapPath(BopInstallationDefaults.LocalizationResourcesPath);
        //    var pattern = $"*.{BopInstallationDefaults.LocalizationResourcesFileExtension}";
        //    foreach (var filePath in _fileProvider.EnumerateFiles(directoryPath, pattern))
        //    {
        //        var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
        //        using (var streamReader = new StreamReader(filePath))
        //        {
        //            localizationService.ImportResourcesFromXml(language, streamReader);
        //        }
        //    }
        //}

        protected virtual void InstallCustomersAndUsers(string defaultUserPhone, string defaultUserPassword)
        {
            var crAdministrators = new UserRole
            {
                Name = "Administrators",
                Active = true,
                IsSystemRole = true,
                SystemName = BopUserDefaults.AdministratorsRoleName
            };

            var crRegistered = new UserRole
            {
                Name = "Registered",
                Active = true,
                IsSystemRole = true,
                SystemName = BopUserDefaults.RegisteredRoleName
            };

            var crGuests = new UserRole
            {
                Name = "Guests",
                Active = true,
                IsSystemRole = true,
                SystemName = BopUserDefaults.GuestsRoleName
            };

            var userRoles = new List<UserRole>
            {
                crAdministrators,
                crRegistered,
                crGuests
            };
            _userRoleRepository.Insert(userRoles);

            //admin user
            var adminUser = new User
            {
                Phone = defaultUserPhone,
                Username = defaultUserPhone,
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
            };

            adminUser.AddUserRoleMapping(new UserUserRoleMapping { UserRole = crAdministrators });
            adminUser.AddUserRoleMapping(new UserUserRoleMapping { UserRole = crRegistered });

            _customerRepository.Insert(adminUser);


            //set hashed admin password
            var customerRegistrationService = EngineContext.Current.Resolve<IUserRegistrationService>();
            customerRegistrationService.ChangePassword(new ChangePasswordRequest(defaultUserPhone, false,
                 PasswordFormat.Hashed, defaultUserPassword, null, BopUserServiceDefaults.DefaultHashedPasswordFormat));
        }


        protected virtual void InstallSettings()
        {
            var settingService = EngineContext.Current.Resolve<ISettingService>();


            settingService.SaveSetting(new CommonSettings
            {
                UseResponseCompression = true
            });


            settingService.SaveSetting(new LocalizationSettings
            {
                DefaultAdminLanguageId = _languageRepository.Table.Single(l => l.Name == "English").Id,
                UseImagesForLanguageSelection = false,
                SeoFriendlyUrlsForLanguagesEnabled = false,
                AutomaticallyDetectLanguage = true,
                LoadAllLocaleRecordsOnStartup = true,
                LoadAllLocalizedPropertiesOnStartup = true,
                LoadAllUrlRecordsOnStartup = false,
                IgnoreRtlPropertyForAdminArea = false
            });

            settingService.SaveSetting(new UserSettings
            {
                UsernamesEnabled = false,
                AllowUsersToChangeUsernames = false,
                DefaultPasswordFormat = PasswordFormat.Hashed,
                HashedPasswordFormat = BopUserServiceDefaults.DefaultHashedPasswordFormat,
                PasswordMinLength = 6,
                PasswordRequireDigit = false,
                PasswordRequireLowercase = false,
                PasswordRequireNonAlphanumeric = false,
                PasswordRequireUppercase = false,
                UnduplicatedPasswordsNumber = 4,
                PasswordRecoveryLinkDaysValid = 7,
                PasswordLifetime = 90,
                FailedPasswordAllowedAttempts = 0,
                FailedPasswordLockoutMinutes = 30,
                AllowViewingProfiles = false,
                DeleteGuestTaskOlderThanMinutes = 1440
            });


            settingService.SaveSetting(new SecuritySettings
            {
                ForceSslForAllPages = true,
                EncryptionKey = CommonHelper.GenerateRandomDigitCode(16),
                AdminAreaAllowedIpAddresses = null,
                EnableXsrfProtectionForAdminArea = true,
                EnableXsrfProtectionForPublicStore = true,
                HoneypotEnabled = false,
                HoneypotInputName = "hpinput",
                AllowNonAsciiCharactersInHeaders = true
            });


            settingService.SaveSetting(new CaptchaSettings
            {
                Enabled = true,
                ShowOnLoginPage = true,
                ShowOnRegistrationPage = true,
                ReCaptchaDefaultLanguage = "fa",
                AutomaticallyChooseLanguage = false
            });

        }



        protected virtual void InstallScheduleTasks()
        {
            var tasks = new List<ScheduleTask>
            {
                new ScheduleTask
                {
                    Name = "Clear cache",
                    Seconds = 3600,
                    Type = "Bop.Services.Caching.ClearCacheTask, Bop.Services",
                    Enabled = false,
                    StopOnError = false
                },
                //new ScheduleTask
                //{
                //    Name = "Clear log",
                //    //60 minutes
                //    Seconds = 3600,
                //    Type = "Nop.Services.Logging.ClearLogTask, Nop.Services",
                //    Enabled = false,
                //    StopOnError = false
                //},
                new ScheduleTask
                {
                    Name = "Delete guests",
                    Seconds = 43200,
                    Type = "Bop.Services.Users.DeleteGuestsTask, Bop.Services",
                    Enabled = true,
                    StopOnError = false
                },
                new ScheduleTask
                {
                    Name = "Delete expired tokens",
                    Seconds = 60,
                    Type = "Bop.Services.Common.DeleteExpiredTokensTask, Bop.Services",
                    Enabled = true,
                    StopOnError = false
                },
            };

            _scheduleTaskRepository.Insert(tasks);
        }


        protected virtual void InstallPermission()
        {

            //register default permissions
            var permissionProviders = new List<Type> { typeof(StandardPermissionProvider) };
            foreach (var providerType in permissionProviders)
            {
                var provider = (IPermissionProvider)Activator.CreateInstance(providerType);
                _permissionService.InstallPermissions(provider);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Install required data
        /// </summary>
        /// <param name="defaultUserPhone">Default user email</param>
        /// <param name="defaultUserPassword">Default user password</param>
        public virtual void InstallRequiredData(string defaultUserPhone, string defaultUserPassword)
        {
            InstallHostedSite();
            InstallLanguages();
            InstallSettings();
            InstallCustomersAndUsers(defaultUserPhone, defaultUserPassword);
            InstallScheduleTasks();
            InstallPermission();
        }

        #endregion
    }
}