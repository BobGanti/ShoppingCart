using ShoppingCart.Models.SupperModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.DataModels
{
    public class User : NTUser
    {
        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "This field should be 8 min & 50 max characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}