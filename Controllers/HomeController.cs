using AngularCoreEmarketing.Data;
using AngularCoreEmarketing.Models;
using AngularCoreEmarketing.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Controllers
{
    public class HomeController :Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        public HomeController(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CatagoryMajor = new SelectList(await _context.CatagoriesMajor.ToListAsync(), "Id", "MajorName");
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserId(HttpContext.User);
                var cartProductIds = await _context.Carts.Where(c => c.CustomerId == user).Select(x => x.ProductId).ToListAsync();   
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartProductIds));
                int totalProduct =cartProductIds.Count();
                if (totalProduct == 0) HttpContext.Session.SetString("CartTotalProduct", "");
                else HttpContext.Session.SetString("CartTotalProduct", totalProduct.ToString());
                var Name = _userManager.GetUserName(HttpContext.User);
                HttpContext.Session.SetString("Name", Name);
                int TotalNotification = 0;
                if (User.IsInRole("Admin"))
                {
                    TotalNotification = await _context.OrderdProducts.Where(x=>x.OrderStatus == orderStatus.Pending).CountAsync();
                }
                else
                {
                    TotalNotification = await _context.OrderdProducts.Where(x => x.CustomerId == user && x.OrderStatus == orderStatus.dilivered).CountAsync();
                }
                    HttpContext.Session.SetString("DeliveryNotification", TotalNotification.ToString());
            }
            else
            {
                var jsonString = HttpContext.Session.GetString("Cart");
                string CartTotalCount="";
                if (jsonString != null)
                {
                    var SessionCartCsharp = JsonConvert.DeserializeObject<List<int>>(jsonString);
                    CartTotalCount = SessionCartCsharp.Count.ToString();
                }
               
               
                HttpContext.Session.SetString("CartTotalProduct", CartTotalCount);
            }
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserId(HttpContext.User);
                var sellerCheck = await _context.ProductsDetails.AnyAsync(x => x.ProductSellerId == user && x.Id == id);
                ViewBag.Seller = sellerCheck;
                ViewBag.Id = id;
                ViewBag.LoginUser = user;
            }
            return View();
        }


        //public void Download(string ProductImage)
        //{
        //    string path = Server.MapPath("~/Content/PrdouctImages");
        //    string fullPath = Path.Combine(path, ProductImage);
        //    download(fullPath);
        //}
        private void dowload(string fullPath)
        {

        }

        public async Task<JsonResult> AddToCart(int ProductId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserId(HttpContext.User);
                if (!(await _context.Carts.AnyAsync(x => x.CustomerId == user && x.ProductId == ProductId)))
                {
                    var CartItem = new Cart();
                    CartItem.CustomerId = user;
                    CartItem.ProductId = ProductId;
                    _context.Carts.Add(CartItem);
                    await _context.SaveChangesAsync();
                    var CustomerOrders = await _context.Carts.Where(x => x.CustomerId == user).CountAsync();
                    HttpContext.Session.SetString("CartTotalProduct", JsonConvert.SerializeObject(CustomerOrders));
                    return Json("Added");
                }
                return Json(null);
            }
            else
            {
                var sessionCartJson = HttpContext.Session.GetString("Cart");
                var ListOfProductId = new List<int>();
                if (HttpContext.Session.Get("Cart") == null)
                {
                    ListOfProductId.Add(ProductId);
                    HttpContext.Session.SetString("Cart",JsonConvert.SerializeObject(ListOfProductId));
                    HttpContext.Session.SetString("CartTotalProduct", ListOfProductId.Count.ToString());
                    return Json("Added");
                }

                else
                {
                    ListOfProductId = JsonConvert.DeserializeObject<List<int>>(sessionCartJson);
                    if (ListOfProductId.Contains(ProductId))
                    {
                        return Json("AlreadyExist");
                    }

                    ListOfProductId.Add(ProductId);
                    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(ListOfProductId));
                    HttpContext.Session.SetString("CartTotalProduct", ListOfProductId.Count.ToString());
                    return Json("Added");
                }
            }
        }
        public async Task<IActionResult> CartList()
        {
            var orderList = new List<int>();
            var list = new List<CartViewModel>();
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserId(HttpContext.User);
                orderList = await _context.Carts.Where(x => x.CustomerId==user).Select(b=>b.ProductId).ToListAsync();
            }
            else
            {
                var sessionCartJson = HttpContext.Session.GetString("Cart");
                orderList = JsonConvert.DeserializeObject<List<int>>(sessionCartJson);
            }
            
            foreach (var item in orderList)
            {
                var CartViewItem = new CartViewModel();
                var product = await _context.ProductsDetails.SingleOrDefaultAsync(x => x.Id == item);
                CartViewItem.Name = product.ProductName;
                CartViewItem.Description = product.ProductDescription;
                CartViewItem.Id = item;
                CartViewItem.Image = product.ProductImage;
                CartViewItem.PorductId = product.Id;
                list.Add(CartViewItem);

            }
            if (!list.Any())
                return RedirectToAction("Index");
            return View(list);
        }
        public async Task<IActionResult> RemoveFromeCart(int Id)
        {
            
           
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserId(HttpContext.User);
                var item = await _context.Carts.SingleOrDefaultAsync(x => x.ProductId == Id &&x.CustomerId==user);
                if (item != null)
                {
                    _context.Carts.Remove(item);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("CartTotalProduct", _context.Carts.Where(v=>v.CustomerId==user).Count().ToString());

                }
            }
            else
            {
                var sessionCartJson = HttpContext.Session.GetString("Cart");
                string CartTotalProduct = "";
                var SessionCartCsharp = new List<int>();
                if (sessionCartJson != null)
                {
                    SessionCartCsharp = JsonConvert.DeserializeObject<List<int>>(sessionCartJson);
                    CartTotalProduct = (SessionCartCsharp.Count - 1).ToString();
                }

                HttpContext.Session.SetString("CartTotalProduct", CartTotalProduct);
                SessionCartCsharp.Remove(Id);
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(SessionCartCsharp));
                if (!SessionCartCsharp.Any())
                    return RedirectToAction("Index");
            }
            
            return RedirectToAction("CartList");
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
}

