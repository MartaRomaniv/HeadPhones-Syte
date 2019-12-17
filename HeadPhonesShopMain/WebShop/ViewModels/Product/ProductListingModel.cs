using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.ViewModels.Product
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
