using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.SupperModels;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ShoppingCart.Models.ViewModels.Pages
{
    public class PageVM : NTPage
    {
        public PageVM()
        {}

        public PageVM(Page page)
        {
            Id = page.Id;
            Title = page.Title;
            Slink = page.Slink;
            Sorting = page.Sorting;
            HasSidebar = page.HasSidebar;
            Body = page.Body;
        }

        [Required]
        [AllowHtml]
        [StringLength(int.MaxValue)]
        public string Body { get; set; }
    }
}