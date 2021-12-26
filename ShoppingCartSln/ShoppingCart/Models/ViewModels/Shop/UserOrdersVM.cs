using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.SupperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.ViewModels.Shop
{
    public class UserOrdersVM : NTOrder
    {
        public UserOrdersVM()
        { }
        public UserOrdersVM(Order order)
        {
            OrderId = order.OrderId;
            UserId = order.UserId;
            CreatedDate = order.CreatedDate;
        }

        public decimal OrderAmount { get; set; }
        public Dictionary<string, int> ProductsAndQty { get; set; }

    }
}