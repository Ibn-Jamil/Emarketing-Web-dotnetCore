using AngularCoreEmarketing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EditApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        public void ParentImageRemove(int id)
        {
            var product = _context.ProductsDetails.SingleOrDefault(x => x.Id == id);
          //  DeleteStringPic(product.ProductImage);
            product.ProductImage = "No-Image-Exist";
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
