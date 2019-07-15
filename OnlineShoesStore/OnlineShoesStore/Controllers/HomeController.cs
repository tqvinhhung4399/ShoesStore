﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineShoesStore.Models;
using Microsoft.Extensions.Configuration;

namespace OnlineShoesStore.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }
        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ProcessRegister() {
            string username = Request.Form["txtUsername"];
            string password = Request.Form["txtPassword"];
            string gender = Request.Form["txtGender"];
            string fullname = Request.Form["txtFullname"];
            string dob = Request.Form["txtDob"];
            string address = Request.Form["txtAddress"];
            string phoneNumber = Request.Form["txtPhoneNumber"];
            string role = "user";
            bool isDeleted = false;
            //UserData ud = new UserData(Configuration);

            UserData ud = new UserData();
            UserDTO user = new UserDTO(username, password, fullname, gender, DateTime.Parse(dob), address, phoneNumber, isDeleted, role);
            if (ud.RegisterUser(user))
            {
                return View("Index");
            } else
            {
                return View("Error");
            }
        }

        public IActionResult ProcessLogin()
        {
            string username = Request.Form["txtUsername"];
            string password = Request.Form["txtPassword"];
            UserDTO user = new UserData().CheckLogin(username, password);
            if (user == null)
            {
                ViewBag.Invalid = "Wrong username or password";
                ViewBag.Username = username;
                return View("Login");
            } else if (user.Role.Equals("admin"))
            {
                HttpContext.Session.SetString("SessionUser", user.Username);
                HttpContext.Session.SetString("SessionRole", user.Role);
                return RedirectToAction("UserManager", "Admin");
            } else
            {
                HttpContext.Session.SetString("SessionUser", user.Username);
                HttpContext.Session.SetString("SessionRole", user.Role);
                return View("Index");
            }
        }

        public IActionResult ProcessLogout()
        {
            HttpContext.Session.SetString("SessionUser", null);
            HttpContext.Session.SetString("SessionRole", null);
            return View("Index");
        }

        public IActionResult Index()
        {
            //List<CategoryDTO> categories = new CategoryData().GetCategories();
            //ViewBag.Categories = categories;
            return View();
        }

        public IActionResult Category()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
