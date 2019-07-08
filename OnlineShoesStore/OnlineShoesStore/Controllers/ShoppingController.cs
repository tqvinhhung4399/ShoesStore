using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShoesStore.Models;

namespace OnlineShoesStore.Controllers
{
    public class ShoppingController : Controller
    {
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}