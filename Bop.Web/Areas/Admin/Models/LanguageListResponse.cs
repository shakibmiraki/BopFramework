using System.Collections.Generic;
using Bop.Web.Framework.Models;

namespace Bop.Web.Areas.Admin.Models
{
    public class LanguageListResponse : BaseResponseModel
    {
        public List<LanguageResponse> Languages { get; set; }
    }
}
