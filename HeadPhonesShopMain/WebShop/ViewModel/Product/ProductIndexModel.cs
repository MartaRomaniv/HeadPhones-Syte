using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.ViewModels.Product;
namespace WebShop.ViewModel.Product
{
    public class ProductIndexModel
    {
        public PageViewModal PageViewModel { get; set; }
        public IEnumerable<ProductListingModel> ProductList { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
    }
}
