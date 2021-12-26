using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.SupperModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.DataModels
{
    public class OrderDetail : NTOrderDetail
    {
        [ForeignKey("OrderId")]
        public virtual Order Orders { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
    }
}