using Bop.Web.Models.Users;

namespace Bop.Web.Factories
{
    public interface IUserModelFactory
    {
        /// <summary>
        /// Prepare the login model
        /// </summary>
        /// <returns>Login model</returns>
        LoginRequest PrepareLoginModel();

        /// <summary>
        /// Prepare the user register model
        /// </summary>
        /// <param name="model">Customer register model</param>
        /// <returns>Customer register model</returns>
        RegisterRequest PrepareRegisterModel(RegisterRequest model);
    }
}