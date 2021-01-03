using ShoppingCart.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.SupperModels
{
    public abstract class NTProduct
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Slink { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [StringLength(255)]
        public string CategoryName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string ImageName { get; set; }

    }
}