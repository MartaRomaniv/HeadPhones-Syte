using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebShop.Data.Interfaces;
using WebShop.Data.Models;

namespace Webshop.Service
{
    public class ShopUserService : IShopUser
    {
        private readonly WebShopContext _context;

        public ShopUserService(WebShopContext context)
        {
            _context = context;
        }

        public IEnumerable<ShopUser> GetAll()
        {
            return _context.ShopUsers;
        }

        public ShopUser GetById(string id)
        {
            return _context.ShopUsers.FirstOrDefault(user => user.Id == id);
        }
    }
}