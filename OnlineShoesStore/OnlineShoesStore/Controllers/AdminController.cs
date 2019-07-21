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
        private readonly string index = "~/Views/Home/Index.cshtml";

        //H
        public IActionResult ProcessUpdateShoes()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
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
        }

        //H
        public IActionResult RestoreProduct()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
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
            if (!IsAdmin())
            {
                return View(index);
            }
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
            if (!IsAdmin())
            {
                return View(index);
            }
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
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.ProductID = HttpContext.Request.Query["txtProductID"];
            return View("UploadImage");
        }

        public async Task<IActionResult> UploadImage(List<IFormFile> files)
        {

            List<string> images = new List<string>();
            int productID = int.Parse(Request.Form["txtProductID"]);
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string image = "/img/" + formFile.FileName;
                    // full path to file in temp location
                    images.Add(image);
                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/img",
                        formFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            if (new ProductImageData().InsertImagesByProductID(productID, images))
            {
                ViewBag.Announcement = "Upload image(s) for Product (ID = " + productID + ") successfully";
            } else
            {
                ViewBag.Announcement = "Upload image(s) for Product (ID = " + productID + ") failed";
            }
            return View("ProductManager");
        }

        //H
        public IActionResult ProcessRemoveShoes()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
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
            if (!IsAdmin())
            {
                return View(index);
            }
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
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.ListCategories = new CategoryData().GetCategories();
            ViewBag.ListBrands = new BrandData().GetBrands();
            ViewBag.ListOrigins = new OriginData().GetOrigins();
            return View();
        }

        public IActionResult ProcessAddNewShoes()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
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
                    ViewBag.Anouncement = "Add new product successfully!";
                }
            }
            else
            {
                ViewBag.Anouncement = "Add new product failed!";
            }
            ViewBag.ListShoes = new ShoesData().GetAllShoes();
            return View("ShoesManager");
        }

        //H
        public IActionResult ShoesManager()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.ListShoes = new ShoesData().GetAllShoes();
            return View();
        }

        //H
        public IActionResult EditShoes()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
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
            if (!IsAdmin())
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
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.ListUser = new UserData().LoadUsers();
            return View();
        }

        public IActionResult UnbanUser(string username)
        {
            UserData data = new UserData();
            if (!IsAdmin())
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
            if (!IsAdmin())
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


        private bool IsAdmin()
        {
            if (HttpContext.Session.GetString("SessionRole") == null) //chưa đăng nhập
            {
                return false;
            }
            else if (!HttpContext.Session.GetString("SessionRole").Equals("admin")) //role khác admin
            {
                return false;
            }
            return true; //role là admin
        }

        //Quản lí sản phảm
        public IActionResult EditProduct()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            string shoesIDStr = HttpContext.Request.Query["txtProductID"];
            int productID = Int32.Parse(shoesIDStr);
            ViewBag.ShoesForPageEditProduct = new ShoesData().GetShoesDetailByProductID(productID);
            ViewBag.Product = new ProductData().GetProductByProductID(productID);
            ViewBag.ListProductDetails = new ProductDetailData().GetProductDetailsByProductID(productID);
            return View("EditProduct");
        }

        //H
        public IActionResult ProcessEditShoes()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
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
            string[] productIDs = Request.Form["txtProductID"];
            string[] newColors = Request.Form["txtNewColor"];
            string[] newPrices = Request.Form["txtNewPrice"];
            if (check = new ShoesData().UpdateShoes(new ShoesDTO {ShoesId = shoesID, Name = name, CategoryId = categoryID, BrandId = brandID, Material = material, Description = description, OriginId = originID }))
            {
                List<ProductDTO> listProducts = new List<ProductDTO>();
                for (int i = 0; i < colors.Length; i++)
                {
                    listProducts.Add(new ProductDTO {ProductId = int.Parse(productIDs[i]) , ShoesId = shoesID, Color = colors[i], Price = double.Parse(prices[i]) });
                }
                if (check = new ProductData().UpdateListProducstById(listProducts))
                {
                    if (newColors != null || newPrices != null)
                    {
                        List<ProductDTO> newList = new List<ProductDTO>();
                        for (int i = 0; i < newColors.Length; i++)
                        {
                            newList.Add(new ProductDTO { ShoesId = shoesID, Color = colors[i], Price = double.Parse(prices[i]), IsDeleted = false });
                        }
                        check = new ProductData().InsertProducts(newList);
                    }
                }
            }
            if (check)
            {
                ViewBag.Announcement = "Edit shoes successfully!";
            }
            else
            {
                ViewBag.Announcement = "Edit shoes failed!";
            }
            ViewBag.ListShoes = new ShoesData().GetAllShoes();
            return View("ShoesManager");
        }

        public IActionResult BrandManager()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.Brand = new BrandData().GetBrands();
            return View();
        }

        public IActionResult EditBrand()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.ID = HttpContext.Request.Query["txtBrandID"];
            int id = int.Parse(HttpContext.Request.Query["txtBrandID"]);
            ViewBag.Brand = new BrandData().GetBrandNameByID(id);
            return View();
        }

        public IActionResult ProcessEditBrand()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            string brand = Request.Form["txtBrand"];
            int id = int.Parse(Request.Form["txtBrandID"]);
            if(new BrandData().UpdateBrand(new BrandDTO { BrandId = id, Name = brand }))
            {
                ViewBag.Announcement = "Update Successfully";
            }
            else
            {
                ViewBag.Announcement = "Update Failed";
            }
            ViewBag.Brand = new BrandData().GetBrands();
            return View("BrandManager");
        }

        public IActionResult AddBrand()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            return View();
        }

        public IActionResult ProcessAddBrand()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            string brand = Request.Form["txtBrand"];
            if(new BrandData().AddNewBrand(brand))
            {
                ViewBag.Announcement = "Add new brand successfully";
            }
            else
            {
                ViewBag.Announcement = "Add new brand failed";
            }
            ViewBag.Brand = new BrandData().GetBrands();
            return View("BrandManager");
        }

        public IActionResult CategoryManager()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.Category = new CategoryData().GetCategories();
            return View();
        }

        public IActionResult EditCategory()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.ID = HttpContext.Request.Query["txtCategoryID"];
            int id = int.Parse(HttpContext.Request.Query["txtCategoryID"]);
            ViewBag.Category = new CategoryData().getCategoryNameByCategoryID(id);
            return View();
        }

        public IActionResult ProcessEditCategory()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            string category = Request.Form["txtCategory"];
            int id = int.Parse(Request.Form["txtCategoryID"]);
            if (new CategoryData().UpdateCategory(new CategoryDTO { CategoryId = id, Name = category }))
            {
                ViewBag.Announcement = "Update Successfully";
            }
            else
            {
                ViewBag.Announcement = "Update Failed";
            }
            ViewBag.Category = new CategoryData().GetCategories();
            return View("CategoryManager");
        }

        public IActionResult AddCategory()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            return View();
        }

        public IActionResult ProcessAddCategory()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            string category = Request.Form["txtCategory"];
            if (new CategoryData().AddNewCategory(category))
            {
                ViewBag.Announcement = "Add new category successfully";
            }
            else
            {
                ViewBag.Announcement = "Add new category failed";
            }
            ViewBag.Category = new CategoryData().GetCategories();
            return View("CategoryManager");
        }

        public IActionResult OriginManager()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.Origin = new OriginData().GetOrigins();
            return View();
        }

        public IActionResult EditOrigin()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.ID = HttpContext.Request.Query["txtOriginID"];
            int id = int.Parse(HttpContext.Request.Query["txtOriginID"]);
            ViewBag.Origin = new OriginData().GetOriginNameByID(id);
            return View();
        }

        public IActionResult ProcessEditOrigin()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            string origin = Request.Form["txtOrigin"];
            int id = int.Parse(Request.Form["txtOriginID"]);
            if (new OriginData().UpdateOrigin(new OriginDTO { OriginId = id, Name = origin }))
            {
                ViewBag.Announcement = "Update Successfully";
            }
            else
            {
                ViewBag.Announcement = "Update Failed";
            }
            ViewBag.Origin = new OriginData().GetOrigins();
            return View("OriginManager");
        }

        public IActionResult AddOrigin()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            return View();
        }

        public IActionResult ProcessAddOrigin()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            string origin = Request.Form["txtOrigin"];
            if (new OriginData().AddNewOrigin(origin))
            {
                ViewBag.Announcement = "Add new origin successfully";
            }
            else
            {
                ViewBag.Announcement = "Add new origin failed";
            }
            ViewBag.Origin = new OriginData().GetOrigins();
            return View("OriginManager");
        }

        public IActionResult OrderManager()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.Orders = new OrderData().GetAllOrders();
            return View();
        }

        public IActionResult ProcessChangeOrderStatus()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            int id = int.Parse(HttpContext.Request.Query["txtOrderID"]);
            string status = HttpContext.Request.Query["txtStatus"];
            if(new OrderData().UpdateOrderStatusByOrderID(id, status))
            {
                ViewBag.Announcement = "Update status successfully";
            }
            else
            {
                ViewBag.Announcement = "Update status failed";
            }
            ViewBag.Orders = new OrderData().GetAllOrders();
            return View("OrderManager");
        }

        public IActionResult ContactManager()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            ViewBag.Contact = new ContactData().GetAllData();
            return View();
        }

        public IActionResult ViewContactMessage()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            int id = int.Parse(HttpContext.Request.Query["txtContactID"]);
            ViewBag.Message = new ContactData().GetMessageByContactID(id);
            return View();
        }

        public IActionResult WelcomeAdmin()
        {
            if (!IsAdmin())
            {
                return View(index);
            }
            return View();
        }
    }
}