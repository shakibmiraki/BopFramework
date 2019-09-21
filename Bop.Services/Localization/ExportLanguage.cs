using System.Collections.Generic;

namespace Bop.Services.Localization
{
    public class ExportLanguage
    {
        public Dictionary<string, string> Resources { get; set; }

        public LocalizationConfiguration Configuration { get; set; }
    }

    public class LocalizationConfiguration
    {
        public string LanguageCode { get; set; }
    }

}
