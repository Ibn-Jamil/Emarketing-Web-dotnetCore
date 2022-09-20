using AutoMapper;
using AngularCoreEmarketing.Dtos;
using AngularCoreEmarketing.Models;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using AngularCoreEmarketing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AngularCoreEmarketing.Controllers.Api
{

    [ApiController]
    [Route("Api/[controller]")]
    public class HomeApiController : ControllerBase
    {
       private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeApiController(IWebHostEnvironment webHostEnvironment, IMapper mapper, ApplicationDbContext context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;

        }
       [HttpGet]
        
        public List<ProductDetailsDto> List(int SortByPrice, int SortByCatagory, int CatagoryGenre, int PageIndex)
        {         
            int counter = 0, showEntries = 32, SkipEntries = (PageIndex - 1) * 32;
            var SpecificCatagory = new List<CatagorySpecific>();
            SpecificCatagory.Clear();
            var ProductList = new List<ProductDetailsDto>();
            ProductList.Clear();
            ////////////////////////////////////////////////////--------extracting Category------------/////////////////////////////////////////////////////////               
            #region
            if (SortByCatagory > 0)
            {
                if (CatagoryGenre == 1)
                {
                    var SubCatagory = _context.CatagoriesMajor.Where(x => x.Id == SortByCatagory)
                        .Include(v=>v.MajorSubCatagory).SelectMany(v=>v.MajorSubCatagory.SelectMany(b=>b.SpecificCatagory)).ToList();
                   
                        SpecificCatagory.AddRange(SubCatagory);
                    
                }
                if (CatagoryGenre == 2)
                {
                    var temp = _context.CatagoriesMajorSub.Where(x => x.Id == SortByCatagory)
                        .Include(m=>m.SpecificCatagory).SelectMany(v=>v.SpecificCatagory).ToList();
                    SpecificCatagory.AddRange(temp);
                }

                if (CatagoryGenre == 3)
                {
                    var temp = _context.CatagoriesSpecific.Where(x => x.Id == SortByCatagory).ToList();
                    SpecificCatagory.AddRange(temp);
                }
                #endregion
                ////////////////////////////////////////////////////-------Sorting--------------/////////////////////////////////////////////////////////////////////
                #region
                if (SortByPrice == 0)
                {

                    foreach (var item in SpecificCatagory)
                    {
                        counter += _context.ProductsDetails.Where(x => x.CatagoriesSpecificId == item.Id).Count();
                    }
                    foreach (var item in SpecificCatagory)
                    {
                        var tempList = _context.ProductsDetails.Where(x => x.CatagoriesSpecificId == item.Id)
                                        .OrderBy(x => x.Id).Skip(SkipEntries).Take(showEntries).ToList()
                                        .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
                        ProductList.AddRange(tempList);

                    }
                    if (ProductList.Any())
                        ProductList.ElementAt(0).ProductsCounter = counter;
                    return ProductList;
                }
                else if (SortByPrice == 1)
                {
                    foreach (var item in SpecificCatagory)
                    {
                        var tempList = _context.ProductsDetails.Where(x => x.CatagoriesSpecificId == item.Id).OrderBy(x => x.ProductPrice)
                                      .Skip(SkipEntries).Take(showEntries).ToList()
                                      .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
                        ProductList.AddRange(tempList);
                    }
                    if (ProductList.Any())
                        ProductList.ElementAt(0).ProductsCounter = counter;
                    return ProductList;
                }
                else if (SortByPrice == 2)
                {
                    foreach (var item in SpecificCatagory)
                    {
                        var tempList = _context.ProductsDetails.Where(x => x.CatagoriesSpecificId == item.Id).OrderByDescending(x => x.ProductPrice)
                            .Skip(SkipEntries).Take(showEntries).ToList()
                           .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
                        ProductList.AddRange(tempList);
                    }
                    if (ProductList.Any())
                        ProductList.ElementAt(0).ProductsCounter = counter;
                    return ProductList;
                }
                else if (SortByPrice == 3)
                {
                    foreach (var item in SpecificCatagory)
                    {
                        var tempList = _context.ProductsDetails.Where(x => x.CatagoriesSpecificId == item.Id).OrderBy(x => x.ProductPostDate)
                            .Skip(SkipEntries).Take(showEntries).ToList()
                           .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
                        ProductList.AddRange(tempList);
                    }
                    if (ProductList.Any())
                        ProductList.ElementAt(0).ProductsCounter = counter;
                    return ProductList;
                }
                else
                {
                    foreach (var item in SpecificCatagory)
                    {
                        var tempList = _context.ProductsDetails.Where(x => x.CatagoriesSpecificId == item.Id).OrderByDescending(x => x.ProductPostDate)
                                      .Skip(SkipEntries).Take(showEntries).ToList()
                                      .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
                        ProductList.AddRange(tempList);
                    }
                    if (ProductList.Any())
                        ProductList.ElementAt(0).ProductsCounter = counter;
                    return ProductList;
                }
                #endregion
            }
            /////////////////////////////////-----------------without category--------------//////////////////////////////
            #region
            if (SortByCatagory == 0)
            {
                counter = _context.ProductsDetails.Count();
                if (SortByPrice == 0)
                {

                    ProductList = _context.ProductsDetails.OrderBy(x => x.Id).Skip(SkipEntries)
                        .Take(showEntries).ToList()
                        .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
                    if (ProductList.Any())
                        ProductList.ElementAt(0).ProductsCounter = counter;
                    return ProductList;

                }

                else if (SortByPrice == 1)
                {

                    ProductList = _context.ProductsDetails.OrderBy(x => x.ProductPrice)
                                     .Skip(SkipEntries).Take(showEntries).ToList()
                                     .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
                    if (ProductList.Any())
                        ProductList.ElementAt(0).ProductsCounter = counter;
                    return ProductList;

                }

                else if (SortByPrice == 2)
                {
                    ProductList = _context.ProductsDetails.OrderByDescending(x => x.ProductPrice)
                                           .Skip(SkipEntries).Take(showEntries).ToList()
                                           .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
                    if (ProductList.Any())
                        ProductList.ElementAt(0).ProductsCounter = counter;
                    return ProductList;

                }
                else if (SortByPrice == 3)
                {
                    ProductList = _context.ProductsDetails.OrderBy(x => x.ProductPostDate)
                                       .Skip(SkipEntries).Take(showEntries).ToList()
                                       .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
                    if (ProductList.Any())
                        ProductList.ElementAt(0).ProductsCounter = counter;
                    return ProductList;
                }

                else
                {
                    ProductList = _context.ProductsDetails.OrderByDescending(x => x.ProductPostDate)
                                        .Skip(SkipEntries).Take(showEntries).ToList()
                                        .Select(_mapper.Map<ProductDetails, ProductDetailsDto>).ToList();
                    if (ProductList.Any())
                        ProductList.ElementAt(0).ProductsCounter = counter;
                    return ProductList;
                }

            }
            #endregion
            return ProductList;
        }

        [HttpGet("{Id}")]
        public ProductDetailViewDto Details(int Id)
        {
            var model = new ProductDetailViewDto();
            var user = User.Identity.Name;
            var DetailEntry = _context.ProductsDetails.Include(x => x.ImageList).Include(c=>c.Sizes).SingleOrDefault(x => x.Id == Id);
            model.ProductsDetailsDto = _mapper.Map<ProductDetails, ProductDetailsDto>(DetailEntry);

            model.ProductsFeedbackListDto = _context.ProductsFeedback.Where(x => x.ProductDetailsId == Id)
                    .OrderByDescending(c => c.QuestionTime).Take(3).ToList()
                    .Select(_mapper.Map<ProductFeedback, ProductFeedbackDto>);

            model.CustomersReviewListDto = _context.CustomerReviews.Where(c => c.ProductDetailsId == Id).ToList().
                Select(_mapper.Map<CustomerReview, CustomerReviewDto>);
            return model;
        }
        [HttpPost]
        public void Update(ProductDetailsDto Model)
        {
            var Entry = _mapper.Map<ProductDetailsDto, ProductDetails>(Model);
            if (Model != null)
            {
                _context.Entry(Entry).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
        [HttpDelete("{Id}")]
        public async void Delete(int Id)
        {
            var product = await _context.ProductsDetails.SingleOrDefaultAsync(x => x.Id == Id);
            DeleteStringPic(product.ProductImage);
            var images = await _context.ImagesDirectories.Where(x => x.ProductDetailsId == Id).ToListAsync();
            foreach (var item in images)
            {
                DeleteStringPic(item.ImagePath);
            }
            var sizes = await _context.ProductsSizes.Where(x => x.ProductDetailsId == Id).ToListAsync();
            _context.ImagesDirectories.RemoveRange(images);
            _context.ProductsSizes.RemoveRange(sizes);
            _context.ProductsDetails.Remove(product);
            await _context.SaveChangesAsync();
        }
        public void DeleteStringPic(string Str)
        {
            if (Str != null)
            {
                var DbPath = Str.Split("\\");
                var OnlyName = DbPath.Last();
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath + "/Content/PrdouctImages", OnlyName);
                FileInfo de = new FileInfo(fullPath);
                if (de != null)
                    de.Delete();
            }
        }
    }
}
