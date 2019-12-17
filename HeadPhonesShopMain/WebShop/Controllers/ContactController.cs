using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data.Interfaces;
using WebShop.Data.Models;
using WebShop.ViewModels.Product;

namespace WebShop.Controllers
{
    public class ContactController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
