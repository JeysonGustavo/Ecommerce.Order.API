using AutoMapper;
using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Request;
using Ecommerce.Order.API.Core.Models.Response;

namespace Ecommerce.Order.API.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Request
            CreateMap<OrderRequestModel, OrderModel>();
            CreateMap<OrderDetailRequestModel, OrderDetailModel>();
            #endregion

            #region Response
            CreateMap<OrderModel, OrderResponseModel>();
            CreateMap<ProductModel, ProductResponseModel>();
            CreateMap<OrderDetailModel, OrderDetailResponseModel>();
            CreateMap<OrderDetailModel, OrderDetailUpdateUnitsResponseModel>();
            #endregion
        }
    }
}
