using AutoMapper;
using AngularCoreEmarketing.Data;
using AngularCoreEmarketing.Dtos;
using AngularCoreEmarketing.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace AngularCoreEmarketing.Controllers.Api
{
    [Route("Api/[controller]")]
    [ApiController]
    public class FilterProductApiController : ControllerBase
    {
        ApplicationDbContext _context;
        readonly IMapper _mapper;
        public FilterProductApiController(ApplicationDbContext Context, IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;
        }

        [HttpGet("{Category}/{ProductId}")]
        public IEnumerable<ProductDetailsDto> List(int? Category, int? ProductId)
        {
            if (Category != 0)
            {
                var TrendingProductWithCatagory = _context.ProductsDetails.Where(x => x.CatagoriesSpecificId == Category && x.Id != ProductId)
                    .OrderByDescending(c => c.Rating).Take(8).ToList().
                    Select(_mapper.Map<ProductDetails, ProductDetailsDto>);
                return TrendingProductWithCatagory;
            }
            else
            {
                var TrendingProduct = _context.ProductsDetails.OrderByDescending(c => c.Rating).Take(8).ToList().
                           Select(_mapper.Map<ProductDetails, ProductDetailsDto>);
                return TrendingProduct;
            }
        }
        [HttpGet("{'Query'}")]
        public IList<ProductDetailsDto> search(string Query)
        {
            var Cat_List = _context.ProductsDetails.Where(x => x.ProductName.Contains(Query)).ToList().
                Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
            if (Cat_List.Any())
                Cat_List.ElementAt(0).ProductsCounter = Cat_List.Count;
            return Cat_List;
        }
    }
}
