﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
           // ViewBag.Branches = PopulateBranches();

            var lEmail = this.User.FindFirstValue(ClaimTypes.Name);
            ViewBag.User = lEmail;
            if (lEmail == null)
            {
                return RedirectToAction("../Identity/Account/Login");
            }
            return View();
        }

        //public JsonResult selectSBU(string id)
        //{
        //    List<BranchModel> branches = PopulateBranches();
        //    var productos = branches.Where(x => x.Branchcode == id);
        //    return Json(branches.Where(x => x.Branchcode == id));
        //}

        //// Get Branch from Database.
        //private static List<BranchModel> PopulateBranches()
        //{
        //    List<BranchModel> branches = new List<BranchModel>();
        //    branches.Add(new BranchModel { Branchcode = "1", BranchName = "Branch 1" });
        //    branches.Add(new BranchModel { Branchcode = "2", BranchName = "Branch 2" });
        //    branches.Add(new BranchModel { Branchcode = "3", BranchName = "Branch 3" });
        //    branches.Add(new BranchModel { Branchcode = "4", BranchName = "Branch 4" });
        //    branches.Add(new BranchModel { Branchcode = "5", BranchName = "Branch 5" });
        //    return branches;
        //}

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
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
