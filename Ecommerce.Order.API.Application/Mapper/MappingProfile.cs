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
            CreateMap<OrderModel, OrderResponseModel>();
            CreateMap<OrderRequestModel, OrderModel>();
            CreateMap<ProductModel, ProductResponseModel>();

            CreateMap<OrderDetailRequestModel, OrderDetailModel>();
            CreateMap<OrderDetailModel, OrderDetailResponseModel>();
            CreateMap<OrderDetailModel, OrderDetailUpdateUnitsResponseModel>();
        }
    }
}
