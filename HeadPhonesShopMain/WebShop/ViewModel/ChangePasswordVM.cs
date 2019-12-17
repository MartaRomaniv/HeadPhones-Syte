using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MyWebProject2.ViewModel
{
    public class ChangePasswordVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
