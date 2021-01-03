using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.SupperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Models.ViewModels.Shop
{
    public class ProductVM : NTProduct
    {
        public ProductVM()
        { }

        public ProductVM(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Slink = product.Slink;
            Description = product.Description;
            Price = product.Price;
            CategoryName = product.CategoryName;
            CategoryId = product.CategoryId;
            ImageName = product.ImageName;
        }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}