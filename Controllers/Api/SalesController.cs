using AngularCoreEmarketing.Data;
using AngularCoreEmarketing.Dtos;
using AngularCoreEmarketing.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AngularCoreEmarketing.Controllers.Api
{
    [Route("Api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SalesController(IMapper mapper, ApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public List<ProductDetailsDto> List(int PageIndex)
        {
            int showEntries = 6, SkipEntries = (PageIndex - 1) * 6;
            var ProductList = _context.ProductsDetails.Where(x => x.ProductDiscount > 0).OrderBy(x => x.ProductDiscount).Skip(SkipEntries)
                           .Take(showEntries).ToList()
                           .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
            if (ProductList.Any())
                ProductList.ElementAt(0).ProductsCounter = ProductList.Count();
            return ProductList;
        }
    }
}
