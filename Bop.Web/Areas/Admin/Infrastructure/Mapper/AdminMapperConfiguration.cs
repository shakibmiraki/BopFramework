using AutoMapper;
using Bop.Core.Infrastructure.Mapper;

namespace Bop.Web.Areas.Admin.Infrastructure.Mapper
{
    public class AdminMapperConfiguration : Profile, IOrderedMapperProfile
    {


        #region Ctor

        public AdminMapperConfiguration()
        {
            CreateReceiptsMaps();
        }
        #endregion


        /// <summary>
        /// Create receipt map
        /// </summary>
        private void CreateReceiptsMaps()
        {
            //CreateMap<ReceiptRequestModel, ReceiptRequest>().ReverseMap();
        }

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;
    }
}
