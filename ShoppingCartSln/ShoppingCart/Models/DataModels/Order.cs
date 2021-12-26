using ShoppingCart.Models.SupperModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.DataModels
{
    public class Order : NTOrder
    {
        [ForeignKey("UserId")]
        public virtual User Users { get; set; }
    }
}