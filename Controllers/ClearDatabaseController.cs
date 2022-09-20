using AngularCoreEmarketing.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Controllers
{
    public class ClearDatabaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClearDatabaseController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.RefreshProductReview = null;
            ViewBag.ClearAndRefresh = null;
            ViewBag.RefreshQueriesTable = null;
            ViewBag.RefreshReviewTable = null;
            ViewBag.RefreshCartTable = null;
            ViewBag.RefreshProductSizesTable = null;
            ViewBag.RefreshIamgesDirectoriesTable = null;
            return View();
        
        }
        public async Task<IActionResult> ClearAndRefresh()
        {
            var FolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Content/PrdouctImages");
            var AllImages=Directory.GetFiles(FolderPath);
            foreach (var item in AllImages)
            {
                var pictureName=item.Split(@"\").Last();
                bool CheckExistance = await _context.ImagesDirectories.AnyAsync(c => c.ImagePath.EndsWith(pictureName));
                if (!CheckExistance)
                {
                    FileInfo de = new FileInfo(item);
                    if (de != null)
                        de.Delete();
                }
            }
            return RedirectToAction("RefreshProductDetailStock");
        }
        public async Task<IActionResult> RefreshProductDetailStock()
        {

            var Products = await _context.ProductsDetails.ToListAsync();
            foreach (var item in Products)
            {
                var ProductReviews = await _context.CustomerReviews.Where(x=>x.ProductDetailsId==item.Id).Select(b=>b.ratingStars).ToListAsync();
                
                if (ProductReviews.Any())
                {
                    item.Rating=(float)ProductReviews.Average();
                    _context.Entry(item).State=EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }


            return RedirectToAction("RefreshQueriesTable");

        }
        public async Task<IActionResult> RefreshQueriesTable()
        {

            var CustomerQueries = await _context.ProductsFeedback.Select(v => v.ProductDetailsId).Distinct().ToListAsync();
            foreach (var item in CustomerQueries)
            {
                var deletedProduct = await _context.ProductsDetails.FirstOrDefaultAsync(x => x.Id == item);
                if (deletedProduct == null)
                {
                    var ReviewRemove = await _context.ProductsFeedback.Where(c => c.ProductDetailsId == item).ToListAsync();
                    _context.RemoveRange(ReviewRemove);
                    await _context.SaveChangesAsync();
                }
            }
           
           
            return RedirectToAction("RefreshReviewTable");

        }

        public async Task<IActionResult> RefreshReviewTable()
        {
            var CustomerReviews = await _context.CustomerReviews.Select(v => v.ProductDetailsId).Distinct().ToListAsync();
            foreach (var item in CustomerReviews)
            {
                var deletedProduct = await _context.ProductsDetails.FirstOrDefaultAsync(x => x.Id == item);
                if (deletedProduct == null)
                {
                    var ItemsList = await _context.CustomerReviews.Where(c => c.ProductDetailsId == item).ToListAsync();
                    _context.RemoveRange(ItemsList);
                    await _context.SaveChangesAsync();
                }
            }
            
            return RedirectToAction("RefreshCartTable");

        }

        public async Task<IActionResult> RefreshCartTable()
        {
            var CartProduct = await _context.Carts.Select(v => v.ProductId).Distinct().ToListAsync();
            foreach (var item in CartProduct)
            {
                var deletedProduct = await _context.ProductsDetails.FirstOrDefaultAsync(x => x.Id == item);
                if (deletedProduct == null)
                {
                    var ItemsList = await _context.Carts.Where(c => c.ProductId == item).ToListAsync();
                    _context.RemoveRange(ItemsList);
                    await _context.SaveChangesAsync();
                }
            }
           
            return RedirectToAction("RemoveDeletedCatagoryProduct");

        }
        public async Task<IActionResult> RemoveDeletedCatagoryProduct()
        {
            var deletedCatagoryProducts=await _context.ProductsDetails.Select(c=>c.CatagoriesSpecificId).ToListAsync();

            foreach (var item in deletedCatagoryProducts)
            {
                var cat = await _context.CatagoriesSpecific.FirstAsync(c => c.Id == item);
                    if(cat==null)
                RedirectToRoute("/HomeApi/Delete/" + item);
            }
            return RedirectToAction("RefreshProductSizesTable");

        }

        public async Task<IActionResult> RefreshProductSizesTable()
        {
            var CustomerReviews = await _context.ProductsSizes.Select(v => v.ProductDetailsId).Distinct().ToListAsync();
            foreach (var item in CustomerReviews)
            {
                var deletedProduct = await _context.ProductsDetails.FirstOrDefaultAsync(x => x.Id == item);
                if (deletedProduct == null)
                {
                    var ItemsList = await _context.ProductsSizes.Where(c => c.ProductDetailsId == item).ToListAsync();
                    _context.RemoveRange(ItemsList);
                    await _context.SaveChangesAsync();
                }
            }
           
            return RedirectToAction("RefreshIamgesDirectoriesTable");
        }

        public async Task<IActionResult> RefreshIamgesDirectoriesTable()
        {
            var CustomerReviews = await _context.ImagesDirectories.Select(v => v.ProductDetailsId).Distinct().ToListAsync();
            foreach (var item in CustomerReviews)
            {
                var deletedProduct = await _context.ProductsDetails.FirstOrDefaultAsync(x => x.Id == item);
                if (deletedProduct == null)
                {
                    RemovPic(item);
                }
            }
            ViewBag.RefreshCartTable = "Customer Cart Table Cleaned Successfully";
            ViewBag.RefreshProductReview = "Product Reviews Matched Successfully";
            ViewBag.RefreshReviewTable = "Customer Reviews Table Cleaned Successfully";
            ViewBag.RefreshQueriesTable = "Customer Queries Table Cleaned Successfully";
            ViewBag.RefreshProductSizesTable = "Product Sizes Table Cleaned Successfully";
            ViewBag.ClearAndRefresh = "Unused inaccessible images Deleted from Directory";
            ViewBag.RefreshIamgesDirectoriesTable = "Image Directory Table Cleaned Successfully";
            return View("Index");
        }

        public async void RemovPic(int id)
        {
            var ImgList = await _context.ImagesDirectories.Where(x => x.ProductDetailsId == id).ToListAsync();
            foreach (var item in ImgList)
            {
                var DbPath = item.ImagePath.Split(@"\");
                var OnlyName = DbPath.Last();
                var contentPath = _webHostEnvironment.WebRootPath;
                var fullPath = Path.Combine(contentPath + "/Content/PrdouctImages", OnlyName);
                FileInfo de = new FileInfo(fullPath);
                if (de != null)
                    de.Delete();
            }
           
            _context.ImagesDirectories.RemoveRange(ImgList);
            await _context.SaveChangesAsync();
          
        }
    }
}
