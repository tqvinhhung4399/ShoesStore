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
        public IActionResult AddProduct()
        {
            return View();
        }
        public IActionResult ProductManager()
        {
            return View();
        }
        public IActionResult UserManager()
        {
            return View(checkAdmin()=="admin"?"":checkAdmin());
        }

        public IActionResult BanUser()
        {
            return View(checkAdmin());
        }
        

        private string checkAdmin()
        {
            if (HttpContext.Session.GetString("SessionRole") == null)
            {
                return "~/Views/Home/Index.cshtml";
            }
            else if (!HttpContext.Session.GetString("SessionRole").Equals("admin"))
            {
                return "~/Views/Home/Index.cshtml";
            }
            return "admin";
        }
    }
}