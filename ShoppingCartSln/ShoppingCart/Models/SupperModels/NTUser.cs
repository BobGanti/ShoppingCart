using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.SupperModels
{
    public abstract class NTUser
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "This field should be 2 min & 50 max characters!")]
        public string Firstname { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "This field should be 2 min & 50 max characters!")]
        public string Lastname { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "This field should be 50 max characters!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "This field should be 50 max characters!")]
        public string Mobile { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "This field should be 6 min & 50 max characters!")]
        public string Username { get; set; }
    }
}