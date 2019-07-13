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
            return View();
        }

        public IActionResult BanUser()
        {
            checkAdmin();
            string username;
            return View();
        }

        public IActionResult Test()
        {
            if (checkAdmin())
            {
                return RedirectToAction("UserManager");
            }
            
        }

        private bool checkAdmin()
        {
            if (HttpContext.Session.GetString("SessionRole") == null)
            {
                RedirectToAction("Index", "Home");
                return false;
            } else 
            if (!HttpContext.Session.GetString("SessionRole").Equals("admin"))
            {
                RedirectToAction("Index", "Home");
                return false;
            }
            return true;
        }
    }
}