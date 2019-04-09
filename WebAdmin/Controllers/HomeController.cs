using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Models;
using WebAdmin.Services;

namespace WebAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBAdminContext _context;

        public HomeController(DBAdminContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
           // ViewBag.Branches = PopulateBranches();

            var lEmail = this.User.FindFirstValue(ClaimTypes.Name);
            ViewBag.User = lEmail;
           

            if (lEmail == null)
            {
                return RedirectToAction("../Identity/Account/Login");
            }

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User =  _context.AspNetUsers.SingleOrDefault(m => m.Email == Email);
            if (User == null)
            {
                return NotFound();
            }
            Int64 IDUser = User.UserID;

            var sistem = _context.SegSistemaUsuario.Where(x => x.IdUsuario == IDUser);

            List<int?> idsis = new List<int?>();
            List<string> nomsis = new List<string>();


            foreach (var item in sistem)
            {
                
                idsis.Add(item.CodigoSistema);
                
            }

            foreach (var item in idsis)
            {

                var con = from x in _context.Sistemas
                          where x.CodigoSistema == item
                          select x.NombreSistema;
                foreach (var nom in con)
                {
                    nomsis.Add(nom);
                }
                
            }

            UserRol.UserRol user = new UserRol.UserRol(nomsis);


            ViewBag.RolSystem = user.Rol;

           // Proxy proxy = new Proxy();
          //  var prueba = proxy.GetAll();
          //  var segunda = proxy.GetByid(5);
            
            return View();
        }

        

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
