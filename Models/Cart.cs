using AngularCoreEmarketing.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularCoreEmarketing.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        public int ProductId { get; set; }
        public ProductDetails Product { get; set; }
        public string Size { get; set; }
        public int? Quantity { get; set; }
        public bool CartChecked { get; set; }
        public Cart()
        {
            CartChecked = false;
        }

    }
}