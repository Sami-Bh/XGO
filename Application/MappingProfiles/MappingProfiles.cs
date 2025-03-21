using Application.DTOs;
using AutoMapper;
using XGOModels;

namespace Application.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.HasChildren, opt => opt.MapFrom(source => source.SubCategories.Any()));
            CreateMap<CategoryDto, Category>();
            CreateMap<SubCategory, SubCategoryDto>()
                                .ForMember(dest => dest.HasChildren, opt => opt.MapFrom(source => source.Products.Any()));

            CreateMap<SubCategoryDto, SubCategory>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
