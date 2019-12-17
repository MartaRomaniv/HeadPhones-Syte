using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Data.Models;

namespace WebShop.Data.Interfaces
{
    public interface ICart
    {
        Cart GetByID(int id);
        IEnumerable<Cart> GetAll();

        //User ID
        IEnumerable<CartItem> GetItems(string id);

        Cart GetByUserID(string id);
        Task Add(Cart cart);
        Task AddCartItem(CartItem cartItem);
        Task DeleteCartItem(CartItem cartItem);
        Task AddItemToCart(Product product, string id);

        int DeleteItemFromCart(CartItem cartItem, string id);
        int DeleteItemFromCartAtAll(CartItem cartItem, string id);

        void Clear(string id);
        decimal GetTotal(string id);

        Task Delete(int id);
    }
}
