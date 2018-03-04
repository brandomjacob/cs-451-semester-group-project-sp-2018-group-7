﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompareStream.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Login";
            return View();
        }

        public ActionResult Login()
        {
            //Add login functionality here
            var loginEmail = Request["loginEmail"];
            var loginPassword = Request["loginPassword"];
            ViewBag.Title = "Logged in";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
  
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}