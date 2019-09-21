using System;
using Bop.Core.Domain.Security;
using Bop.Core.Domain.Users;
using Bop.Web.Models.Users;


namespace Bop.Web.Factories
{
    public class UserModelFactory : IUserModelFactory
    {
        private readonly UserSettings _userSettings;
        private readonly CaptchaSettings _captchaSettings;

        public UserModelFactory(UserSettings userSettings, CaptchaSettings captchaSettings)
        {
            _userSettings = userSettings;
            _captchaSettings = captchaSettings;
        }

        public LoginRequest PrepareLoginModel()
        {
            var model = new LoginRequest
            {
                DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage
            };
            return model;
        }

                /// <summary>
        /// Prepare the customer register model
        /// </summary>
        /// <param name="model">Customer register model</param>
        /// <returns>Customer register model</returns>
        public virtual RegisterRequest PrepareRegisterModel(RegisterRequest model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnRegistrationPage;

            return model;
        }
    }
}
