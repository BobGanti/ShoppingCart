using ShoppingCart.Models.Contexts;
using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.SupperModels;
using ShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        private readonly CartDb db;

        public ShopController()
        {
            db = new CartDb();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        // GET: Admin/Shop
        public ActionResult Categories()
        {
            var categories = db.Categories.ToArray()
                               .OrderBy(c => c.Sorting)
                               .Select(c => new CategoryVM(c))
                               .ToList();

            return View(categories);
        }

        // POST: Admin/Shop/AddCategory
        [HttpPost]
        public string AddCategory(string newcatName)
        {
            if (db.Categories.Any(c => c.Name.Equals(newcatName)))
                return "titletaken";

            Category category = new Category()
            {
                Name = newcatName,
                Slink = newcatName.Replace(" ", "-").ToLower(),
                Sorting = 100
            };

            db.Categories.Add(category);
            db.SaveChanges();

            return category.Id.ToString();
        }

        // POST: admin/shop/reordercategories/
        [HttpPost]
        public void ReorderCategory(int[] id)
        {
            int count = 1;
            foreach (var categoryId in id)
            {
                Category category = db.Categories.Find(categoryId);
                category.Sorting = count;

                db.SaveChanges();
                count++;
            }
        }

        // POST: admin/shop/RenameCategory/
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            if (db.Categories.Any(c => c.Name == newCatName))
                return "titletaken";

            var category = db.Categories.SingleOrDefault(c => c.Id == id);
            if (category == null)
                return "The Category does not exist!";

            category.Name = newCatName;
            category.Slink = newCatName.Replace(" ", "-").ToLower();

            db.SaveChanges();
            return "OK";
        }

        // GET: admin/shop/deletecategory/1
        public ActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Single(c => c.Id == id);

            db.Categories.Remove(category);
            db.SaveChanges();

            TempData["msg"] = "The " + category.Name + " Category was deleted successfully!";
            return RedirectToAction("Categories");
        }

    }
}