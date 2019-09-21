using System.Collections.Generic;
using Bop.Web.Framework.UI;

namespace Bop.Web.Framework.Models
{
    public class BaseResponseModel
    {
        protected BaseResponseModel()
        {
            Messages = new List<string>();
        }

        public ResultType Result { get; set; }

        public List<string> Messages { get; set; }
    }
}
