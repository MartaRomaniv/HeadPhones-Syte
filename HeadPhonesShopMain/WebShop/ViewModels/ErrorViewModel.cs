using System;

namespace WebShop.Models
{
  public class ErrorViewModel
  {
    public string RequestId { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
  }
}