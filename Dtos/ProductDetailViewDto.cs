using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Dtos
{
    public class ProductDetailViewDto
    {
        public ProductDetailsDto ProductsDetailsDto { get; set; }
        public IEnumerable<ProductFeedbackDto> ProductsFeedbackListDto { get; set; }
        public IEnumerable<CustomerReviewDto> CustomersReviewListDto { get; set; }
        public ProductDetailViewDto()
        {
            ProductsFeedbackListDto = new List<ProductFeedbackDto>();
            CustomersReviewListDto = new List<CustomerReviewDto>();
        }
    }
}
