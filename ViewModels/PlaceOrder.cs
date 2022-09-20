using AngularCoreEmarketing.Models;
using System.Collections.Generic;

namespace AngularCoreEmarketing.ViewModels
{
    public class PlaceOrder
    {
        public DeliveryAddress Address { get; set; }
        public List<Cart> Cart { get; set; }

        public readonly int FreeDeliveryLimit;
        
        public PlaceOrder()
        {
            FreeDeliveryLimit = 50;
            Cart = new List<Cart>();
        }
    }
}