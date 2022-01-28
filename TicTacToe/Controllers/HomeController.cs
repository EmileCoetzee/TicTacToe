﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicTacToe.DataAccess;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    public class HomeController : Controller
    {
        Query _query;
        Ganss.XSS.HtmlSanitizer _htmlSanitizer;

        public HomeController()
        {
            _query = new Query();
            _htmlSanitizer = new Ganss.XSS.HtmlSanitizer();
        }


        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Start 
        public ActionResult Start()
        {
            return View();
        }

        // GET: Home/GetPlayer1Form (Partial)
        public PartialViewResult GetPlayer1Form()
        {
            Player player = new Player();

            player.isPlayer1 = true;

            return PartialView(player);
        }

        // GET: Home/GetPlayer2Form (Partial)
        public PartialViewResult GetPlayer2Form()
        {
            Player player = new Player();

            player.isPlayer1 = true;

            return PartialView(player);
        }

        // POST: Home/AddPlayer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPlayer(Player model)
        {

            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Error, some data is missing.  Please ensure that all fields are entered.");


                model.Name = _htmlSanitizer.Sanitize(model.Name);

                //check if player exists


                return Json(new { data = ".", resultCode = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message.ToString(), resultCode = 0 }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}