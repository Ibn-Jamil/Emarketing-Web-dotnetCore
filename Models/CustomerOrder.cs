using AngularCoreEmarketing.Data;
using AngularCoreEmarketing.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AngularCoreEmarketing.Models
{
    public class DeliveryAddress
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType("int")]
        [Display(Name = "Mobile/Phone")]
        public string ContactNumber { get; set; }
        [MaxLength(1024)]
        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        [Display(Name = "State/Province")]
        public string State { get; set; }
        public string Country { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Display(Name = "Send Gift")]
        public bool SendGift { get; set; }
        public DateTime TimeForForienKeyHelp { get; set; }
        [MaxLength(1024)]
        [Display(Name = "Special Request")]
        public string AdditionalRequests { get; set; }
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
    }

    public class OrderdProduct
    {
        public int Id { get; set; }
        public int Price { get; set; }
        [Display(Name = "Delivery Charges")]
        public int DeliveryCharges { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public string Image { get; set; }
        public string Disprition { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderTime { get; set; }
        [ForeignKey("product")]
        public int ProductId { get; set; }
        public ProductDetails Product { get; set; }
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public DeliveryAddress Address { get; set; }
        [Display(Name ="Order Status")]
        public orderStatus OrderStatus { get; set; }

    }
}