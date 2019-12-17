using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data.Interfaces;
using WebShop.Data.Models;

namespace Webshop.Service
{
    public class CartService : ICart
    {
        private readonly WebShopContext _context;
        private readonly IShopUser _userService;

        public CartService(WebShopContext context, IShopUser userService)
        {
            _context = context;
            _userService = userService;
        }

        public CartService(WebShopContext context)
        {
            _context = context;
        }

        public Cart GetByID(int id)
        {
            return _context.Carts.Where(cart => cart.CartId == id)
                .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .Include(cart => cart.ShopUser)
                .FirstOrDefault();
        }

        public IEnumerable<Cart> GetAll()
        {
            return _context.Carts
                .Include(cart => cart.CartItems).ThenInclude(cartItem => cartItem.Product)
                .Include(cart => cart.ShopUser);
        }

        public IEnumerable<CartItem> GetItems(string id)
        {
            return _context.CartItems.Where(cartItem => cartItem.Cart.ShopUser.Id == id);
        }

        public Cart GetByUserID(string id)
        {
            return _context.Carts.Where(cart => cart.ShopUser.Id == id)
                .Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                .SingleOrDefault();
        }

        public async Task Add(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task AddCartItem(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }

        public async Task DeleteCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task AddItemToCart(Product product, string id)
        {
            var user = _userService.GetById(id);

            var cart = GetByUserID(id);
            if (cart == null && user == null)
            {
                _context.Carts.Add(new Cart { ShopUser = user });
                _context.SaveChanges();
            }
            cart = GetByUserID(id);

            var shoppingCartItem = _context.CartItems.SingleOrDefault(ci => ci.Product.Id == product.Id && ci.Cart.CartId == cart.CartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new CartItem
                {
                    Product = product,
                    Amount = 1,
                    Cart = cart
                };

                await AddCartItem(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _context.SaveChanges();
        }

        public int DeleteItemFromCart(CartItem cartItem, string id)
        {
            var cart = GetByUserID(id);

            var shoppingCartItem = _context.CartItems.SingleOrDefault(ci => ci.CartItemId == cartItem.CartItemId && ci.Cart.CartId == cart.CartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _context.CartItems.Remove(shoppingCartItem);
                }
            }
            _context.SaveChanges();
            return localAmount;
        }

        public int DeleteItemFromCartAtAll(CartItem cartItem, string id)
        {
            var cart = GetByUserID(id);

            var shoppingCartItem = _context.CartItems.SingleOrDefault(ci => ci.CartItemId == cartItem.CartItemId && ci.Cart.CartId == cart.CartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                _context.CartItems.Remove(shoppingCartItem);
            }
            _context.SaveChanges();
            return localAmount;
        }

        public async Task Delete(int id)
        {
            var cart = GetByID(id);
            _context.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public void Clear(string id)
        {
            var cart = GetByUserID(id);
            _context.CartItems.RemoveRange(cart.CartItems);
            _context.SaveChanges();
        }

        public decimal GetTotal(string id)
        {
            var cart = GetByUserID(id);
            decimal total = 0.0M;
            foreach (var cartItem in cart.CartItems)
            {
                total += cartItem.Product.Price * cartItem.Amount;
            }
            return total;
        }
    }
}
