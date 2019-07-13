using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
    }
}