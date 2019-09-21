
namespace Bop.Web.Framework.Models
{
    /// <summary>
    /// Represents base entity model
    /// </summary>
    public partial class BaseEntityModel : BaseModel
    {
        /// <summary>
        /// Gets or sets model identifier
        /// </summary>
        public virtual int Id { get; set; }
    }
}