using System;
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

        public IActionResult ViewUserInformation()
        {
            string username = HttpContext.Session.GetString("SessionUser");
            if (username != null)
            {
                ViewBag.UserInfo = new UserData().GetUserInfoByUserID(username);
            }
            else
            {
                return View("Index");
            }
            return View();
        }

        public IActionResult EditInformation()
        {
            if (!IsUser())
            {
                return View("Index");
            }
            ViewBag.UserInfo = new UserData().GetUserInfoByUserID(HttpContext.Session.GetString("SessionUser"));
            return View();
        }

        //public IActionResult ProcessEditInfo()
        //{
        //    string username = HttpContext.Session.GetString("SessionUser");
        //    string fullname = Request.Form["txtFullname"];
        //    string gender = Request.Form["slGender"];
        //    string dob = Request.Form["txtBirthdate"];
        //    string address = Request.Form["txtAddress"];
        //    string phoneNumber = Request.Form["txtPhonenumber"];
        //    UserDTO user = new UserDTO { Username = username, Fullname = fullname, Gender = gender, Dob = DateTime.Parse(dob), Address = address, Tel = phoneNumber};
        //    if (new UserData().UpdateUserInfoByUsername(user))
        //    {
        //        ViewBag.Success = "Update information successfully!";
        //    } else
        //    {
        //        ViewBag.Failed = "Update information failed!";
        //    }
        //    return RedirectToAction("ViewInfo");
        //}

        public IActionResult ViewInfo() //create ViewInfo.cshtml
        {
            UserDTO user = new UserData().GetUserInfoByUsername(HttpContext.Session.GetString("SessionUser"));
            ViewBag.User = user;
            return View();
        }

        public IActionResult ChangePassword() //create ChangePassword.cshtml
        {
            return View();
        }

        public IActionResult ProcessEditInfo()
        {
            string username = Request.Form["txtUsername"];
            string fullname = Request.Form["txtFullname"];
            string gender = Request.Form["slGender"];
            string dob = Request.Form["txtDob"];
            string address = Request.Form["txtAddress"];
            string phoneNumber = Request.Form["txtPhoneNumber"];
            UserDTO dto = new UserDTO
            {
                Fullname = fullname,
                Username = username,
                Gender = gender,
                Dob = DateTime.Parse(dob),
                Address = address,
                Tel = phoneNumber
            };
            if (new UserData().UpdateUserInfoByUsername(dto))
            {
                ViewBag.Successful = "Update successfully";

            }
            else
            {
                ViewBag.Failed = "Update failed";
            }
            ViewBag.UserInfo = new UserData().GetUserInfoByUserID(username);
            return View("ViewUserInformation");
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

        public IActionResult ProcessChangePassword()
        {
            if (!IsUser())
            {
                return View("Index");
            }
            string oldPassword = Request.Form["txtOldPassword"];
            string newPassword = Request.Form["txtNewPassword"];
            UserData data = new UserData();
            if(data.CheckMatchingOldPassword(oldPassword, HttpContext.Session.GetString("SessionUser")))
            {
                if(data.ChangePassword(HttpContext.Session.GetString("SessionUser"), newPassword))
                {
                    ViewBag.Successful = "Update password successfully";
                }
                else
                {
                    ViewBag.Failed = "Update password failed";
                }
                ViewBag.UserInfo = new UserData().GetUserInfoByUserID(HttpContext.Session.GetString("SessionUser"));
                return View("ViewUserInformation");
            }
            ViewBag.Error = "Wrong password";
            return View("ChangePassword");
        }

        public IActionResult ProcessRegister()
        {
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
            try
            {
                if (ud.RegisterUser(user))
                {
                    return View("Index");
                }
                else
                {
                    return View("Error");
                }
            } catch (Exception e)
            {
                ViewBag.Duplicate = "Username is already existed. Please input another username";
                return View("Register");
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
            }
            else if (user.Role.Equals("admin"))
            {
                HttpContext.Session.SetString("SessionUser", user.Username);
                HttpContext.Session.SetString("SessionRole", user.Role);
                return RedirectToAction("UserManager", "Admin");
            }
            else
            {
                HttpContext.Session.SetString("SessionUser", user.Username);
                HttpContext.Session.SetString("SessionRole", user.Role);
                return View("Index");
            }
        }

        public IActionResult ProcessLogout()
        {
            HttpContext.Session.Remove("SessionUser");
            HttpContext.Session.Remove("SessionRole");
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

        public bool IsUser()
        {
            string role = HttpContext.Session.GetString("SessionRole");
            if (role != null)
            {
                if (role.Equals("user"))
                {
                    return true;
                }
            }
            return false;

        }

    }
}
