using ShoppingCart.Models.Contexts;
using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.ViewModels;
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
        private readonly NtiAppsContext db;
        private string currencyIcon;
        private int cartQuantity;

        public CartController()
        {
            db = new NtiAppsContext();
            currencyIcon = "€";
            cartQuantity = 0;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        [Authorize]
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            if(cart.Count == 0)
            {
                ViewBag.Msg = "Your Cart is Empty!";
                return View();
            }

            decimal grandTotal = 0m;
            
            foreach(var item in cart)
            {
                grandTotal += item.Total;
            }

            ViewBag.GrandTotal = $"{grandTotal}";
            ViewBag.CurrencyIcon = currencyIcon;

            return View(cart);
        }

        public ActionResult CartPartial()
        {
            //int qty = 0;
            //decimal price = 0m;
            CartVM cartModel = new CartVM();

            if (Session["cart"] != null)
            {
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    cartModel.Quantity += item.Quantity;
                    cartModel.Price += item.Quantity * item.Price;
                }
            }
            else
            {
                cartModel.Quantity = 0;
                cartModel.Price = 0m;
                ViewBag.CartPartialDisplay = "Empty!";
            }
            ViewBag.CartQuantity = cartModel.Quantity;
            ViewBag.CurrencyIcon = currencyIcon;

            return PartialView(cartModel);
        }

        public ActionResult AddToCartPartial(int id)
        {
            /*
              * init CartVM list
              * init CartVM
              * get the product
              * product in cart ? increment : add New
              * get total qty and price and add to model
              * save cart back to session
              * return partical view with model
            */

            List<CartVM> cartList = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            var cartModel = new CartVM();
            var productInDb = db.Products.Find(id);

            var productInCart = cartList.FirstOrDefault(p => p.ProductId == id);          
            if(productInCart == null)
            {
                cartList.Add(new CartVM() 
                { 
                    ProductId = productInDb.Id,
                    ProductName = productInDb.Name,
                    Quantity = 1,
                    Price = productInDb.Price,
                    Image = productInDb.ImageName
                });
            }
            else
            {
                productInCart.Quantity++;
            }

            int qty = 0;
            decimal price = 0m;

            foreach (var item in cartList)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }

            cartModel.Quantity = qty;
            cartModel.Price = price;
            string txt = (cartModel.Quantity > 1) ? "items" : "item";
            ViewBag.CartQuantity = cartModel.Quantity;
            ViewBag.CurrencyIcon = currencyIcon;

            Session["cart"] = cartList;

            return PartialView(cartModel);
        }

        // GET: /Cart/IncrementProduct
        public JsonResult IncrementProduct(int productId)
        {
            List<CartVM> cartItems = Session["cart"] as List<CartVM>;
            CartVM productInCart = cartItems.FirstOrDefault(p => p.ProductId.Equals(productId));
            productInCart.Quantity++;

            foreach(var item in cartItems)
            {
                cartQuantity += item.Quantity;
            }

            var result = new {cartQty = cartQuantity, qty = productInCart.Quantity, price = productInCart.Price };
            ViewBag.CurrencyIcon = currencyIcon;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: /Cart/DecrementProduct
        public JsonResult DecrementProduct(int productId)
        {
            List<CartVM> cartItems = Session["cart"] as List<CartVM>;
            CartVM productInCart = cartItems.FirstOrDefault(p => p.ProductId == productId);
            if(productInCart.Quantity > 1)
            {
                productInCart.Quantity--;
            }
            else
            {
                productInCart.Quantity = 0;
                cartItems.Remove(productInCart);
            }

            var result = new { qty = productInCart.Quantity, price = productInCart.Price };
            ViewBag.CurrencyIcon = currencyIcon;

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // GET: /Cart/RemoveProduct
        public void RemoveProduct(int productId)
        {
            List<CartVM> cartItems = Session["cart"] as List<CartVM>;
            CartVM productInCart = cartItems.FirstOrDefault(p => p.ProductId == productId);
            cartItems.Remove(productInCart);
        }

        public ActionResult PaypalPartial()
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;
            return PartialView(cart);
        }

        // GET: Cart/PlaceOrder
        [HttpPost]
        public void PlaceOrder()
        {
            List<CartVM> cartIems = Session["cart"] as List<CartVM>;
            var userId = db.Users.SingleOrDefault(u => u.Username.Equals(User.Identity.Name)).Id;

            Order order = new Order()
            {
                UserId = userId,
                CreatedDate = DateTime.Now
            };

            db.Orders.Add(order);
            db.SaveChanges();

            OrderDetail orderDetail = new OrderDetail();

            foreach (var item in cartIems)
            {
                orderDetail.OrderId = order.OrderId;
                orderDetail.UserId = userId;
                orderDetail.ProductId = item.ProductId;
                orderDetail.Quantity = item.Quantity;

                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
            }

            // ToDo
            // Send Email To Admin
            // Send Email To Client


        }
    }
}