using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineShoesStore.Models;

namespace OnlineShoesStore.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly string index = "~/Views/Home/Index.cshtml";

        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult Cart()
        {
            if (!IsAuthorizedUser())
            {
                return View(index);
            }
            int cartID = new CartData().GetCartIDByUsername(HttpContext.Session.GetString("SessionUser"));
            ViewBag.Cart = new CartItemData().GetCartItemsByCartID(cartID);
            return View();
        }

        public IActionResult AddToCart()
        {
            if (!IsAuthorizedUser())
            {
                return View(index);
            }
            int cartID = new CartData().GetCartIDByUsername(HttpContext.Session.GetString("SessionUser"));
            int productDetailID = int.Parse(Request.Form["slProductDetail"]);
            int quantity = int.Parse(Request.Form["txtQuantity"]);
            CartItemData cid = new CartItemData();
            if(cid.AlreadyExistedInCart(cartID, productDetailID))
            {
                ViewBag.Announcement = "Product has already had in cart";
            }
            else
            {
                if (new CartItemData().InsertNewItemToCart(new CartItemDTO { CartId = cartID, ProductDetailId = productDetailID, Quantity = quantity }))
                {
                    ViewBag.Announcement = "Add to cart successfully";
                }
                else
                {
                    ViewBag.Announcement = "Add to cart failed";
                }
            }
            ViewBag.Cart = new CartItemData().GetCartItemsByCartID(cartID);
            return View("Cart");
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        public bool IsAuthorizedUser()
        {
            if(HttpContext.Session.GetString("SessionUser") != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}