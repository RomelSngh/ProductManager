using AutoMapper;
using ProductManagement.Contracts;
using ProductManagement.Models;

namespace ProductManagement.Mapper
{
    public class MappingProfile : Profile
    {

        public MappingProfile() {

            CreateMap<Product, EditProductViewModel>()
                .ForMember(x => x.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(x => x.ProductCode, opt => opt.MapFrom(src => src.ProductCode))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(x => x.ImageName, opt => opt.MapFrom(src => src.Image))
                .ForMember(x => x.Image, opt => opt.MapFrom(src => new FormFile(null, 0, 0, "", "")));

            CreateMap<EditProductViewModel, Product>()
                .ForMember(x => x.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(x => x.ProductCode, opt => opt.MapFrom(src => src.ProductCode))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(x => x.Image, opt => opt.MapFrom(src => src.ImageName));

            CreateMap<Product, CreateProductViewModel>()
                .ForMember(x => x.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(x => x.ProductCode, opt => opt.MapFrom(src => src.ProductCode))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(x => x.ImageName, opt => opt.MapFrom(src => src.Image));

            CreateMap<CreateProductViewModel, Product>()
               .ForMember(x => x.ProductId, opt => opt.MapFrom(src => src.ProductId))
               .ForMember(x => x.ProductCode, opt => opt.MapFrom(src => src.ProductCode))
               .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(x => x.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(x => x.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
               .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
               .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(x => x.Image, opt => opt.MapFrom(src => src.ImageName));


         }
 
    }
}
