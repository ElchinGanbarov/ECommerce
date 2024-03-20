using AutoMapper;
using ECommerce.Application.Features.Commands.Products.CreateProduct;
using ECommerce.Application.Features.Queries.GetAllProduct;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, GetAllProductQueryRequest>().ForMember(x=>x.Name , z=>z.MapFrom(c=>c.Name));
        }
    }
}