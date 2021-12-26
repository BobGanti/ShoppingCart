using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Areas.Admin.Models.ViewModels
{
    public class AdminOrdersVM
    {
        public int OrderNumber { get; set; }
        public string Username { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQty { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}