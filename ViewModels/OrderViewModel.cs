using AngularCoreEmarketing.Data;
using AngularCoreEmarketing.Models;

namespace AngularCoreEmarketing.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public ProductDetails Product { get; set; }
        public string CustomerId{ get; set; }
        public ApplicationUser Customer { get; set; }
        public int Quantity { get; set; }
        public orderStatus OrderStatus{ get; set; }
    }
    public enum orderStatus
    {
        Pending=1,
        Confirmed,
        Unavialible,
        dilivered,
        reviewed

    }
}