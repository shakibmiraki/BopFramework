using Bop.Core.Configuration;
using Bop.Web.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;


namespace Bop.Web.Controllers
{
    public class ApiSettingController : BaseController
    {
        private readonly SpaConfig _spaConfig;

        public ApiSettingController(SpaConfig spaConfig)
        {
            _spaConfig = spaConfig;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_spaConfig); // For the Angular Client
        }

    }
}