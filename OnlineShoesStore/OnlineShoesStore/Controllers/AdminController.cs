using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public IActionResult AddProduct()
        {
            ViewBag.ListCategories = new CategoryData().GetCategories();
            ViewBag.ListBrands = new BrandData().GetBrands();
            ViewBag.ListOrigins = new OriginData().GetOrigins();
            return View();
        }

        //public IActionResult ProcessAddNewShoes()
        //{
        //    string name = Request.Form["txtName"];
        //    //lay cac parameter

        //    if (new ShoesData().AddNewShoes(shoes))
        //    {
        //        ViewBag.Success = "Add new shoes successfully";
        //        return View(); //view tuong ung sau khi AddNewShoes
        //    } else
        //    {
        //        ViewBag.Failed = "Add new shoes failed!";
        //        return View(); //view tuong ung khi add that bai
        //    }
            
        //}

        public IActionResult ProductManager()
        {
            if (CheckAdmin() != null) {
                return View(index);
            }
            return View();
        }

        public IActionResult LoadDataTable()
        {
            DataTableData data = new DataTableData();
         
            var result = data.GetData();
            return Json(result);
        }

        public IActionResult UserManager()
        {
            if (CheckAdmin() != null) {
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
            if (CheckAdmin() != null) { //role khong phai admin
                return View(index);
            }
            if (new UserData().BanUserByUsername(username))
            {
                ViewBag.BanSuccessful = "User " + username + " has been banned!";
                ViewBag.ListUser = data.LoadUsers();
            } else {
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
            return View("ProductManager");
        }
    }
}