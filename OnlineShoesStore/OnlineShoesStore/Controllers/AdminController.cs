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
            return View();
        }
        public IActionResult ProductManager()
        {
            if (CheckAdmin() != null) {
                return View(index);
            }
            
            return View();
        }
        public IActionResult UserManager()
        {
            if (CheckAdmin() != null) {
                return View(index);
            }
            ViewBag.ListUser = new UserData().LoadUsers();
            return View(CheckAdmin());
        }

        public IActionResult BanUser()
        {
            if (CheckAdmin() != null) { //role khong phai admin
                return View(index);
            }
            string username = HttpContext.Request.Query["username"];
            if (new UserData().BanUserByUsername(username))
            {
                ViewBag.BanSuccessful = "User " + username + "has been banned!";
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
    }
}