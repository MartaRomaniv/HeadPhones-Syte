using System.Collections.Generic;

namespace WebShop.ViewModels.Cart
{
    public class CartIndexModel
    {
        public IEnumerable<CartListingModel> CartItemsList { get; set; }

        public decimal Total { get; set; }
    }
}
