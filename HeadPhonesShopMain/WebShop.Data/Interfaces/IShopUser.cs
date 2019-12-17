using System.Collections.Generic;
using WebShop.Data.Models;

namespace WebShop.Data.Interfaces
{
    public interface IShopUser
    {
        ShopUser GetById(string id);
        IEnumerable<ShopUser> GetAll();
    }
}
