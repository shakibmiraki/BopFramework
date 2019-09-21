using Bop.Web.Framework.Models;

namespace Bop.Web.Areas.Admin.Models
{
    public class LanguageResponse : BaseResponseModel
    {
        public int LanguageId { get; set; }

        public string LanguageName { get; set; }

        public string LanguageCulture { get; set; }

        public string LanguageCode { get; set; }
    }
}
