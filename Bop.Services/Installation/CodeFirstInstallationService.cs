﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bop.Core;
using Bop.Data;
using Bop.Core.Domain.Common;
using Bop.Core.Domain.Localization;
using Bop.Core.Domain.Security;
using Bop.Core.Domain.Site;
using Bop.Core.Domain.Tasks;
using Bop.Core.Domain.Customers;
using Bop.Core.Infrastructure;
using Bop.Services.Configuration;
using Bop.Services.Security;
using Bop.Services.Customers;
using Bop.Services.Defaults;
using Bop.Services.Localization;

namespace Bop.Services.Installation
{
    /// <summary>
    /// Code first installation service
    /// </summary>
    public partial class CodeFirstInstallationService : IInstallationService
    {
        #region Fields


        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerRole> _customerRoleRepository;
        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
        private readonly IRepository<HostedSite> _hostedSiteRepository;
        private readonly IPermissionService _permissionService;
        private readonly IBopDataProvider _dataProvider;
        private readonly ILocalizationService _localizationService;


        #endregion

        #region Ctor

        public CodeFirstInstallationService(IRepository<Customer> customerRepository,
            IRepository<CustomerRole> customerRoleRepository,
            IRepository<Language> languageRepository,
            IRepository<ScheduleTask> scheduleTaskRepository,
            IRepository<HostedSite> hostedSiteRepository,
            IPermissionService permissionService,
            IBopDataProvider dataProvider,
            ILocalizationService localizationService)
        {
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
            _languageRepository = languageRepository;
            _scheduleTaskRepository = scheduleTaskRepository;
            _permissionService = permissionService;
            _hostedSiteRepository = hostedSiteRepository;
            _dataProvider = dataProvider;
            _localizationService = localizationService;
        }

        #endregion

        #region Utilities

        protected virtual T InsertInstallationData<T>(T entity) where T : BaseEntity
        {
            return _dataProvider.InsertEntity(entity);
        }

        protected virtual void InsertInstallationData<T>(params T[] entities) where T : BaseEntity
        {
            foreach (var entity in entities)
            {
                InsertInstallationData(entity);
            }
        }

