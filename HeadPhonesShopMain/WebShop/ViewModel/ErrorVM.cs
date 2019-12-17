using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebProject2.ViewModel
{
    public class ErrorVM
    {
        public string RequestId { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
