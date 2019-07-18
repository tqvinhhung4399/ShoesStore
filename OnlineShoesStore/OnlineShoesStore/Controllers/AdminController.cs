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
        //H
        public IActionResult ProcessUpdateShoes()
        {
            bool check = true;
            int shoesID = int.Parse(Request.Form["txtShoesID"]);
            string name = Request.Form["txtName"];
            int categoryID = int.Parse(Request.Form["slCategory"]);
            int brandID = int.Parse(Request.Form["slBrand"]);
            string material = Request.Form["txtMaterial"];
            string description = Request.Form["txtDescription"];
            int originID = int.Parse(Request.Form["slOrigin"]);
            string[] colors = Request.Form["txtColor"];
            string[] prices = Request.Form["txtPrice"];
            string[] newColors = Request.Form["txtNewColor"];
            string[] newPrices = Request.Form["txtNewPrice"];
            if (check = new ShoesData().UpdateShoes(new ShoesDTO { ShoesId = shoesID, Name = name, CategoryId = categoryID, BrandId = brandID, Material = material, Description = description, OriginId = originID }))
            {
                List<ProductDTO> listProducts = new List<ProductDTO>();
                for (int i = 0; i < colors.Length; i++)
                {
                    listProducts.Add(new ProductDTO { ShoesId = shoesID, Color = colors[i], Price = double.Parse(prices[i]), IsDeleted = false });
                }
                //if(check = new ProductData().UpdateProductByNOTEHAM())
                //{
                //    if(newColors != null || newPrices != null)
                //    {
                //        List<ProductDTO> listNewProducts = new List<ProductDTO>();
                //        for (int i = 0; i < colors.Length; i++)
                //        {
                //            listNewProducts.Add(new ProductDTO { ShoesId = shoesID, Color = colors[i], Price = double.Parse(prices[i]), IsDeleted = false });
                //        }
                //        check = new ProductData().InsertProducts(listNewProducts);
                //    }
                //}
            }
            if (check)
            {
                ViewBag.Successful = "Update successfully";
            }
            else
            {
                ViewBag.Failed = "Update failed";
            }
            return View("ShoesManager");

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
        public IActionResult RestoreProduct()
        {
            ProductData data = new ProductData();
            int productID = Int32.Parse(HttpContext.Request.Query["txtProductID"]);
            if (data.RestoreProduct(productID))
            {
                ViewBag.Successful = "Restore " + data.GetProductNameByProductID(productID) + " (" + data.GetProductByProductID(productID).Color + ") successfully";
            }
            else
            {
                ViewBag.Failed = "Restore " + data.GetProductNameByProductID(productID) + " (" + data.GetProductByProductID(productID).Color + ") failed";
            }
            return View("ProductManager");
        }

        //H
        public IActionResult RemoveProduct()
        {
            ProductData data = new ProductData();
            int productID = Int32.Parse(HttpContext.Request.Query["txtProductID"]);
            if(data.RemoveProduct(productID))
            {
                ViewBag.Successful = "Remove " + data.GetProductNameByProductID(productID) + " (" + data.GetProductByProductID(productID).Color + ") successfully";
            }
            else
            {
                ViewBag.Failed = "Remove " + data.GetProductNameByProductID(productID) + " (" + data.GetProductByProductID(productID).Color + ") failed";
            }
            return View("ProductManager");
        }

        //H
        public IActionResult UpdateProduct()
        {
            bool check =false;
            string productIDStr = Request.Form["txtProductID"];
            int productID = Int32.Parse(productIDStr);
            string priceStr = Request.Form["txtProductPrice"];
            double price = Double.Parse(priceStr);
            string color = Request.Form["txtProductColor"];
            ProductDTO proDto = new ProductDTO
            {
                ProductId = productID,
                Color = color,
                Price = price
            };
            if(check = new ProductData().UpdateProductById(proDto))
            {
                string[] size = Request.Form["txtSize"];
                string[] quantity = Request.Form["txtQuantity"];
                List<ProductDetailDTO> list = new List<ProductDetailDTO>();
                ProductDetailDTO detailDto;
                for (int i = 0; i < size.Length; i++)
                {
                    detailDto = new ProductDetailDTO
                    {
                        ProductId = productID,
                        Quantity = Int32.Parse(quantity[i]),
                        Size = Int32.Parse(size[i])
                    };
                    list.Add(detailDto);
                }
                if (check = new ProductDetailData().UpdateQuantityByProductDTO(list))
                {
                    string[] newSize = Request.Form["txtNewSize"];
                    string[] newQuantity = Request.Form["txtNewQuantity"];
                    if (newSize != null && newQuantity != null)
                    {
                        List<ProductDetailDTO> newList = new List<ProductDetailDTO>();
                        ProductDetailDTO newDetailDto;
                        for (int i = 0; i < newSize.Length; i++)
                        {
                            newDetailDto = new ProductDetailDTO
                            {
                                ProductId = productID,
                                Quantity = Int32.Parse(newQuantity[i]),
                                Size = Int32.Parse(newSize[i])
                            };
                            newList.Add(newDetailDto);
                        }
                        check = new ProductDetailData().AddProductDetailsByProductDTO(newList);
                    }
                }
            }
            if (check)
            {
                ViewBag.Successful = "Update successfully!";
            }
            else
            {
                ViewBag.Failed = "Update failed!";
            }
       
            return View("ProductManager");
        }
        public IActionResult UploadImageAction()
        {
            return View("UploadImage");
        }

        private readonly string index = "~/Views/Home/Index.cshtml";

        public async Task<IActionResult> UploadImage(List<IFormFile> files)
        {
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/img",
                        formFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return View("UploadImage");
        }

        //H
        public IActionResult ProcessRemoveShoes()
        {
            string shoesIDStr = HttpContext.Request.Query["txtShoesID"];
            int id = Int32.Parse(shoesIDStr);
            if (new ShoesData().RemoveShoesByShoesID(id))
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
                //if (new ProductData().InsertProducts(listProducts))
                //{
                //    ViewBag.AddSuccess = "Add new product successfully!";
                //}
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
            ViewBag.Product = new ProductData().GetProductByProductID(productID);
            ViewBag.ListProductDetails = new ProductDetailData().GetProductDetailsByProductID(productID);
            return View("EditProduct");
        }
    }
}