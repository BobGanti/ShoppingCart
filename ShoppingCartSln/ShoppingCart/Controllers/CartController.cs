using ShoppingCart.Models.Contexts;
using ShoppingCart.Models.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly LocalDb db;

        public CartController()
        {
            db = new LocalDb();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CartPartial()
        {
            CartVM cartModel = new CartVM();

            // Calculate total and save in ViewBag
            int qty = 0;
            decimal price = 0m;

            if (Session["cart"] != null)
            {
                // init the cart list
                var cart = (List<CartVM>)Session["cart"];

                foreach (var item in cart)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;
                }

                string lbl = (cartModel.Quantity > 1) ? "items" : "item";
                ViewBag.CartPartial = $"{cartModel.Quantity} {lbl} - {cartModel.Price}";
            }
            else
            {
                cartModel.Quantity = 0;
                cartModel.Price = 0m;
                ViewBag.CartPartial = "Empty!";
            }
           
            return PartialView(cartModel);
        }
    }
}