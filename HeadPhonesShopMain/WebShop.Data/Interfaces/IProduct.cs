using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Data.Models;

namespace WebShop.Data.Interfaces
{
    public interface IProduct
    {
        Product GetByID(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAllFiltered(string searchQuery);

        Task Add(Product product);
        Task Delete(int id);
    }
}
