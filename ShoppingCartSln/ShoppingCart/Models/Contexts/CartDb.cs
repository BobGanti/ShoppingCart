using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.Contexts
{
    public class CartDb : DbContext
    {
        public CartDb() : base("CartDb")
        { }


    }
}