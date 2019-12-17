using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data.Interfaces;
using WebShop.Data.Models;

namespace Webshop.Service
{
    public class ProductService : IProduct
    {
        private readonly WebShopContext _context;
        private readonly IShopUser _userService;

        public ProductService(WebShopContext context, IShopUser userService)
        {
            _context = context;
            _userService = userService;
        }

        public Product GetByID(int id)
        {
            var product = _context.Products.Where(p => p.Id == id).FirstOrDefault();
            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products;
        }

        public IEnumerable<Product> GetAllFiltered(string searchQuery)
        {
            return
                string.IsNullOrEmpty(searchQuery)
                ? GetAll()
                : GetAll().Where(product => product.Name.ToLower().Contains(searchQuery.ToLower()) || product.Description.ToLower().Contains(searchQuery.ToLower()));
        }


        public async Task Add(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var product = GetByID(id);
            _context.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
