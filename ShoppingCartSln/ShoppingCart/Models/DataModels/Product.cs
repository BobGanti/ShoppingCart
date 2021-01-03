using ShoppingCart.Models.SupperModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.DataModels
{
    public class Product : NTProduct
    { 
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}