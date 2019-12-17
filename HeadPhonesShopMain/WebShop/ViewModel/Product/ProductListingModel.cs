using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.ViewModel.Product
{
    public class ProductListingModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
    }
}
