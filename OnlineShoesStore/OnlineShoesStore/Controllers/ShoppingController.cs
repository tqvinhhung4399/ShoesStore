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
            //if (!IsAuthorizedUser())
            //{
            //    return View(index);
            //}
            //if (new CartItemData().CheckValidCartItems(new CartItemData().GetCartItemsByCartID(new CartData().GetCartIDByUsername(HttpContext.Session.GetString("SessionUser"))))){
            //    return View();
            //} else
            //{
            //    return RedirectToAction("Cart");
            //}
            if (!IsAuthorizedUser())
            {
                return View(index);
            }
            bool check = true;
            List<CartItemDTO> list = new List<CartItemDTO>();
            string[] productDetailID = Request.Form["txtProductDetailID"];
            string[] quantity = Request.Form["txtQuantity"];
            int cartID = new CartData().GetCartIDByUsername(HttpContext.Session.GetString("SessionUser"));
            for (int i = 0; i < productDetailID.Length; i++)
            {
                int pdID = int.Parse(productDetailID[i]);
                int qtt = int.Parse(quantity[i]);
                list.Add(new CartItemDTO { CartId = cartID, Quantity = qtt, ProductDetailId = pdID });
            }
            if (check = new CartItemData().CheckValidCartItems(list))
            {
                ViewBag.UserInfo = new UserData().GetUserInfoByUserID(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Cart = new CartItemData().GetCartItemsByCartID(cartID);
                return View();
            }
            else
            {
                ViewBag.Announcement = "Update failed";
            }

            ViewBag.Cart = new CartItemData().GetCartItemsByCartID(cartID);
            return View("Cart");
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

            int quantity = int.Parse(Request.Form["txtQuantity"]);
            int productDetailID = int.Parse(Request.Form["slProductDetail"]);
            
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
            if (!IsAuthorizedUser())
            {
                return View(index);
            }
            if (new CartItemData().CheckValidCartItems(new CartItemData().GetCartItemsByCartID(new CartData().GetCartIDByUsername(HttpContext.Session.GetString("SessionUser")))))
            {
                string paymentMethod = Request.Form["txtPaymentMethod"];
                int cartID = new CartData().GetCartIDByUsername(HttpContext.Session.GetString("SessionUser"));
                float total = float.Parse(Request.Form["txtTotal"]);
                DateTime date = new DateTime();
                string status = "pending";
                if (new OrderData().InsertNewOrder(new OrderDTO { cartID = cartID, Total = total, DateCreated = date, Status = status, PaymentMethod = paymentMethod}))
                {
                    new CartData().CheckOutCartByCartID(cartID);
                    ViewBag.Cart = new CartItemData().GetCartItemsByCartID(cartID);
                    new ProductDetailData().UpdateAvailableProductDetailQuantity(new CartItemData().GetCartItemsByCartID(cartID));
                    return View();
                } else
                {
                    ViewBag.Announcement = "Something went wrong. Your cart items are no longer valid";
                    ViewBag.Cart = new CartItemData().GetCartItemsByCartID(cartID);
                    return View("Cart");
                }
            }
            else
            {
                ViewBag.Announcement = "Something went wrong. Your cart items are no longer valid";
                int cartID = new CartData().GetCartIDByUsername(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Cart = new CartItemData().GetCartItemsByCartID(cartID);
                return View("Cart");
            }
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

        public IActionResult ProcessUpdateCart()
        {
            if (!IsAuthorizedUser())
            {
                return View(index);
            }
            bool check = true;
            List<CartItemDTO> list = new List<CartItemDTO>();
            string[] productDetailID = Request.Form["txtProductDetailID"];
            string[] quantity = Request.Form["txtQuantity"];
            int cartID = new CartData().GetCartIDByUsername(HttpContext.Session.GetString("SessionUser"));
            for (int i = 0; i < productDetailID.Length; i++)
            {
                int pdID = int.Parse(productDetailID[i]);
                int qtt = int.Parse(quantity[i]);
                list.Add(new CartItemDTO { CartId = cartID, Quantity = qtt, ProductDetailId = pdID });
            }
            if(check = new CartItemData().CheckValidCartItems(list))
            {
                ViewBag.Announcement = "Update Successfully";
            }
            else
            {
                ViewBag.Announcement = "Update failed";
            }
            ViewBag.Cart = new CartItemData().GetCartItemsByCartID(cartID);
            return View("Cart");
        }
    }
}