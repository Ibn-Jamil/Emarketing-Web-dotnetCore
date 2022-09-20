using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Dtos
{
    public class ProductFeedbackDto
    {
        public int Id { get; set; }
        public string Questions { get; set; }
        public string Answar { get; set; }
        public string UserName { get; set; }
        public DateTime QuestionTime { get; set; }
        public DateTime AnswarTime { get; set; }
        public int ProductDetailsId { get; set; }
        public ProductDetailsDto ProductsDetails { get; set; }
    }
}
