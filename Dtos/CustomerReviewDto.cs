using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Dtos
{
    public class CustomerReviewDto
    {
        public int Id { get; set; }
        public int RatingStars { get; set; }
        public string UserName { get; set; }
        public string ReviewsDescription { get; set; }
        public string ReviewResponse { get; set; }
        public bool CustomerReviewd { get; set; }
        public bool SellerAnswered { get; set; }
        public DateTime ReviewTime { get; set; }
        public DateTime ResponseTime { get; set; }
        public int ProductDetailsId { get; set; }
        public ProductDetailsDto ProductsDetails { get; set; }
    }
}
