using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Models.SupperModels
{
    public abstract class NTPage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Slink { get; set; }

        public int Sorting { get; set; }

        public bool HasSidebar { get; set; }
    }
}