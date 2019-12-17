using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data.Models;
using WebShop.ViewModels.Product;
namespace WebShop.ViewModel.Product
{
    public class PageIndexModel
    {
        public PageViewModal PageViewModel { get; set; }
        public IEnumerable<WebShop.Data.Models.Product> ProductList { get; set; }
    }
}
