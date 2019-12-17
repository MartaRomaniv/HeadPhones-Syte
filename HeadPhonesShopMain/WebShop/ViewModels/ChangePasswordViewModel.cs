using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
