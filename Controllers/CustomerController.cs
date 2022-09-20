using AngularCoreEmarketing.Models;
using AngularCoreEmarketing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AngularCoreEmarketing.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AngularCoreEmarketing.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Customer
        [Authorize]
        public async Task<IActionResult> ReviewProduct(int Id)
        {
            var user = _userManager.GetUserId(HttpContext.User);
           
                var orderCheck = await _context.OrderdProducts
                    .AnyAsync(x => x.ProductId == Id && x.CustomerId == user && x.OrderStatus == orderStatus.dilivered);
            if (orderCheck)
                return View();
            else return RedirectToAction("Details", "Home", new { id = Id }) ;
        }
        [Authorize]
        public async Task<IActionResult> PurchasedList()
        {
            var user = _userManager.GetUserId(HttpContext.User);
            if (User.IsInRole("Admin"))
            {
                var ModelAdmin = await _context.OrderdProducts.Include(c => c.Product).Include(v => v.Address)
               .Where(x => x.OrderStatus == orderStatus.Pending).OrderByDescending(x => x.OrderTime)
               .ToListAsync();
                return View(ModelAdmin);
            }
            var Model = await _context.OrderdProducts.Include(n=>n.Customer).Include(c => c.Product).Include(v => v.Address)
                .Where(x => x.CustomerId == user &&x.OrderStatus!=orderStatus.reviewed).OrderByDescending(x => x.OrderTime)
                .ToListAsync();
            return View(Model);
        }
        public async Task<IActionResult> SubmitReview(ReviewViewModel Model)
        {
            if (Model.ReviewsDescription != string.Empty && Model.RatingStars != 0 && Model.ProductDetailsId != 0)
            {
                var user = _userManager.GetUserId(HttpContext.User);
                var userName = await _context.Users.Where(x => x.Id == user).Select(x => x.UserName).SingleOrDefaultAsync();
                var Entry = new CustomerReview();
                var ChangOrderStatus = new OrderdProduct();
                Entry.UserName = userName;
                Entry.ReviewsDescription = Model.ReviewsDescription;
                Entry.ratingStars = Model.RatingStars;
                Entry.ReviewTime = DateTime.Now;
                Entry.ResponseTime = DateTime.Now;
                Entry.CustomerReviewd = true;
                Entry.ProductDetailsId = Model.ProductDetailsId;
                Entry.SellerAnswered = false;
                _context.CustomerReviews.Add(Entry);
                ChangOrderStatus = await _context.OrderdProducts
                    .Where(x => x.ProductId == Model.ProductDetailsId && x.CustomerId == user && x.OrderStatus == orderStatus.dilivered)
                    .FirstOrDefaultAsync();
                if (ChangOrderStatus != null)
                {
                    ChangOrderStatus.OrderStatus = orderStatus.reviewed;
                    _context.Entry(ChangOrderStatus).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                   var AllReview= await _context.CustomerReviews.Where(x => x.ProductDetailsId == Model.ProductDetailsId).Select(c => c.ratingStars).ToListAsync();
                    var ProductAvergeReview =await _context.ProductsDetails.FirstOrDefaultAsync(v => v.Id == Model.ProductDetailsId);
                    ProductAvergeReview.Rating = (float)AllReview.Average();
                    _context.Entry(ProductAvergeReview).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Details", "Home",new { id=Model.ProductDetailsId});
        }
            public async Task<JsonResult> AskQuestion(string Question, int ProductId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.error("Something Went Wrong");
                return Json(null);
            }
            string user = _userManager.GetUserId(HttpContext.User);
            var userName = await _context.Users.Where(x => x.Id == user).Select(x => x.UserName).SingleOrDefaultAsync();
            var feedback = new ProductFeedback();
            feedback.UserName = userName;
            feedback.ProductDetailsId = ProductId;
            feedback.Questions = Question;
            feedback.QuestionTime = DateTime.Now;
            feedback.AnswarTime = DateTime.Now;

            _context.ProductsFeedback.Add(feedback);
            await _context.SaveChangesAsync();
            return Json(Question);
        }
      
        //public PartialViewResult ModalRender()
        //{

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var user = _userManager.GetUserId(HttpContext.User);
        //        var seller = _context.OrderdProducts.Where(x => x.CustomerId == user);
        //        return PartialView("ReviewProduct");
        //    }
        //    return null;
        //}
        public async Task<JsonResult> NotificationCounter()
        {
            var user =_userManager.GetUserId(HttpContext.User);
            var TotalNotification = await _context.OrderdProducts.Where(x => x.CustomerId == user && x.OrderStatus == orderStatus.dilivered).CountAsync();
            return Json(TotalNotification);
        }
        [Authorize]
        public async Task<IActionResult> PlaceOrder()
        {
            var user = _userManager.GetUserId(HttpContext.User);
            var _session = HttpContext.Session.GetString("Cart");
            ViewBag.countryList = new SelectList(Countries.CountryList, "Key", "Value");
            if (User.Identity.IsAuthenticated)
            {

                if (_session != null)
                {
                    var session = JsonConvert.DeserializeObject<List<int>>(_session);
                    foreach (var item in session)
                    {
                        if (!await _context.Carts.AnyAsync(x => x.ProductId == item))
                        {
                            var CartItem = new Cart();
                            CartItem.ProductId = item;
                            CartItem.CustomerId = user;
                            _context.Carts.Add(CartItem);
                            await _context.SaveChangesAsync();
                        }

                    }
                }
            }
            var Model = new PlaceOrder();
            Model.Cart = await _context.Carts.Include(x => x.Product).Where(x => x.CustomerId == user)
                .Include(c => c.Product.Sizes).ToListAsync();
            _session = null;
            ViewBag.UserId= _userManager.GetUserId(HttpContext.User);
            return View(Model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PlaceOrder Model)
        {
            var time = DateTime.Now;
            var user = _userManager.GetUserId(HttpContext.User);
            if (user != Model.Address.CustomerId)
                return null;
            Model.Address.TimeForForienKeyHelp = time;
            _context.DeliveryAddress.Add(Model.Address);
            await _context.SaveChangesAsync();
            var addressTableRefrence = await _context.DeliveryAddress.OrderByDescending(x => x.TimeForForienKeyHelp).FirstOrDefaultAsync(x => x.CustomerId == user);
            foreach (var item in Model.Cart)
            {
                var orders = new OrderdProduct();
                if (item.CartChecked == true)
                {
                    var Product = await _context.ProductsDetails.Include(c=>c.Sizes).SingleOrDefaultAsync(x => x.Id == item.ProductId);
                    var sizeRemove = new ProductSizes();
                    if (Product.Sizes.Any())
                    {
                        sizeRemove = await _context.ProductsSizes.SingleOrDefaultAsync(x => x.ProductDetailsId == Product.Id && x.AllSizes == item.Size);

                    }
  
                    orders.OrderTime = time;
                    orders.Image = Product.ProductImage;
                    orders.DeliveryCharges = Product.DeliveryCharge;
                    orders.Disprition = Product.ProductDescription;
                    orders.Price = Product.ProductPrice;
                    orders.Quantity = (int)item.Quantity;
                    orders.Size = item.Size;
                    orders.CustomerId = user;
                    orders.ProductId = Product.Id;
                    orders.AddressId = addressTableRefrence.Id;
                    orders.OrderStatus = orderStatus.Pending;
                    _context.OrderdProducts.Add(orders);
                    Product.ProductStock = Product.ProductStock - (int)item.Quantity;
                    _context.Entry(Product).State = EntityState.Modified;

                    if (sizeRemove.Id != 0)
                    {
                        sizeRemove.Quantity = sizeRemove.Quantity - orders.Quantity;
                        if (sizeRemove.Quantity == 0)
                            _context.ProductsSizes.Remove(sizeRemove);
                        _context.Entry(sizeRemove).State = EntityState.Modified;

                    }
                    var orignalDbEntry = await _context.Carts.FirstOrDefaultAsync(c => c.Id == item.Id);
                    _context.Carts.Remove(orignalDbEntry);
                    await _context.SaveChangesAsync();
                }


            }
            //sendSuccessfullOrderMail();
            return RedirectToAction("PurchasedList");
        }
        public async Task<IActionResult> CartCollection()
        {
            string user = null;
            var _session = HttpContext.Session.GetString("Cart");
            var Model = new PlaceOrder();
            if (User.Identity.IsAuthenticated)
            {
                user = _userManager.GetUserId(HttpContext.User);
               
                if (_session!= null)
                {
                    var carList = new List<Cart>();
                    var session =JsonConvert.DeserializeObject<List<int>>(_session);
                    foreach (var item in session)
                    {
                        if (!await _context.Carts.AnyAsync(x => x.ProductId == item))
                        {
                            var CartItem = new Cart();
                            CartItem.ProductId = item;
                            CartItem.CustomerId = user;
                            carList.Add(CartItem);
                        }

                    }
                    _context.Carts.AddRange(carList);
                    await _context.SaveChangesAsync();
                   int totalProduct = await _context.Carts.Where(x => x.CustomerId == user).CountAsync();
                    HttpContext.Session.SetString("CartTotalProduct", totalProduct.ToString());
                }
                Model.Cart = await _context.Carts.Include(x => x.Product).Where(x => x.CustomerId == user)
                                         .Include(c => c.Product.Sizes).ToListAsync();

                HttpContext.Session.SetString("Cart","");
            }
            else
            {
                if (_session != null)
                {
                    var carList = new List<Cart>();
                    var ProductList = new List<ProductDetails>();
                    var session = JsonConvert.DeserializeObject<List<int>>(_session);
                    int counter = 0;
                    foreach (var item in session)
                    {
                        var cartItem = new Cart();
                        var ProductTemp = await _context.ProductsDetails.Where(x => x.Id == item).SingleOrDefaultAsync();
                        ProductList.Add(ProductTemp);
                        cartItem.Id = counter+1;
                        cartItem.Product = ProductList[counter];
                        carList.Add(cartItem);
                        counter++;
                    }
                    Model.Cart = carList;
                    HttpContext.Session.SetString("CartTotalProduct", carList.Count.ToString());

                }
            }
            return View(Model);
        }
        public async Task<JsonResult> loadQuantity(int id, string Size)
        {
            var sizes = await _context.ProductsSizes.Where(x => x.ProductDetailsId == id && x.AllSizes == Size).FirstOrDefaultAsync();
            var data = sizes.Quantity;
            return Json(data);
        }
        private void sendSuccessfullOrderMail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("muhammadessa020@gamil.com");
                mail.To.Add(new MailAddress("k_essa98@yahoo.com"));
                mail.Subject = "From: Brand- Thanks!";
                mail.Body = "<p>Thank you For Purchase your order will be delivered with in 3-5 working Days</p>";
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
                ViewBag.ThankYouModel = "Thank You Your Mail Successfully Sent!";
            }
            catch (Exception exp)
            {
                throw new Exception("Unable to Send Mail To K_essa98yahoo.com", exp);
            }
        }
    }
}
