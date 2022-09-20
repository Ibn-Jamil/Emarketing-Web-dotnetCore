using AngularCoreEmarketing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Dtos
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int DeliveryCharge { get; set; }
        public int ProductPrice { get; set; }
        public float Rating { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public DateTime ProductPostDate { get; set; }
        public int ProductStock { get; set; }
        public int ProductDiscount { get; set; }
        public int ProductsCounter { get; set; }
        public int CatagoriesSpecificId { get; set; }
        public virtual IList<ImagesDirectoryDto> ImageList { get; set; }
        public virtual IList<productSizesDto> Sizes { get; set; }
        public string ProductSellerId { get; set; }
        public ApplicationUser ProductSeller { get; set; }

    }
    public class ImagesDirectoryDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProductDetailsId { get; set; }
    }
    public class productSizesDto
    {
        public int Id { get; set; }
        public string AllSizes { get; set; }
        public int Quantity { get; set; }
        public int ProductDetailsId { get; set; }

    }
}
