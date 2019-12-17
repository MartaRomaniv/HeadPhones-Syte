using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShop.Data.Interfaces;
using WebShop.Data.Models;
using WebShop.ViewModels.Cart;

namespace WebShop.Controllers
{
    public class CartController : Controller
    {
        private readonly WebShopContext _context;
        private readonly IProduct _productService;
        private readonly ICartItem _cartItemService;
        private readonly ICart _cartService;
        private readonly UserManager<ShopUser> _userManager;

        public CartController(WebShopContext context, IProduct productService, ICartItem cartItemService, ICart cartService, UserManager<ShopUser> userManager)
        {
            _context = context;
            _productService = productService;
            _cartItemService = cartItemService;
            _cartService = cartService;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            var UserID = _userManager.GetUserId(User);
            IEnumerable<CartListingModel> cartItems = _cartService.GetByUserID(UserID).CartItems.Select(cartItem => new CartListingModel
            {
                CartItemId = cartItem.CartItemId,
                ProductId = cartItem.Product.Id,
                Name = cartItem.Product.Name,
                ImageURL = cartItem.Product.ImageURL,
                Price = cartItem.Product.Price,
                Amount = cartItem.Amount,
            });

            CartIndexModel model = new CartIndexModel
            {
                CartItemsList = cartItems,
                Total = _cartService.GetTotal(UserID)
            };

            return View(model);
        }

        [Authorize]
        //[HttpPost]
        public IActionResult AddToCart(int id)
        {
            var product = _productService.GetByID(id);
            var userId = _userManager.GetUserId(User);
            _cartService.AddItemToCart(product, userId);
            return RedirectToAction("Index");
        }

        //[HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cartItem = _cartItemService.GetByID(id);
            var UserID = _userManager.GetUserId(User);
            _cartService.DeleteItemFromCart(cartItem, UserID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCartAtAll(int id)
        {
            var cartItem = _cartItemService.GetByID(id);
            var UserID = _userManager.GetUserId(User);
            _cartService.DeleteItemFromCartAtAll(cartItem, UserID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveAllItems()
        {
            var UserID = _userManager.GetUserId(User);
            _cartService.Clear(UserID);
            return RedirectToAction("Index");
        }

        public decimal Total()
        {
            var userId = _userManager.GetUserId(User);
            return _cartService.GetTotal(userId);
        }
    }
}