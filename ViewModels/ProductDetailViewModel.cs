using AngularCoreEmarketing.Models;
using System.Collections.Generic;

namespace AngularCoreEmarketing.ViewModels
{
    public class ProductDetailViewModel
    {
        public ProductDetails ProductsDetails { get; set; }
        public List<ProductFeedback> ProductFeedBackList { get; set; }
        public List<CustomerReview> CustomersReviewsList { get; set; }

    }
}