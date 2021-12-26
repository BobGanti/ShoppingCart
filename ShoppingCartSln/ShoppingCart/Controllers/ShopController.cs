using ShoppingCart.Models.Contexts;
using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class ShopController : Controller
    {
        private readonly NtiAppsContext db;

        public ShopController()
        {
            db = new NtiAppsContext();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            var categoryList = db.Categories.ToArray()
                                 .OrderBy(c => c.Sorting)
                                 .Select(c => new CategoryVM(c)).ToList();

            return PartialView(categoryList);
        }

        // GET: /shop/category/name
        public ActionResult Category(string name)
        {
            Category category = db.Categories.FirstOrDefault(c => c.Slink == name);
            if (category == null)
                return HttpNotFound();
            
            var productModels = db.Products.ToArray()
                                    .Where(p => p.CategoryId == category.Id)
                                    .Select(p => new ProductVM(p))
                                    .ToList();

            // Retrieve CategoryName
            var productInCat = db.Products.Where(p => p.CategoryId == category.Id).FirstOrDefault();
            if (productInCat == null)
            {
                TempData["error"] = "Sorry! There are no prodcts in that Category";
                return RedirectToAction("Index", "Pages");
            }
            ViewBag.CategoryName = productInCat.CategoryName;          
            
            return View(productModels);
        }

        // GET: /shop/product-details/name
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            if (!db.Products.Any(p => p.Slink.Equals(name)))
                return RedirectToAction("Index", "Shop");

            var productInDb = db.Products.Where(p => p.Slink == name).FirstOrDefault();
            if(productInDb == null)
            {
                TempData["error"] = $"The Prodcuct {name} is not found";
                return RedirectToAction("Index", "Shop");
            }
            ProductVM model = new ProductVM(productInDb);
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + productInDb.Id + "/Gallery/Thumbs"))
                                          .Select(fn => Path.GetFileName(fn));


            return View("ProductDetails", model);
        }

    }
}