using System;
using System.ComponentModel.DataAnnotations;

namespace AngularCoreEmarketing.Models
{
    public class CustomerReview
    {
        public int Id { get; set; }
        [Required]
        public int ratingStars { get; set; }
        [Required]
        public string ReviewsDescription { get; set; }
        public string ReviewResponse { get; set; }
        [Required]
        public bool CustomerReviewd { get; set; }
        public bool SellerAnswered { get; set; }
        [Required]
        public DateTime ReviewTime { get; set; }
        [Required]
        public string UserName { get; set; }
        public DateTime ResponseTime { get; set; }
        [Required]
        public int ProductDetailsId { get; set; }
        public ProductDetails ProductsDetails { get; set; }
    }
}