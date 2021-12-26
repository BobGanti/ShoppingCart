using ShoppingCart.Models.Contexts;
using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.ViewModels.Account;
using ShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShoppingCart.Controllers
{
    public class AccountController : Controller
    {
        private readonly NtiAppsContext db;

        public AccountController()
        {
            db = new NtiAppsContext();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        // GET: account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        // GET: account/login
        public ActionResult Login()
        {
            string username = User.Identity.Name;
            if (!String.IsNullOrEmpty(username))
                return RedirectToAction("user-profile");

            return View();
        }

        // POST: account/login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Login Error!");
                return View(model);
            }

            if(!db.Users.Any(u => u.Username.Equals(model.Username) && u.Password.Equals(model.Password)))
            {
                ModelState.AddModelError("", "Your Username and/or password is incorrect!. Try again");
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, model.RememberMe));

            }
        }

        [Authorize]
        // GET: account/logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return Redirect("~/account/login");
        }

        [Authorize]
        public ActionResult UserNavPartial()
        {
            string userrname = User.Identity.Name;
            var userInDb = db.Users.FirstOrDefault(u => u.Username == userrname);

            var model = new UserNavPartialVM()
            {
                Firstname = userInDb.Firstname,
                Lastname = userInDb.Lastname
            };

            return PartialView(model);
        }

        [Authorize]
        // GET: account/create-account
        [HttpGet]
        [ActionName("user-profile")]
        public ActionResult UserProfile()
        {
            string userrname = User.Identity.Name;
            var userInDb = db.Users.FirstOrDefault(u => u.Username == userrname);

            var model = new UserProfileVM(userInDb);

            return View("UserProfile", model);
        }

        [Authorize]
        // POST: account/create-account
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("user-profile")]
        public ActionResult UserProfile(UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error!");
                return View("UserProfile", model);
            }

            if (db.Users.Where(u => u.Id != model.Id).Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("", "That username already esixts!");
                model.Username = "";
                return View("UserProfile", model);
            }

            var userInDb = db.Users.Find(model.Id);
            if (!string.IsNullOrWhiteSpace(model.Password) && model.Password.Equals(model.ConfirmPassword))
            {
                userInDb.Password = model.Password;
            }
            else
            {
                ModelState.AddModelError("", "The passwords do not match!");
                return View("UserProfile", model);
            }
            
            userInDb.Firstname = model.Firstname;
            userInDb.Lastname = model.Lastname;
            userInDb.Email = model.Email;
            userInDb.Username = model.Username;
            userInDb.Mobile = model.Mobile;
           
            db.SaveChanges();

            TempData["msg"] = "You have succesfully edited your profile";

            return Redirect("~/account/user-profile");
        }

        // GET: account/create-account
        [HttpGet]
        [ActionName("create-account")]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        [Authorize]
        // POST: account/create-account

        [ValidateAntiForgeryToken]
        [ActionName("create-account")]
        public ActionResult CreateAccount(UserVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ensure all fields are filled!");
                return View("CreateAccount", model);
            }

            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "The passwords do not match!");
                return View("CreateAccount", model);
            }

            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "The passwords do not match!");
                return View("CreateAccount", model);
            }

            model.Username = model.Username.Replace(" ", "-").ToLower();

            if (db.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("", "That username exists already!");
                model.Username = "";
                return View("CreateAccount", model);
            }

            if(db.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("", "That email address exists alraady!");
                return View("CreateAccount", model);
            }

            User user = new User()
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                Username = model.Username,
                Password = model.Password,
                Mobile = model.Mobile
            };

            db.Users.Add(user);
            db.SaveChanges();

            int userId = user.Id;
            var userRole = new UserRole()
            {
                UserId = userId,
                RoleId = 2
            };

            db.UserRoles.Add(userRole);
            db.SaveChanges();

            TempData["msg"] = "Your registration was successful. You can login now";

            return Redirect("~/account/login");
        }

        [Authorize(Roles="user")]
        // GET: /Admin/Shop/Orders
        public ActionResult Orders()
        {
            List<UserOrdersVM> userOrders = new List<UserOrdersVM>();
            var userId = db.Users.Where(u => u.Username.Equals(User.Identity.Name)).SingleOrDefault().Id;

            List<OrderVM> orderModels = db.Orders
                                        .Where(o => o.UserId.Equals(userId)).ToArray()
                                        .Select(o => new OrderVM(o))
                                        .ToList();

            foreach (var orderModel in orderModels)
            {
                Dictionary<string, int> productAndQty = new Dictionary<string, int>();
                decimal totalAmmount = 0m;
                List<OrderDetail> orderDetailsList = db.OrderDetails
                                                        .Where(od => od.OrderId == orderModel.OrderId)
                                                        .ToList();

                foreach (var orderItem in orderDetailsList)
                {
                    var product = db.Products.Where(p => p.Id.Equals(orderItem.ProductId)).FirstOrDefault();
                    var price = product.Price;
                    var productName = product.Name;
                    productAndQty.Add(productName, orderItem.Quantity);

                    totalAmmount += orderItem.Quantity * price;
                }

                userOrders.Add(new UserOrdersVM()
                {
                    OrderId = orderModel.OrderId,
                    OrderAmount = totalAmmount,
                    ProductsAndQty = productAndQty,
                    CreatedDate = orderModel.CreatedDate
                });
            }

            return View(userOrders);
        }
    }
}