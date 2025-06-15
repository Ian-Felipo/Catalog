using AutoMapper;
using CatalogApi.DTOs;
using CatalogApi.Models;

namespace CatalogApi.AutoMappers;

public class CategoryMapperProfile : Profile
{
    public CategoryMapperProfile()
    {
        CreateMap<Category,CategoryRequest>().ReverseMap();
        CreateMap<Category,CategoryResponse>().ReverseMap();
    }
}