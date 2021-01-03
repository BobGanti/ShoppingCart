using PagedList;
using ShoppingCart.Models.Contexts;
using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.SupperModels;
using ShoppingCart.Models.ViewModels.Shop;
using ShoppingCart.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ShoppingCart.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        private readonly LocalDb db;
        private readonly string ItemFolderName = "Products";

        public ShopController()
        {
            db = new LocalDb();
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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

        // GET: Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            var model = new ProductVM();
            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            return View(model);
        }

        // POST: Admin/Shop/AddProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error!", "Something is wrong.");
                return View(model);
            }

            if (db.Products.Any(p => p.Name == model.Name))
            {
                ModelState.AddModelError("Error!", "That product name is taken!");
                return View(model);
            }

            var product = new Product()
            {
                Name = model.Name,
                Slink = model.Name.Replace(" ", "-").ToLower(),
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                CategoryName = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId).Name,
            };

            db.Products.Add(product);
            db.SaveChanges();

            int id = product.Id;
            
            if (file != null && file.ContentLength > 0)
            {
                var upImageRootDirectories = new DirectoryInfo(String.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                string response = new FileDirectoryHandler(id, ItemFolderName, upImageRootDirectories, file).SetFileDir();
                if (response == "OK")
                {
                    product.ImageName = file.FileName;
                }
                else
                {
                    ModelState.AddModelError("", "" + response);
                }
            }

            TempData["msg"] = "The product has been added successfully!";
            ModelState.AddModelError("", "No file was loaded!");

            db.SaveChanges();

            return View(model);
        }

        // GET: admin/shop/Products/
        public ActionResult Products(int? page, int? catId)
        {
            var pageNumber = page ?? 1;
            List<ProductVM> products = db.Products.ToArray()
                                        .Where(p => catId == null || catId == 0 || p.CategoryId == catId)
                                        .Select(p => new ProductVM(p))
                                        .ToList();

            // Populating categories select list
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            // Set selected category
            ViewBag.SelectedCat = catId.ToString();

            // Set Pagination
            var onePageOfProducts = products.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(products);
        }

        // GET: admin/shop/EditProduct/id
        public ActionResult EditProduct(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return Content("That product does not exist!");

            var model = new ProductVM(product);
            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                           .Select(fn => Path.GetFileName(fn));

            return View(model);
        }

        // POST: admin/shop/EditProduct/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(ProductVM model, HttpPostedFileBase file)
        {
            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + model.Id + "/Gallery/Thumbs"))
                                           .Select(fn => Path.GetFileName(fn));

            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + model.Id + "/Gallery/Thumbs"))
                                               .Select(fn => Path.GetFileName(fn));

                ModelState.AddModelError("", "Something is wrong.");
                return View(model);
            }

            // Checking if new name exists in Db
            if(db.Products.Where(p => p.Id != model.Id).Any(p => p.Name == model.Name))
            {
                ModelState.AddModelError("", "That Title is taken!");
                return View(model);
            }

            var product = db.Products.Find(model.Id);
            product.Name = model.Name;
            product.Slink = model.Name.Replace(" ", "-").ToLower();
            product.Description = model.Description;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;
            product.CategoryName = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId).Name;

            #region upload new image
            if (file != null && file.ContentLength > 0)
            {
                var upImageRootDirectories = new DirectoryInfo(String.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                string response = new FileDirectoryHandler(product.Id, ItemFolderName, upImageRootDirectories, file).UpdateFilesInDir();

                if (response == "OK")
                {
                    product.ImageName = file.FileName;

                }
                else
                {
                    product.ImageName = db.Products.Find(1).ImageName;
                    ModelState.AddModelError("", "" + response);
                }
            }
            #endregion

            db.SaveChanges();

            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + model.Id + "/Gallery/Thumbs"))
                                           .Select(fn => Path.GetFileName(fn));

            TempData["msg"] = String.Format("The product {0} was edited successfully", product.Name);

            return View(model);
        }

        public ActionResult DeleteProduct(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            db.Products.Remove(product);
            db.SaveChanges();

            // Delete product image folders
            var upImageRootDirectories = new DirectoryInfo(String.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string mainImageFolder = Path.Combine(upImageRootDirectories.ToString(), "Products\\" + id.ToString());
            
            if (Directory.Exists(mainImageFolder))
                Directory.Delete(mainImageFolder, true);

            //bool res = new FileDirectoryHandler().DeleteFolder(mainImageFolder);

            //if (res)
            //{
            //    TempData["msg"] = $"The product, {product.Name} has been deleted successfully!";
            //}
            //else
            //{
            //    TempData["msg"] = "The product image was not found!";
            //}

            return RedirectToAction("Products");
        }

        //POST: Admin/Shop/SaveGalleryImages/id
        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            foreach(string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];

                if(file != null && file.ContentLength > 0)
                {
                    var upImageRootDirectories = new DirectoryInfo(String.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                    string response = new FileDirectoryHandler(id, ItemFolderName, upImageRootDirectories, file)
                                        .SaveGalleryImages();
                }
            }
        }

        public void DeleteGalleryImage(int id, string imageName)
        {
            //string galleryPath = Request.MapPath("~/Images/Uploads/products/"+id.ToString()+"/Gallery/"+imageName);
            //string galleryThumbsPath = Request.MapPath("~/Images/Uploads/products/" + id.ToString() + "/Gallery/Thumbs/" + imageName);
            
            string galleryFilesPath = Request.MapPath($"~/Images/Uploads/{ItemFolderName}/{id}/Gallery/{imageName}");
            string galleryThumbsFilesPath = Request.MapPath($"~/Images/Uploads/{ItemFolderName}/{id}/Gallery/Thumbs/{imageName}");

            new FileDirectoryHandler().DeleteGalleryFiles(galleryFilesPath, galleryThumbsFilesPath);
        }
    }
}