using System;
using Bop.Core;
using Bop.Core.Infrastructure;
using Bop.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Bop.Web.Framework.Controllers
{
    [EnableCors(BopInfrastructureDefaults.BopCorsPolicyName)]
    public abstract class BaseController : Controller
    {

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exception">Exception</param>
        protected void LogException(Exception exception)
        {
            var logger = EngineContext.Current.Resolve<ILogger>();
            logger.Error(exception.Message, exception);
        }

    }
}