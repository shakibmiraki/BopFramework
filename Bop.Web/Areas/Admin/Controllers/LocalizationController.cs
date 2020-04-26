using System;
using System.IO;
using System.Linq;
using Bop.Core;
using Bop.Services;
using Bop.Services.Localization;
using Bop.Services.Logging;
using Bop.Services.Security;
using Bop.Web.Areas.Admin.Models;
using Bop.Web.Framework;
using Bop.Web.Framework.Controllers;
using Bop.Web.Framework.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Bop.Web.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    public class LocalizationController : AdminBaseController
    {

        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;

        public LocalizationController(ILocalizationService localizationService, ILanguageService languageService, ILogger logger, IWorkContext workContext, IPermissionService permissionService)
        {
            _localizationService = localizationService;
            _languageService = languageService;
            _logger = logger;
            _workContext = workContext;
            _permissionService = permissionService;
        }

        [HttpPost]
        public IActionResult ExportLanguage([FromBody]int languageId)
        {
            var response = new LocalizationResponse { Result = ResultType.Error };

            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
            {
                response.Messages.Add(_localizationService.GetResource("localization.export.createfile.accessdenied"));
                return StatusCode(StatusCodes.Status403Forbidden, response);
            }

            var language = _languageService.GetLanguageById(languageId);

            try
            {
                var json = _localizationService.ExportResourcesToJson(language);
                response.Messages.Add(_localizationService.GetResource("localization.export.createfile.success"));
                response.JsonFile = json;
                response.Result = ResultType.Success;
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex, _workContext.CurrentCustomer);
                response.Messages.Add(_localizationService.GetResource("localization.export.createfile.error"));
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

        }



        [HttpPost]
        public IActionResult ImportLanguage()
        {
            var response = new ImportLanguageResponse { Result = ResultType.Error };
            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {

                    string jsonContent;
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        jsonContent = reader.ReadToEnd();
                    }
                    var importedLanguage = JsonConvert.DeserializeObject<ExportLanguage>(jsonContent);

                    var language = _languageService.GetLanguageById(importedLanguage.Configuration.LanguageId);
                    if (language is null)
                    {
                        response.Messages.Add(
                            _localizationService.GetResource("localization.language.import.notfound"));
                        return StatusCode(StatusCodes.Status204NoContent, response);
                    }

                    _localizationService.ImportResourcesFromJson(language, importedLanguage.Resources);

                    response.Messages.Add(
                        _localizationService.GetResource("localization.language.import.success"));
                    response.Result = ResultType.Success;
                    return Ok(response);
                }
                response.Messages.Add(
                    _localizationService.GetResource("localization.language.import.badrequest"));
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex, _workContext.CurrentCustomer);
                response.Messages.Add(_localizationService.GetResource("localization.language.import.error"));
                return StatusCode(500, response);
            }

        }

        [HttpPost]
        public IActionResult GetAllLanguages()
        {
            var response = new LanguageListResponse { Result = ResultType.Error };

            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
            {
                response.Messages.Add(_localizationService.GetResource("localization.language.getalllanguages.accessdenied"));
                return StatusCode(StatusCodes.Status403Forbidden, response);
            }

            var languages = _languageService.GetAllLanguages().Select(lang => new LanguageResponse
            {
                LanguageId = lang.Id,
                LanguageName = lang.Name,
                LanguageCulture = lang.LanguageCulture,
                LanguageCode = lang.UniqueSeoCode
            }).ToList();

            try
            {
                response.Languages = languages;
                response.Result = ResultType.Success;
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex, _workContext.CurrentCustomer);
                response.Messages.Add(_localizationService.GetResource("localization.language.getalllanguages.error"));
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

        }

    }
}