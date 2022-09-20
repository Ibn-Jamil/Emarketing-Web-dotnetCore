using AngularCoreEmarketing.Models;

namespace AngularCoreEmarketing.ViewModels
{
    public class ProductFeedbackViewModel
    {
        public int Rating { get; set; }
        public string Comments { get; set; }
        public string Questions { get; set; }
        public string Answar { get; set; }
        public ProductDetails ProductsDetails { get; set; }
    }
}