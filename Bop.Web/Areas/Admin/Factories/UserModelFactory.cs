using System;
using Bop.Core;
using Bop.Core.Domain.Users;

namespace Bop.Web.Areas.Admin.Factories
{
    public class UserModelFactory : IUserModelFactory
    {
        private readonly IWorkContext _workContext;

        public UserModelFactory(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        public UserCard PrepareUserCardRegister(string pan)
        {
            return new UserCard
            {
                Cardno = pan,
                CreatedOn = DateTime.Now,
                User = _workContext.CurrentUser
            };
        }
    }
}
