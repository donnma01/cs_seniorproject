﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Ringo Starr - Profile";
            ViewData["Bio"] = "English musician, singer, actor, songwriter, and drummer for the Beatles.";
            ViewData["Location"] = "Liverpool";
            ViewData["ProfPicURL"] = "https://placekitten.com/g/64/64";
            ViewData["first_name"] = "Ringo";
            ViewData["last_name"] = "Starr";
            ViewData["Owner"] = "true";
            ViewData["ProfileType"] = "profile";
            return View();
        }

        public IActionResult Venue()
        {
            ViewData["Title"] = "Marty's Grill - Venue";
            ViewData["Bio"] = "Marty's is a great place to relax and enjoy some fantastic food and even better company!";
            ViewData["Address"] = "700 College Dr.";
            ViewData["City"] = "Decorah";
            ViewData["State"] = "IA";
            ViewData["BannerURL"] = "/images/banner3.svg";
            ViewData["ProfPicURL"] = "https://placekitten.com/g/64/64";
            ViewData["Name"] = "Marty's Grill";
            ViewData["Phone"] = "(563) 555-1234";
            ViewData["Owner"] = "true";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
