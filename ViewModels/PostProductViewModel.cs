using AngularCoreEmarketing.Models;
using System.Collections.Generic;

namespace AngularCoreEmarketing.ViewModels
{
    public class PostProductViewModel
    {
        public ProductDetails ProductDetails { get; set; }
        public List<string> Images { get; set; }
        public PostProductViewModel()
        {
            Images = new List<string>();
        }
    }
}