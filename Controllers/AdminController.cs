using AngularCoreEmarketing.Models;
using AngularCoreEmarketing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AngularCoreEmarketing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace AngularCoreEmarketing.Controllers
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManger;
        public AdminController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext Context,
            UserManager<ApplicationUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = Context;
            _userManger = userManager;
        }
        public IActionResult Index()
        {
            var NotifactionList = new List<int>();
            int Ordered = _context.OrderdProducts.Where(x => x.OrderStatus == orderStatus.Pending).Count();
            int products = _context.ProductsDetails.Count();
            int sale = _context.ProductsDetails.Where(x => x.ProductDiscount > 0).Count();
            int Reviewes = _context.CustomerReviews.Where(x => x.CustomerReviewd == false).Count();
            int Questions = _context.ProductsFeedback.Where(x => x.Answar == null).Count();
            int Deliver = _context.OrderdProducts.Where(x => x.OrderStatus == orderStatus.Confirmed).Count();


            NotifactionList.Add(products);
            NotifactionList.Add(Ordered);
            NotifactionList.Add(sale);
            NotifactionList.Add(products);
            NotifactionList.Add(Reviewes);
            NotifactionList.Add(Questions);
            NotifactionList.Add(Deliver);
            ViewBag.Notification = NotifactionList;
            return View();
        }
        
        public async Task<IActionResult> ProductList()
        {
            var model = await _context.ProductsDetails.Include(b=>b.ProductSeller).ToListAsync();
            return View(model);
        }
      
        public async Task<IActionResult> ProductListResponse()
        {
            var model = new List<ProductDetails>();
            var IdList = new List<int>();
            var Qlist = await _context.ProductsFeedback.Where(x => x.Answar == null)
                .OrderByDescending(x => x.QuestionTime).Select(x => x.ProductDetailsId).Distinct().ToListAsync();
            var Rlist = await _context.CustomerReviews.Where(x => x.ReviewResponse == null)
                .OrderByDescending(x => x.ReviewTime).Select(c => c.ProductDetailsId).Distinct().ToListAsync();
            IdList.AddRange(Qlist);
            foreach (var item in Rlist)
            {
                if (!IdList.Contains(item))
                    IdList.Add(item);
            }
            foreach (var item in IdList)
            {
                var temp = await _context.ProductsDetails.Include(u=>u.ProductSeller).SingleOrDefaultAsync(x => x.Id == item);
                if(temp!=null)
                model.Add(temp);
            }
            return View(model);
        }
        public async Task<IActionResult> ParentImageRemove(int id)
        {
            var product = await _context.ProductsDetails.SingleOrDefaultAsync(x => x.Id == id);
            DeleteStringPic(product.ProductImage);
            product.ProductImage = "No-Image-Exist";
           _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit",new {id});
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.ProductsDetails.SingleOrDefaultAsync(x => x.Id == id);
            DeleteStringPic(product.ProductImage);
            var images = await _context.ImagesDirectories.Where(x => x.ProductDetailsId == id).ToListAsync();
            foreach (var item in images)
            {
                DeleteStringPic(item.ImagePath);
            }
            var sizes = await _context.ProductsSizes.Where(x => x.ProductDetailsId == id).ToListAsync();
            _context.ImagesDirectories.RemoveRange(images);
            _context.ProductsSizes.RemoveRange(sizes);
            _context.ProductsDetails.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProductList");
        }
        public void DeleteStringPic(string str)
        {
            var DbPath = str.Split('/');
            var OnlyName = "/Content/" + DbPath.Last();
            var contentPath = _webHostEnvironment.ContentRootPath;
            var fullPath = Path.Combine(contentPath, OnlyName);
            FileInfo de = new FileInfo(fullPath);
            if (de.Exists)
                de.Delete();
        }
        public async Task<IActionResult> PostProduct()
        {
            var model = new PostProductViewModel();
            ViewBag.CatagoryMajor = new SelectList(await _context.CatagoriesMajor.ToListAsync(), "Id", "MajorName");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> PostProduct(PostProductViewModel model, List<IFormFile> ProductImage)
        {
            int productQuantity = 0;
            if (model.ProductDetails.Sizes != null)
            {
                foreach (var item in model.ProductDetails.Sizes)
                {
                    productQuantity += item.Quantity;
                }
            }
            if (productQuantity != model.ProductDetails.ProductStock && model.ProductDetails.Sizes != null)
            {
                ViewBag.ErrorInStock = "Please Ensure that the stock available must be equal sum of stock in each size";
                return View();
            }
            var time = DateTime.Now;
            var user = _userManger.GetUserId(HttpContext.User);
            model.ProductDetails.ProductPostDate = time;
            model.ProductDetails.ProductSellerId = user;
            if (model.ProductDetails.ProductName == string.Empty || model.ProductDetails.ProductStock == 0
                || model.ProductDetails.ProductSellerId == null || model.ProductDetails.CatagoriesSpecificId == 0)
            {
                ViewBag.Unsuccesfull = "Product is Not Uploaded";
                ViewBag.CatagoryMajor = new SelectList(await _context.CatagoriesMajor.ToListAsync(), "Id", "MajorName");
                return View(model);
            }


            var ImageList = new List<ImagesDirectory>();
            int ProductId = 0;
            for (int i = ProductImage.Count - 1; i >= 0; i--)
            {
                var SingleImage = new ImagesDirectory();
                string ImagePathString= UploadPictures(ProductImage[i]);
                SingleImage.ImagePath = ImagePathString;
                if (i == ProductImage.Count - 1)
                {
                    model.ProductDetails.ProductImage = ImagePathString;
                    _context.ProductsDetails.Add(model.ProductDetails);
                    await _context.SaveChangesAsync();
                    ProductId = await _context.ProductsDetails.Where(x => x.ProductSellerId == user).OrderByDescending(x => x.ProductPostDate).Select(x => x.Id).FirstOrDefaultAsync();
                }
                SingleImage.ProductDetailsId = ProductId;
                ImageList.Add(SingleImage);
            }
            _context.ImagesDirectories.AddRange(ImageList);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public string UploadPictures(IFormFile ImageFile)
        {
            string projectFolderPath = "Content/PrdouctImages";
            var contentPath = _webHostEnvironment.WebRootPath;
            string serverPathFolder = Path.Combine(contentPath, projectFolderPath);
            var str = Path.GetFileNameWithoutExtension(ImageFile.FileName);
            var Ext = Path.GetExtension(ImageFile.FileName);
            var time = DateTime.Now.ToString("hmmddMMyyyy");
            var newName = str + time + Ext;
            var fullpathName = Path.Combine(serverPathFolder, newName);
            using (Stream FileStream = new FileStream(fullpathName, FileMode.Create))
            {
                ImageFile.CopyTo(FileStream);
            }
            return projectFolderPath + @"\" + newName;

        }
        public async Task<IActionResult> Edit(int id)
        {
            var modelPlus = new EditViewModel();
            var model = await _context.ProductsDetails.Include(c => c.CatagoriesSpecific)
                .Include(c => c.Sizes).Include(m=>m.ImageList).FirstOrDefaultAsync(x => x.Id == id);
            var Sub = await _context.CatagoriesMajorSub.Where(n => n.SpecificCatagory.Contains(model.CatagoriesSpecific)).FirstOrDefaultAsync();       
            var Maj = await _context.CatagoriesMajor.Where(c=>c.MajorSubCatagory.Contains(Sub)).FirstOrDefaultAsync();
            ViewBag.CurrentMajorCategory = Maj.MajorName??null;
            ViewBag.CurrentChildCategory = Sub.MajorSubName??null; 
            ViewBag.CurrentGrandChildCategory= model.CatagoriesSpecific.CatagoriesName;
            ViewBag.CatagoryMajor = new SelectList(await _context.CatagoriesMajor.ToListAsync(), "Id", "MajorName");
            modelPlus.Prod = model;
            return View(modelPlus);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model, List<IFormFile> ProductImage)
        {
            int stockTotal = 0;
            if (model.Prod.Sizes!=null){
                foreach (var item in model.Prod.Sizes)
                {
                    stockTotal += item.Quantity;
                    if (item.Id > 0 && item.Quantity == 0)
                    {
                        var entry = await _context.ProductsSizes.SingleOrDefaultAsync(x => x.Id == item.Id);
                        _context.ProductsSizes.Remove(entry);
                    }
                    if (item.Id > 0 && item.Quantity > 0)
                    {
                        var entry = await _context.ProductsSizes.SingleOrDefaultAsync(x => x.Id == item.Id);
                        if (entry.Quantity != item.Quantity)
                        {
                            item.ProductDetailsId = model.Prod.Id;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                    }
                    if (item.Id == 0 && item.Quantity > 0)
                    {
                        item.ProductDetailsId = model.Prod.Id;
                        _context.ProductsSizes.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            model.Prod.ProductStock = stockTotal;
            _context.Entry(model.Prod).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            if (ProductImage != null)
            {
                for (int i = 0; i < ProductImage.Count; i++)
                {
                    var ImgTableEntry = new ImagesDirectory();
                    var dbEntry = UploadPictures(ProductImage[i]);
                    ImgTableEntry.ImagePath = dbEntry;
                    ImgTableEntry.ProductDetailsId = model.Prod.Id;
                    _context.ImagesDirectories.Add(ImgTableEntry);
                    if (i == ProductImage.Count - 1 && model.Prod.ProductImage == "No-Image-Exist")
                        model.Prod.ProductImage = dbEntry;
                    _context.Entry(model.Prod).State = EntityState.Modified;
                    await _context.SaveChangesAsync();


                }
            }

            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> RemovPic(int id, int rt)
        {

            var Img = await _context.ImagesDirectories.FirstOrDefaultAsync(x => x.Id == id);
            var DbPath = Img.ImagePath.Split(@"\");
            var OnlyName = DbPath.Last();
            var contentPath = _webHostEnvironment.WebRootPath;
            var fullPath = Path.Combine(contentPath + "/Content/PrdouctImages", OnlyName);
            FileInfo de = new FileInfo(fullPath);
            if (de != null)
                de.Delete();
            _context.ImagesDirectories.Remove(Img);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new {id=rt });
        }
        public IActionResult FlatSale()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FlatSale(int Percentage)
        {
            
            var allProduct = await _context.ProductsDetails.ToListAsync();
            foreach (var item in allProduct)
            {
                item.ProductDiscount = Percentage;
                _context.Entry(item).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> CreateCatagory()
        {
            ViewBag.MajorTableList = new SelectList(await _context.CatagoriesMajor.ToListAsync(), "Id", "MajorName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CatogryMajor(CatagoryListViewModel model)
        {
            var checkforDuplicate = await _context.CatagoriesMajor.SingleOrDefaultAsync(x => x.MajorName == model.CatagoriesMajor.MajorName);
            if (!ModelState.IsValid || checkforDuplicate != null)
            {
                ViewBag.MajorTableList = new SelectList(await _context.CatagoriesMajor.ToListAsync(), "Id", "MajorName");
                ViewBag.ErrorMajor = model.CatagoriesMajor.MajorName + " Already Exist";
                return View("CreateCatagory", model);
            }
            _context.CatagoriesMajor.Add(model.CatagoriesMajor);
            await _context.SaveChangesAsync();
            return RedirectToAction("CreateCatagory", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> CatagoriesMajorSub(CatagoryListViewModel model)
        {
            var checkforDuplicate = _context.CatagoriesMajor.Include(c => c.MajorSubCatagory).Where(b => b.Id == model.CatagoriesMajor.Id)
                .SelectMany(m => m.MajorSubCatagory.Where(n => n.MajorSubName.ToLower() == model.CatagoriesMajorSub.MajorSubName.ToLower())).ToList();
            if (checkforDuplicate.Any())
            {
                var parentCatagoryName = await _context.CatagoriesMajor.FindAsync(model.CatagoriesMajor.Id);
                ViewBag.ErrorMajorSub = parentCatagoryName.MajorName + "  -> " + model.CatagoriesMajorSub.MajorSubName + " : Already Exist";
                ViewBag.MajorTableList = new SelectList(await _context.CatagoriesMajor.ToListAsync(), "Id", "MajorName");
                return View("CreateCatagory", model);
            }
            if (model.CatagoriesMajor.Id != 0 || model.CatagoriesMajorSub.MajorSubName != null)
            {

                var parent = await _context.CatagoriesMajor.Include(l => l.MajorSubCatagory).FirstOrDefaultAsync(x => x.Id == model.CatagoriesMajor.Id);
                var db = _context.CatagoriesMajorSub.Add(model.CatagoriesMajorSub);
                await _context.SaveChangesAsync();
                var addToParentCatagory = await _context.CatagoriesMajorSub.FirstOrDefaultAsync(x => x.MajorSubName == model.CatagoriesMajorSub.MajorSubName);
                parent.MajorSubCatagory.Add(addToParentCatagory);
                await _context.SaveChangesAsync();
                return RedirectToAction("CreateCatagory", "Admin");
            }
            return View("CreateCatagory", model);
        }
        [HttpPost]
        public async Task<IActionResult> CatagoriesSpecific(CatagoryListViewModel model)
        {
            var search = _context.CatagoriesMajor.Include(b => b.MajorSubCatagory.Where(c => c.SpecificCatagory == model.CatagoriesSpecific))
                .ThenInclude(m => m.SpecificCatagory).SelectMany(x => x.MajorSubCatagory.SelectMany(c => c.SpecificCatagory)).ToList();

            if (search.Any())
            {
                foreach (var item in search)
                {
                    if (item.CatagoriesName == model.CatagoriesSpecific.CatagoriesName)
                    {
                        var major = _context.CatagoriesMajor.FirstOrDefault(x => x.Id == model.CatagoriesMajor.Id);
                        var majorSub = _context.CatagoriesMajorSub.FirstOrDefault(x => x.Id == model.CatagoriesMajorSub.Id);
                        ViewBag.ErrorMajorSpecific = major.MajorName + " ->  " + majorSub.MajorSubName + "  ->  " + item.CatagoriesName + " : Already Exist";
                        ViewBag.MajorTableList = new SelectList(await _context.CatagoriesMajor.ToListAsync(), "Id", "MajorName");
                        model.CatagoriesMajorSub = null;
                        return View("CreateCatagory", model);
                    }
                }
            }

            if (model.CatagoriesMajor.Id != 0 || model.CatagoriesMajorSub.Id != 0 || model.CatagoriesSpecific.CatagoriesName != null)
            {
                var parent = await _context.CatagoriesMajorSub.Include(c => c.SpecificCatagory)
                    .FirstOrDefaultAsync(x => x.Id == model.CatagoriesMajorSub.Id);
                _context.CatagoriesSpecific.Add(model.CatagoriesSpecific);
                await _context.SaveChangesAsync();

                var AddToParent = await _context.CatagoriesSpecific
                    .FirstOrDefaultAsync(x => x.CatagoriesName == model.CatagoriesSpecific.CatagoriesName);
                parent.SpecificCatagory.Add(model.CatagoriesSpecific);
                await _context.SaveChangesAsync();
                return RedirectToAction("CreateCatagory", "Admin");
            }
            return View("CreateCatagory", model);
        }

        public async Task<IActionResult> DeleteCatParent()
        {
            var model = await _context.CatagoriesMajor.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> DeleteParent(int id)
        {
            var Entry = await _context.CatagoriesMajor.Include(c => c.MajorSubCatagory)
            .ThenInclude(n => n.SpecificCatagory).SingleOrDefaultAsync(x => x.Id == id);
            foreach (var item in Entry.MajorSubCatagory)
            {
                if (item.SpecificCatagory.Any())
                {
                _context.CatagoriesSpecific.RemoveRange(item.SpecificCatagory);
                }
            }
            _context.CatagoriesMajorSub.RemoveRange(Entry.MajorSubCatagory);
            _context.CatagoriesMajor.Remove(Entry);

            await _context.SaveChangesAsync();
            return RedirectToAction("CreateCatagory");
        }
        public async Task<IActionResult> DeleteCatChild()
        {
            var model = await _context.CatagoriesMajor.Include(c => c.MajorSubCatagory).ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> DeleteChild(int Id)
        {
            var Entry = await _context.CatagoriesMajorSub.Include(s => s.SpecificCatagory).SingleOrDefaultAsync(x => x.Id == Id);
            if(Entry.SpecificCatagory.Any())
            _context.CatagoriesSpecific.RemoveRange(Entry.SpecificCatagory);
            _context.CatagoriesMajorSub.Remove(Entry);
            await _context.SaveChangesAsync();
            return RedirectToAction("CreateCatagory");
        }
        public async Task<IActionResult> DeleteCatGrandchild()
        {
            var model = await _context.CatagoriesMajor.Include(v => v.MajorSubCatagory)
                .ThenInclude(c => c.SpecificCatagory).ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> DeleteGrandchild(int id)
        {
            var Entry = await _context.CatagoriesSpecific.SingleOrDefaultAsync(x => x.Id == id);
            _context.CatagoriesSpecific.Remove(Entry);
            await _context.SaveChangesAsync();
            return RedirectToAction("CreateCatagory");
        }
        public async Task<IActionResult> ConfirmOrder(int Id)
        {
            var entry = await _context.OrderdProducts.SingleOrDefaultAsync(x => x.Id == Id);
            entry.OrderStatus = orderStatus.Confirmed;
            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //sendConfirmationMail()
            return RedirectToAction("PurchasedList", "Customer");
        }

        public async Task<IActionResult> DeliverOrders()
        {
            var user = _userManger.GetUserId(HttpContext.User);
            if (User.IsInRole("Admin"))
            {
                var ModelAdmin = await _context.OrderdProducts.Include(c => c.Product).Include(v => v.Address)
               .Where(x => x.OrderStatus == orderStatus.Confirmed).OrderByDescending(x => x.OrderTime)
               .ToListAsync();
                return View(ModelAdmin);
            }
            return null;
        }
        public async Task<IActionResult> PrintOrders(int Id)
        {
            var user = _userManger.GetUserId(HttpContext.User);
            if (User.IsInRole("Admin"))
            {
                var ModelAdmin = await _context.OrderdProducts.Include(c => c.Product).Include(v => v.Address)
               .Where(x => x.Id==Id).OrderByDescending(x => x.OrderTime)
               .ToListAsync();
                return View(ModelAdmin);
            }
            return null;
        }
        public async Task<IActionResult> DeliverOrdersSubmit(int Id)
        {
            var entry = await _context.OrderdProducts.SingleOrDefaultAsync(x => x.Id == Id);
            entry.OrderStatus = orderStatus.dilivered;
            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //sendConfirmationMail()
            return RedirectToAction("DeliverOrders");
        }

        private void sendConfirmationMail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("muhammadessa020@gamil.com");
                mail.To.Add(new MailAddress("k_essa98@yahoo.com"));
                mail.Subject = "From: Brand- Order Confirmation!";
                mail.Body = "<p>Your order is Succesfully Processed by Seller Thanks you. Stay with us for More Stunning Products</p>";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                var credentiol = new NetworkCredential()
                {
                    UserName = "muhammadessa020@gmail.com",
                    Password = "zmwesa123"
                };

                SmtpClient client = new SmtpClient();
                client.Credentials = credentiol;
                client.Host = "smtp.gmail.com";
                client.Port = 487;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                ViewBag.ThankYouModel = "Thank You Your Mail Successfuly Sent!";
            }
            catch (Exception exp)
            {
                throw new Exception("Unable to Send Mail To K_essa98yahoo.com", exp);
            }
        }

    }
}
