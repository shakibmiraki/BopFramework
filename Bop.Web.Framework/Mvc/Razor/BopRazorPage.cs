﻿using Bop.Core.Infrastructure;
using Bop.Services.Localization;
using Bop.Web.Framework.Localization;


namespace Bop.Web.Framework.Mvc.Razor
{
    /// <summary>
    /// Web view page
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    public abstract class BopRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        private ILocalizationService _localizationService;
        private Localizer _localizer;

        /// <summary>
        /// Get a localized resources
        /// </summary>
        public Localizer T
        {
            get
            {
                if (_localizationService == null)
                    _localizationService = EngineContext.Current.Resolve<ILocalizationService>();

                if (_localizer == null)
                {
                    _localizer = (format, args) =>
                    {
                        var resFormat = _localizationService.GetResource(format);
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new LocalizedString(format);
                        }
                        return new LocalizedString((args == null || args.Length == 0)
                            ? resFormat
                            : string.Format(resFormat, args));
                    };
                }
                return _localizer;
            }
        }
    }

    /// <summary>
    /// Web view page
    /// </summary>
    public abstract class BopRazorPage : BopRazorPage<dynamic>
    {
    }
}