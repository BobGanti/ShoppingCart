using ShoppingCart.Models.SupperModels;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ShoppingCart.Models.DataModels
{
    public class Page : NTPage
    {
        [Required]
        [StringLength(int.MaxValue)]
        public string Body { get; set; }
    }
}