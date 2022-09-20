using AngularCoreEmarketing.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AngularCoreEmarketing.Models
{
    public class ProductDetails
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Delivery Charges")]
        public int DeliveryCharge { get; set; }
        public float Rating { get; set; }
        [Required]
        [Display(Name = "Price")]
        public int ProductPrice { get; set; }
        [Display(Name = "Parent Image")]
        public string ProductImage { get; set; }
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name = "Since")]
        public DateTime ProductPostDate { get; set; }
        [Display(Name = "Seller")]
        public string ProductSellerId { get; set; }

        public ApplicationUser ProductSeller { get; set; }
        [Required]
        [Display(Name = "Stock Available")]
        public int ProductStock { get; set; }
        [Display(Name = "Discount %")]
        public int ProductDiscount { get; set; }
        [Required]
        public int CatagoriesSpecificId { get; set; }
        public CatagorySpecific CatagoriesSpecific { get; set; }
        public virtual IList<ImagesDirectory> ImageList { get; set; }
        public virtual IList<ProductSizes> Sizes { get; set; }

    }
    public class ColorList
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public int ProductDetailsId { get; set; }


    }
    public class ImagesDirectory
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProductDetailsId { get; set; }


    }
    public class ProductSizes
    {
        public int Id { get; set; }
        public string AllSizes { get; set; }
        public int Quantity { get; set; }
        public int ProductDetailsId { get; set; }


    }
}