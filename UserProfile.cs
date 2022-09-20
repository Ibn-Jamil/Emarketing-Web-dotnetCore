using AngularCoreEmarketing.Dtos;
using AngularCoreEmarketing.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing
{
    public class UserProfile :Profile
    {
        public UserProfile()
        {
            CreateMap<CustomerReview, CustomerReviewDto>();
            CreateMap<CustomerReviewDto, CustomerReview>().ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<ProductDetails, ProductDetailsDto>();
            CreateMap<ImagesDirectory, ImagesDirectoryDto>();
            CreateMap<ProductSizes, productSizesDto>();

            CreateMap<ProductDetailsDto, ProductDetails>().ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<ProductFeedback, ProductFeedbackDto>();
            CreateMap<ProductFeedbackDto, ProductFeedback>().ForMember(v => v.Id, opt => opt.Ignore());

            CreateMap<CatagoryMajor, CatagoryMajorDto>().ForMember(c=>c.CatagoryMajorSubDto,opt=>opt.MapFrom(v=>v.MajorSubCatagory));
            CreateMap<CatagoryMajorDto, CatagoryMajor>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CatagoryMajorSub, CatagoryMajorSubDto>().ForMember(c => c.CatagorySpecificDto, opt => opt.MapFrom(v => v.SpecificCatagory)); ;
            CreateMap<CatagoryMajorSubDto, CatagoryMajorSub>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CatagorySpecific, CatagorySpecificDto>();
            CreateMap<CatagorySpecificDto, CatagorySpecific>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Cart, CartDto>().ForMember(c => c.ProductDto, opt => opt.MapFrom(p => p.Product));
            CreateMap<CartDto, Cart>().ForMember(c => c.Id, opt => opt.Ignore());

        }
    }
}
