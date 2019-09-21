using Bop.Core.Domain.Users;

namespace Bop.Web.Areas.Admin.Factories
{
    public interface IUserModelFactory
    {
        UserCard PrepareUserCardRegister(string pan);
    }
}
