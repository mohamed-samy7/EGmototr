using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EGYmotor.Models;
using EGYmotor.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EGYmotor.Controllers
{
    public class LoginController : Controller
    {
        private readonly EGYmotorContext db;

        public LoginController(EGYmotorContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUser());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterUser model)
        {
            var existingUser = db.RegisterUser.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View("Login", model);
            }

            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.UserName) || model.UserName.Length < 3 || model.UserName.Length > 50)
                errors.Add("User name must be between 3 and 50 characters.");
            else if (!Regex.IsMatch(model.UserName, @"^[\p{L} ]+$"))
                errors.Add("Invalid Name !");

            if (string.IsNullOrWhiteSpace(model.Email))
                errors.Add("Email is required.");
            else if (!new EmailAddressAttribute().IsValid(model.Email))
                errors.Add("Invalid email format.");

            if (string.IsNullOrWhiteSpace(model.PhoneNumber) || model.PhoneNumber.Length != 11 || !model.PhoneNumber.All(char.IsDigit))
                errors.Add("Phone number must be exactly 11 digits and contain only numbers.");

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 8)
                errors.Add("Password must be at least 8 characters long.");
            else if (!Regex.IsMatch(model.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$"))
                errors.Add("Password must contain at least one uppercase letter, one lowercase letter, and one number.");

            if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
                errors.Add("Confirm password is required.");
            else if (model.Password != model.ConfirmPassword)
                errors.Add("Password and confirmation password do not match.");

            if (errors.Any())
            {
                foreach (var error in errors)
                    ModelState.AddModelError("", error);
                return View("Login", model);
            }

            // تشفير الباسورد 
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            model.ConfirmPassword = model.Password;
            model.RegisterDate = DateTime.Now;

            db.RegisterUser.Add(model);
            db.SaveChanges();
            return RedirectToAction("Login", "Login");  
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Email, string Password)
        {
            var user = db.RegisterUser.FirstOrDefault(u => u.Email == Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                HttpContext.Session.SetInt32("UserId", user.RegisterUserId);
                HttpContext.Session.SetString("UserName", user.UserName);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.LoginError = "Invalid email or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}