        protected virtual void InstallHostedSite()
        {
            var stores = new List<HostedSite>
            {
                new HostedSite
                {
                    Name = "Shakib Miraki",
                    Url = "",
                    SslEnabled = false,
                    Hosts = "shakib-miraki.com,www.shakib-mirak.com",
                    DisplayOrder = 1,
                    CompanyName = "Shakib",
                    CompanyAddress = "Iran, Tehran",
                    CompanyPhoneNumber = "(98) 456-78905",
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

        protected virtual void InstallTranslactions()
        {

            var fa = _languageRepository.Table.FirstOrDefault(l => l.LanguageCulture == "fa-IR");
            var en = _languageRepository.Table.FirstOrDefault(l => l.LanguageCulture == "en-US");

            var resources = new List<LocaleStringResource>
            {
              new LocaleStringResource {
                LanguageId = fa.Id,
                ResourceName = "account.accountactivation.activation.code",
                ResourceValue = "کد فعالسازی"
            },
                new LocaleStringResource {
                LanguageId = fa.Id,
                ResourceName = "account.accountactivation.tokenexpired",
                ResourceValue = "کد فعال سازی نامعتبر می باشد"
            },
                new LocaleStringResource {
                LanguageId = fa.Id,
                ResourceName = "account.accountactivation.customeralreadyactivated",
                ResourceValue = "این کاربر قبلا فعال گردیده است"
            },
                new LocaleStringResource {
                LanguageId = fa.Id,
                ResourceName = "account.accountactivation.customernotexist",
                ResourceValue = "کاربری با این اطلاعات در سیستم ثبت نشده است"
            },
                new LocaleStringResource {
                LanguageId = fa.Id,
                ResourceName = "account.accountactivation.wrongtoken",
                ResourceValue = "کد وارد شده اشتباه می باشد"
            },
                new LocaleStringResource {
                LanguageId = fa.Id,
                ResourceName = "account.login.wrongcredentials",
                ResourceValue = "اطلاعات وارد شده برای ورود به سیستم اشتباه است"
            },                new LocaleStringResource {
                LanguageId = fa.Id,
                ResourceName = "account.login.wrongcredentials.customernotexist",
                ResourceValue = "کاربری با این اطلاعات در سیستم ثبت نشده است"
            },                new LocaleStringResource {
                LanguageId = fa.Id,
                ResourceName = "account.login.wrongcredentials.deleted",
                ResourceValue = "اکانت این کاربر حذف شده است"
            },                new LocaleStringResource {
                LanguageId = fa.Id,
                ResourceName = "account.login.wrongcredentials.notactive",
                ResourceValue = "این حساب هنوز فعال نشده است"
            },                new LocaleStringResource {
                LanguageId = fa.Id,
                ResourceName = "account.login.wrongcredentials.wrongcustomernameorpassword",
                ResourceValue = "نام کاربری یا رمز عبور اشتباه است"
            }
            };

            foreach (var resource in resources)
            {
                var localResource = _localizationService.GetLocaleStringResourceByName(resource.ResourceName, fa.Id);

                if (localResource is null)
                {
                    _localizationService.InsertLocaleStringResource(resource);
                }
                else
                {
                    localResource.ResourceValue = resource.ResourceValue;
                    _localizationService.UpdateLocaleStringResource(localResource);
                }

            }
        }



        protected virtual void InstallCustomersAndUsers(string defaultCustomerPhone, string defaultCustomerPassword)
        {
            var crAdministrators = new CustomerRole
            {
                Name = "Administrators",
                Active = true,
                IsSystemRole = true,
                SystemName = BopCustomerDefaults.AdministratorsRoleName
            };

            var crRegistered = new CustomerRole
            {
                Name = "Registered",
                Active = true,
                IsSystemRole = true,
                SystemName = BopCustomerDefaults.RegisteredRoleName
            };

            var customerRoles = new List<CustomerRole>
            {
                crAdministrators,
                crRegistered
            };
            _customerRoleRepository.Insert(customerRoles);

            //admin customer
            var adminUser = new Customer
            {
                Mobile = defaultCustomerPhone,
                Username = defaultCustomerPhone,
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
            };

            _customerRepository.Insert(adminUser);

            InsertInstallationData(
                new CustomerCustomerRoleMapping { CustomerId = adminUser.Id, CustomerRoleId = crAdministrators.Id },
                new CustomerCustomerRoleMapping { CustomerId = adminUser.Id, CustomerRoleId = crRegistered.Id });

            //set hashed admin password
            var customerRegistrationService = EngineContext.Current.Resolve<ICustomerRegistrationService>();
            customerRegistrationService.ChangePassword(new ChangePasswordRequest(defaultCustomerPhone, false,
                 PasswordFormat.Hashed, defaultCustomerPassword, null, BopCustomerServiceDefaults.DefaultHashedPasswordFormat));
        }



        protected virtual void InstallSettings()
        {
            var settingService = EngineContext.Current.Resolve<ISettingService>();


            settingService.SaveSetting(new CommonSettings
            {
                UseResponseCompression = true,
                StaticFilesCacheControl = "public,max-age=31536000"
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

            settingService.SaveSetting(new CustomerSettings
            {
                DefaultPasswordFormat = PasswordFormat.Hashed,
                HashedPasswordFormat = BopCustomerServiceDefaults.DefaultHashedPasswordFormat,
                PasswordMinLength = 6,
                PasswordRequireDigit = false,
                PasswordRequireLowercase = false,
                PasswordRequireNonAlphanumeric = false,
                PasswordRequireUppercase = false,
                UnduplicatedPasswordsNumber = 4,
                PasswordRecoveryLinkDaysValid = 7,
                PasswordLifetime = 90,
                FailedPasswordAllowedAttempts = 0,
                FailedPasswordLockoutMinutes = 30
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
        }



        protected virtual void InstallScheduleTasks()
        {
            var tasks = new List<ScheduleTask>
            {
                new ScheduleTask
                {
                    Name = "Clear cache",
                    Seconds = 600,
                    Type = "Nop.Services.Caching.ClearCacheTask, Nop.Services",
                    Enabled = false,
                    StopOnError = false
                },
                new ScheduleTask
                {
                    Name = "Clear log",
                    //60 minutes
                    Seconds = 3600,
                    Type = "Nop.Services.Logging.ClearLogTask, Nop.Services",
                    Enabled = false,
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
        /// <param name="defaultCustomerPhone">Default customer email</param>
        /// <param name="defaultCustomerPassword">Default customer password</param>
        public virtual void InstallRequiredData(string defaultCustomerPhone, string defaultCustomerPassword)
        {
            InstallHostedSite();
            InstallLanguages();
            InstallTranslactions();
            InstallSettings();
            InstallCustomersAndUsers(defaultCustomerPhone, defaultCustomerPassword);
            InstallScheduleTasks();
            InstallPermission();
        }

        #endregion
    }
}