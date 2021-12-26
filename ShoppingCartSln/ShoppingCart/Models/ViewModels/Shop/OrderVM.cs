using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.SupperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.ViewModels.Shop
{
    public class OrderVM : NTOrder
    {
        public OrderVM()
        { }

        public OrderVM(Order order)
        {
            OrderId = order.OrderId;
            UserId = order.UserId;
            CreatedDate = order.CreatedDate;
        }
    }
}