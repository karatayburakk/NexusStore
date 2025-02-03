using AutoMapper;
using NexusStore.API.Dtos;
using NexusStore.API.Entities;

namespace NexusStore.API.Mappings
{
  public class CategoryProfile : Profile
  {
    public CategoryProfile()
    {
      CreateMap<Category, CategoryResponseDto>();
      CreateMap<CreateCategoryDto, Category>();
      CreateMap<UpdateCategoryDto, Category>();
    }
  }
}