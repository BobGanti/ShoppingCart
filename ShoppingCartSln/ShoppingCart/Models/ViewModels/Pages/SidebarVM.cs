using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.SupperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.ViewModels.Pages
{
    public class SidebarVM : NTSidebar
    {
        public SidebarVM()
        { }

        public SidebarVM(Sidebar sidebar)
        {
            Id = sidebar.Id;
            Body = sidebar.Body;
        }
    }

}