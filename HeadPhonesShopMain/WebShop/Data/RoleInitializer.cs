using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;
using Microsoft.AspNetCore.Identity;
using WebShop.Data.Models;

namespace WebShop.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<ShopUser> userManager, RoleManager<IdentityRole> roleManager, WebShopContext context)
        {
            string adminEmail = "admin@gmail.com";
            string adminName = "admin";
            string password = "_Aa123456";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("moderator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("moderator"));
            }

            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await roleManager.FindByNameAsync(adminEmail) == null)
            {
                ShopUser admin = new ShopUser { Email = adminEmail, UserName = adminName };
                context.Carts.Add(new Cart { ShopUser = admin });
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
