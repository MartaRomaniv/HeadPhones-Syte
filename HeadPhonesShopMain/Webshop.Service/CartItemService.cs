using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data.Interfaces;
using WebShop.Data.Models;

namespace Webshop.Service
{
    public class CartItemService : ICartItem
    {
        private readonly WebShopContext _context;

        public CartItemService(WebShopContext context)
        {
            _context = context;
        }

        public async Task Add(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var cartItem = GetByID(id);
            _context.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<CartItem> GetAll()
        {
            return _context.CartItems
                .Include(cartItem => cartItem.Product);
        }

        public CartItem GetByID(int id)
        {
            return _context.CartItems.Where(cartItem => cartItem.CartItemId == id).SingleOrDefault();
        }
    }
}
