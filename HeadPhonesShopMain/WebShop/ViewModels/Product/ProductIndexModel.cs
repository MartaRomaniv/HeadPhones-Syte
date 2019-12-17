using System.Collections.Generic;
using WebShop.ViewModel.Product;
namespace WebShop.ViewModels.Product
{
  public class ProductIndexModel
  {
        public PageViewModal PageViewModel { get; set; }
        public IEnumerable<ProductListingModel> ProductList { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
    }
}
