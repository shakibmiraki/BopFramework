﻿using System.Collections.Generic;
using Bop.Core;
using Bop.Core.Domain.Security;
using Bop.Core.Infrastructure;
using Bop.Services.Localization;
using Bop.Services.Security;
using Bop.Web.Framework.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Bop.Web.Framework.Security.Captcha
{
    /// <summary>
    /// HTML extensions
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Generate reCAPTCHA Control
        /// </summary>
        /// <param name="helper">HTML helper</param>
        /// <returns>Result</returns>
        public static IHtmlContent GenerateCaptcha(this IHtmlHelper helper)
        {
            var captchaSettings = EngineContext.Current.Resolve<CaptchaSettings>();

            //prepare language
            var language = (captchaSettings.ReCaptchaDefaultLanguage ?? string.Empty).ToLower();
            if (captchaSettings.AutomaticallyChooseLanguage)
            {
                //this list got from this site: https://developers.google.com/recaptcha/docs/language
                //but we use languages only with two letters in the code
                var supportedLanguageCodes = new List<string> { "af", "am", "ar", "az", "bg", "bn", "ca", "cs", "da", "de", "el", "en", "es", "et", "eu", "fa", "fi", "fil", "fr", "gl", "gu", "hi", "hr", "hu", "hy", "id", "is", "it", "iw", "ja", "ka", "kn", "ko", "lo", "lt", "lv", "ml", "mn", "mr", "ms", "nl", "no", "pl", "pt", "ro", "ru", "si", "sk", "sl", "sr", "sv", "sw", "ta", "te", "th", "tr", "uk", "ur", "vi", "zu" };

                var languageService = EngineContext.Current.Resolve<ILanguageService>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var twoLetterIsoCode = workContext.WorkingLanguage != null
                    ? languageService.GetTwoLetterIsoLanguageName(workContext.WorkingLanguage).ToLower()
                    : string.Empty;

                language = supportedLanguageCodes.Contains(twoLetterIsoCode) ? twoLetterIsoCode : language;
            }

            //prepare theme
            var theme = (captchaSettings.ReCaptchaTheme ?? string.Empty).ToLower();
            switch (theme)
            {
                case "blackglass":
                case "dark":
                    theme = "dark";
                    break;

                case "clean":
                case "red":
                case "white":
                case "light":
                default:
                    theme = "light";
                    break;
            }

            //prepare identifier
            var id = $"captcha_{CommonHelper.GenerateRandomInteger()}";

            //prepare public key
            var publicKey = captchaSettings.ReCaptchaPublicKey ?? string.Empty;

            //generate reCAPTCHA Control
            var scriptCallbackTag = new TagBuilder("script") { TagRenderMode = TagRenderMode.Normal };
            scriptCallbackTag.InnerHtml
                .AppendHtml($"var onloadCallback{id} = function() {{grecaptcha.render('{id}', {{'sitekey' : '{publicKey}', 'theme' : '{theme}' }});}};");

            var captchaTag = new TagBuilder("div") { TagRenderMode = TagRenderMode.Normal };
            captchaTag.Attributes.Add("id", id);

            var url = string.Format($"{BopSecurityDefaults.RecaptchaApiUrl}{BopSecurityDefaults.RecaptchaScriptPath}", id,
                !string.IsNullOrEmpty(language) ? $"&hl={language}" : string.Empty);
            var scriptLoadApiTag = new TagBuilder("script") { TagRenderMode = TagRenderMode.Normal };
            scriptLoadApiTag.Attributes.Add("src", url);
            scriptLoadApiTag.Attributes.Add("async", null);
            scriptLoadApiTag.Attributes.Add("defer", null);

            return new HtmlString(
                scriptCallbackTag.RenderHtmlContent() +
                captchaTag.RenderHtmlContent() +
                scriptLoadApiTag.RenderHtmlContent()
                );
        }
    }
}