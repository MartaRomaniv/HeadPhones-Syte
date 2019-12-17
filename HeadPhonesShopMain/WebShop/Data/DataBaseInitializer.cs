using System.Linq;
using WebShop.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebShop.Data.Interfaces;
using System.Collections.Generic;
using WebShop.ViewModels.Product;
using WebShop.Controllers;
namespace WebShop.Data
{
    public class DataBaseInitializer:Controller
    {
        public static void Initialize(WebShopContext context)
        {
            context.Database.EnsureCreated();
            
            if (context.Products.Any())
            {
                return;
            }

            var products = new Product[]
            {
        new Product{ Name = "Item1", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item2", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item3", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item4", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item5", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item6", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item7", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item8", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item9", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item10", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item11", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item12", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item13", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item14", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item15", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item16", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item17", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item18", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item19", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},
        new Product{ Name = "Item20", Description = "This item will be perfecr choose for your look!", ImageURL = "https://i.imgur.com/ydxzeqI.png", Price = 245.55m},

            };
           

            foreach (Product product in products)
            {
                context.Add(product);
            }
            context.SaveChanges();
        }

    }

}
