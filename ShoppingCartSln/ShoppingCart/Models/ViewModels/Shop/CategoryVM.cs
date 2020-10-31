using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.SupperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.ViewModels.Shop
{
    public class CategoryVM : NTCategory
    {
        public CategoryVM()
        { }

        public CategoryVM(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Slink = category.Slink;
            Sorting = category.Sorting;
        }
    }
}