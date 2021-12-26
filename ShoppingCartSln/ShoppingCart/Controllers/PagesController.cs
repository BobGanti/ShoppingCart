


using ShoppingCart.Models.Contexts;
using ShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class PagesController : Controller
    {
        private readonly NtiAppsContext db;

        public PagesController()
        {
            db = new NtiAppsContext();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        // GET: Default
        public ActionResult Index(string pageSlink = "")
        {
            if (pageSlink == "")
            { pageSlink = "home"; }

            if(!db.Pages.Any(p => p.Slink.Equals(pageSlink)))
            {
                return RedirectToAction("Index", new { pege = "" });
            }

            var pageInDb = db.Pages.Where(p => p.Slink.Equals(pageSlink)).FirstOrDefault();
            ViewBag.PageTitle = pageInDb.Title;

            if(pageInDb.HasSidebar == true)
            {
                ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "No";
            }

            PageVM model = new PageVM(pageInDb);

            return View(model);
        }

        public ActionResult PagesMenuPartial()
        {
            List<PageVM> models = db.Pages.ToArray()
                                    .OrderBy(p => p.Sorting)
                                    .Where(p => p.Slink != "home")
                                    .Select(p => new PageVM(p))
                                    .ToList();

            return PartialView(models);
        }

        public ActionResult SidebarPartial()
        {
            var sidebar = db.Sidebars.Find(1);
            var sidebarModel = new SidebarVM(sidebar); 

            return PartialView(sidebarModel);
        }
    }
}