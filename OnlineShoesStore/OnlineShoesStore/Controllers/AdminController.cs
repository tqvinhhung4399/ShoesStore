using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoesStore.Models;

namespace OnlineShoesStore.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult UploadImageAction()
        {
            return View("UploadImage");
        }

        private readonly string index = "~/Views/Home/Index.cshtml";

        [HttpPost("FileUpload")]
        public async Task<IActionResult> UploadImage(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName();
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            
            ViewBag.UploadSuccessful = Path.GetTempFileName();
            return View("UploadImage");
        }

        public IActionResult ProcessRemoveShoes()
        {
            string shoesIDStr = HttpContext.Request.Query["txtShoesID"];
            int id = Int32.Parse(shoesIDStr);
            if(new ShoesData().RemoveShoesByShoesID(id))
            {
                ViewBag.RemoveSuccessful = "Remove " + new ShoesData().GetShoesNameByShoesID(id) + " successfully";
            }
            else
            {
                ViewBag.RemoveFailed = "Remove " + new ShoesData().GetShoesNameByShoesID(id) + " failed";
            }
            ViewBag.ListShoes = new ShoesData().GetAllShoes();
            return View("ShoesManager");
        }

        public IActionResult ProcessRestoreShoes()
        {
            string shoesIDStr = HttpContext.Request.Query["txtShoesID"];
            int id = Int32.Parse(shoesIDStr);
            if (new ShoesData().RestoreShoesByShoesID(id))
            {
                ViewBag.RestoreSuccessful = "Restore " + new ShoesData().GetShoesNameByShoesID(id) + " successfully";
            }
            else
            {
                ViewBag.RestoreFailed = "Restore " + new ShoesData().GetShoesNameByShoesID(id) + " failed";
            }
            ViewBag.ListShoes = new ShoesData().GetAllShoes();
            return View("ShoesManager");
        }

        public IActionResult AddProduct()
        {
            ViewBag.ListCategories = new CategoryData().GetCategories();
            ViewBag.ListBrands = new BrandData().GetBrands();
            ViewBag.ListOrigins = new OriginData().GetOrigins();
            return View();
        }

        public IActionResult ProcessAddNewShoes()
        {
            string name = Request.Form["txtName"];
            int categoryID = int.Parse(Request.Form["slCategory"]);
            int brandID = int.Parse(Request.Form["slBrand"]);
            string material = Request.Form["txtMaterial"];
            string description = Request.Form["txtDescription"];
            int originID = int.Parse(Request.Form["slOrigin"]);
            string[] colors = Request.Form["txtColor"];
            string[] prices = Request.Form["txtPrice"];
            if (new ShoesData().InsertShoes(new ShoesDTO { Name = name, CategoryId = categoryID, BrandId = brandID, Material = material, Description = description, OriginId = originID }))
            {
                int shoesID = new ShoesData().GetNewestShoesId();
                List<ProductDTO> listProducts = new List<ProductDTO>();
                for (int i = 0; i < colors.Length; i++)
                {
                    listProducts.Add(new ProductDTO { ShoesId = shoesID, Color = colors[i], Price = double.Parse(prices[i]), IsDeleted = false });
                }
                if (new ProductData().InsertProducts(listProducts))
                {
                    ViewBag.AddSuccess = "Add new product successfully!";
                }
            }
            ViewBag.AddFailed = "Add new product failed!";
            return RedirectToAction("ShoesManager");

            //if (new ShoesData().AddNewShoes(shoes))
            //{
            //    ViewBag.Success = "Add new shoes successfully";
            //    return View(); //view tuong ung sau khi AddNewShoes
            //}
            //else
            //{
            //    ViewBag.Failed = "Add new shoes failed!";
            //    return View(); //view tuong ung khi add that bai
            //}
        }

        //H
        public IActionResult ShoesManager()
        {
            ViewBag.ListShoes = new ShoesData().GetAllShoes();
            return View();
        }

        //H
        public IActionResult EditShoes()
        {
            string shoesIDStr = HttpContext.Request.Query["txtShoesID"];
            int shoesID = Int32.Parse(shoesIDStr);
            ViewBag.Shoes = new ShoesData().GetShoesInformationByShoesID(shoesID);
            ViewBag.ListCategories = new CategoryData().GetCategories();
            ViewBag.ListBrands = new BrandData().GetBrands();
            ViewBag.ListOrigins = new OriginData().GetOrigins();
            ViewBag.Products = new ProductData().GetProductsByShoesID(shoesID);
            return View();
        }


        public IActionResult ProductManager()
        {
            if (CheckAdmin() != null)
            {
                return View(index);
            }
            return View();
        }

        //H
        public IActionResult LoadDataTable()
        {
            DataTableData data = new DataTableData();

            var result = data.GetData();
            return Json(result);
        }

        public IActionResult UserManager()
        {
            if (CheckAdmin() != null)
            {
                return View(index);
            }
            ViewBag.ListUser = new UserData().LoadUsers();
            return View(CheckAdmin());
        }

        public IActionResult UnbanUser(string username)
        {
            UserData data = new UserData();
            if (CheckAdmin() != null)
            {
                return View(index);
            }
            if (data.UnbanUserByUsername(username))
            {
                ViewBag.UnbanSuccessful = "User " + username + " has been unbanned!";
                ViewBag.ListUser = data.LoadUsers();
            }
            else
            {
                ViewBag.UnbanFailed = "Unban user " + username + " failed!";
            }
            return View("UserManager");
        }

        public IActionResult BanUser(string username)
        {
            UserData data = new UserData();
            if (CheckAdmin() != null)
            { //role khong phai admin
                return View(index);
            }
            if (new UserData().BanUserByUsername(username))
            {
                ViewBag.BanSuccessful = "User " + username + " has been banned!";
                ViewBag.ListUser = data.LoadUsers();
            }
            else
            {
                ViewBag.BanFailed = "Ban user " + username + " failed!";
            }
            return View("UserManager");
        }


        private string CheckAdmin()
        {
            if (HttpContext.Session.GetString("SessionRole") == null) //chưa đăng nhập
            {
                return index;
            }
            else if (!HttpContext.Session.GetString("SessionRole").Equals("admin")) //role khác admin
            {
                return index;
            }
            return null; //role là admin
        }

        //Quản lí sản phảm
        public IActionResult EditProduct()
        {
            string shoesIDStr = HttpContext.Request.Query["txtProductID"];
            int productID = Int32.Parse(shoesIDStr);
            ViewBag.ShoesForPageEditProduct = new ShoesData().GetShoesDetailByProductID(productID);
            ViewBag.ListProductDetails = new ProductDetailData().GetProductDetailsByProductID(productID);
            return View("EditProduct");
        }
    }
}