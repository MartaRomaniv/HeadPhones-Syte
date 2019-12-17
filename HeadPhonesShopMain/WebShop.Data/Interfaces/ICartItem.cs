using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Data.Models;

namespace WebShop.Data.Interfaces
{
    public interface ICartItem
    {
        CartItem GetByID(int id);
        IEnumerable<CartItem> GetAll();

        Task Add(CartItem cartItem);
        Task Delete(int id);
    }
}
