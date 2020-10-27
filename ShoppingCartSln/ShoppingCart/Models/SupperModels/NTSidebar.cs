using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ShoppingCart.Models.SupperModels
{
    public abstract class NTSidebar
    {
        public int Id { get; set; }

        [StringLength(int.MaxValue)]
        [AllowHtml]
        public string Body { get; set; }
    }
}