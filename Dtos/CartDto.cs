using AngularCoreEmarketing.Data;
using AngularCoreEmarketing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Dtos
{
    public class CartDto
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        public int ProductId { get; set; }
        public ProductDetailsDto ProductDto { get; set; }
        public string Size { get; set; }
        public int? Quantity { get; set; }
        public bool CartChecked { get; set; }
       

    }
}
