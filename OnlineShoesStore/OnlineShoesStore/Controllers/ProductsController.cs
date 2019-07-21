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
            ProductData data = new ProductData();
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
            List<ProductDTO> listProducts = new ProductData().SearchProductsByName(Request.Query["search"]);
            ViewBag.Products = listProducts;
            return View("Category");
        }

        public IActionResult ProductDetail()
        {
            string idStr = HttpContext.Request.Query["txtID"];
            int productID = Int32.Parse(idStr);
            ViewBag.Shoes = new ShoesData().GetShoesDetailByProductID(productID);
            ViewBag.ListProductDetails = new ProductDetailData().GetProductDetailsByProductID(productID);
            ViewBag.ListImages = new ProductImageData().GetImagesByProductID(productID);
            ViewBag.ListQuantities = new ProductDetailData().GetAvailableQuantityByProductDetailIDs(new ProductDetailData().GetProductDetailsByProductID(productID));
            return View("Product");
        }

        public IActionResult SearchByFilters()
        {
            string categoryID = Request.Form["txtCategory"];
            string brandID = Request.Form["txtBrand"];
            string price = Request.Form["txtPrice"];
            List<ProductDTO> listProducts = new ProductData().GetProductsByFilters(categoryID, brandID, price);
            ViewBag.Products = listProducts;
            return View("Category");
        }
    }
}