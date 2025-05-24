using AutoMapper;
using CatalogApi.DTOs;
using CatalogApi.Models;

namespace CatalogApi.AutoMappers;

public class ProductMapperProfile : Profile
{
    public ProductMapperProfile()
    {
        CreateMap<Product,ProductRequest>().ReverseMap();
        CreateMap<Product,ProductResponse>().ReverseMap();
    }
}