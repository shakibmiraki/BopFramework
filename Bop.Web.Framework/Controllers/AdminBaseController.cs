using System;
using Bop.Core;
using Bop.Core.Infrastructure;
using Bop.Services;
using Bop.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Bop.Web.Framework.Controllers
{

    [AuthorizeAdmin]
    [EnableCors(BopInfrastructureDefaults.BopCorsPolicyName)]
    public abstract class AdminBaseController : Controller
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