using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class TechFilesController : Controller
    {

        private readonly DBAdminContext _context;
       

        public TechFilesController(DBAdminContext context)
        {
            _context = context;

        }
        // GET: SalesCompanies
        public async Task<IActionResult> Index()
        {
            var lEmail = this.User.FindFirstValue(ClaimTypes.Name);

            ViewBag.User = lEmail; // HttpContext.Session.GetString("Email");
            if (lEmail == null)
            {
                //return RedirectToAction("Login", "SegUsuarios");
                return RedirectToAction("../Identity/Account/Login");
                //return RedirectToAction("Index", "Login");
            }

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == Email);
            if (User == null)
            {
                return NotFound();
            }
            Int64 IDUser = User.UserID;


            //var students = _context.SegUsuarios.FromSql($"GetUserName {IDUser}")
            //          .ToList();

            //comprobando que cumple los requisitos
            //Si el usuario es de Technical Support
            var segsistemausuario = from x in _context.SegSistemaUsuario
                                    where x.IdUsuario == IDUser &&
                                      x.CodigoSistema == 2
                                    select x;


            var admin = segsistemausuario.Count();

            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;


            //si encontro un usuario Significa que es de Technical Support por logica si tiene Acceso A Cases
            if (admin == 1)
            {

               
                return View(await _context.Techfiles.ToListAsync());
            }
            else
            {

                
                return RedirectToAction("../Home/Privacy");
                // return NotFound();
            }

            
        }
    }
}