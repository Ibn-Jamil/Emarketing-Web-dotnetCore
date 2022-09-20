using AngularCoreEmarketing.Data;
using AngularCoreEmarketing.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularCoreEmarketing.ViewModels;
using Newtonsoft.Json;
using AngularCoreEmarketing.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace AngularCoreEmarketing.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceOrderApi : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public PlaceOrderApi(UserManager<ApplicationUser> userManager,ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public List<CartDto> GetCartProduct()
        {
            var user = _userManager.GetUserId(HttpContext.User);
            var _session = HttpContext.Session.GetString("Cart");
            if (User.Identity.IsAuthenticated)
            {

                if (_session != null)
                {
                    var session = JsonConvert.DeserializeObject<List<int>>(_session);
                    foreach (var item in session)
                    {
                        if (! _context.Carts.Any(x => x.ProductId == item))
                        {
                            var CartItem = new Cart();
                            CartItem.ProductId = item;
                            CartItem.CustomerId = user;
                            _context.Carts.Add(CartItem);
                            _context.SaveChangesAsync();
                        }

                    }
                }
            }
           
           var Model = _context.Carts.Include(x => x.Product).Where(x => x.CustomerId == user)
                .Include(c => c.Product.Sizes).Select(_mapper.Map<Cart,CartDto>).ToList();
            _session = null;
            return Model;
        }
    }
}
