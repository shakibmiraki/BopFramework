using System;
using System.Linq;
using Bop.Core.Domain.Users;
using Bop.Services.Tasks;

namespace Bop.Services.Common
{
    public class DeleteExpiredTokensTask : IScheduleTask
    {

        private readonly IGenericAttributeService _genericAttributeService;

        public DeleteExpiredTokensTask(IGenericAttributeService genericAttributeService)
        {
            _genericAttributeService = genericAttributeService;
        }


        public void Execute()
        {
            var tokens = _genericAttributeService.GetAttributesByKey(BopUserDefaults.AccountActivationTokenAttribute)
                .Where(token => token.InsertDate < DateTime.Now.AddMinutes(-2)).ToList();


            _genericAttributeService.DeleteAttributes(tokens);

        }
    }
}
