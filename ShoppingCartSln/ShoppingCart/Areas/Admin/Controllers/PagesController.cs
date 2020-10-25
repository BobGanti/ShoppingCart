using ShoppingCart.Models.Contexts;
using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.ViewModels.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        private readonly CartDb db;

        public PagesController()
        {
            db = new CartDb();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageVM> models = db.Pages.ToArray()
                                 .OrderBy(p => p.Sorting)
                                 .Select(p => new PageVM(p)).ToList();
            return View(models);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPage(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Page page = new Page();

            page.Title = model.Title;

            //check for slink and format it
            model.Slink = FormatSlink(model.Slink, model.Title);

            //check that title and slink are unique
            if (db.Pages.Any(p => p.Title.Equals(model.Title)) || db.Pages.Any(p => p.Slink.Equals(model.Slink)))
            {
                ModelState.AddModelError("", "That title or slink already exists!");
                return View(model);
            }

            page.Slink = model.Slink;
            page.HasSidebar = model.HasSidebar;
            page.Sorting = 100;
            page.Body = model.Body;
            
            db.Pages.Add((page));
            db.SaveChanges();

            TempData["msg"] = "The " + model.Title + " page has been added successfully!";

            return RedirectToAction("AddPage");

        }

        // GET: Admin/Pages/EditPage/1
        [HttpGet]
        public ActionResult EditPage(int? id)
        {
            if (id == null)
            {
                return Content("Bad Request");
            }

            Page page = db.Pages.Find(id);

            if (page == null)
            {
                return HttpNotFound("The page does not exist");
            }

            PageVM model = new PageVM(page);

            return View(model);
        }

        // POST: admin/editpage/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPage(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //declare slug
            string slug = model.Slink;

            Page page = db.Pages.SingleOrDefault(p => p.Id == model.Id);

            page.Title = model.Title;

            //check for slink and format it
            if (model.Slink != "home")
            {
                model.Slink = FormatSlink(model.Slink, model.Title);
            }

            //check that title and slink are unique
            if (db.Pages.Where(p => p.Id != model.Id).Any(p => p.Title == model.Title) ||
                db.Pages.Where(p => p.Id != model.Id).Any(p => p.Slink == model.Slink))
            {
                ModelState.AddModelError("", "That title or slink already exists.");
                return View(model);
            }

            page.Slink = model.Slink;
            page.Body = model.Body;
            page.HasSidebar = model.HasSidebar;

            db.SaveChanges();

            TempData["msg"] = "The " + model.Title + " page has been updated successfully!";

            return RedirectToAction("EditPage");
        }

        // GET: admin/pagedetails/1
        public ActionResult PageDetails(int? id)
        {
            if (id == null)
            {
                return Content("Bad Request");
            }

            Page page = db.Pages.SingleOrDefault(p => p.Id == id);

            if (page == null)
            {
                return HttpNotFound("The page does not exist!");
            }

            PageVM model = new PageVM(page);

            return View(model);
        }

        // GET: Admin/Pages/DeletePage/1
        public ActionResult DeletePage(int? id)
        {
            if (id == null)
            {
                return Content("Bad Request");
            }

            Page page = db.Pages.Single(p => p.Id == id);

            if (page == null)
            {
                return HttpNotFound("The page does not exist!");
            }

            db.Pages.Remove(page);
            db.SaveChanges();

            TempData["msg"] = "The " + page.Title + " page was deleted successfully!";

            return RedirectToAction("Index");
        }

        static string FormatSlink(string slink, string title)
        {
            //check for slink and format it
            if (string.IsNullOrWhiteSpace(slink))
            {
                slink = title.Replace(" ", "-").ToLower();
            }
            else
            {
                slink = slink.Replace(" ", "-").ToLower();
            }
            return slink;
        }
    }
}