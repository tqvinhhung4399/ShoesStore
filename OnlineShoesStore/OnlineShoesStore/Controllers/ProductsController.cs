using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShoesStore.Models;

namespace OnlineShoesStore.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Category()
        {
            string idStr = HttpContext.Request.Query["txtCategory"];
            CategoryData data = new CategoryData();
            if (idStr.Equals("all"))
            {
                ViewBag.Products = data.GetAllProducts();
            }
            else
            {
                int id = Int32.Parse(idStr);
                ViewBag.Products = data.GetProductsByCategoryID(id);
            }
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }

        public IActionResult Search()
        {
            string search = HttpContext.Request.Query["search"]; //lay gia tri can search: Products/Search/search=yezzy
            ViewBag.ListSearch = new ShoesData().FindByName(search);
            return View();
        }

        public IActionResult ProductDetail()
        {
            int shoesID = Int32.Parse(HttpContext.Request.Query["id"]);
            ViewBag.Shoes = new ShoesData().ViewShoesDetailByShoesID(shoesID);
            //ViewBag.Products;
            //ViewBag.ProductDetails;
            //ViewBag.ProductImages;

            return View();
        }
    }
}