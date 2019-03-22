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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techfiles = await _context.Techfiles.FirstOrDefaultAsync(m => m.IdRow == id);
            if (techfiles == null)
            {
                return NotFound();
            }

            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;


            return View(techfiles);
        }

        public JsonResult AnexosTechFiles(string id)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;


               

               var customerData = _context.Attachments.Where(c => c.Typeid == "01" && c.StringId == id && c.TechnicalSupport == '1');






                //Sorting  
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Idattachfile.ToString() == searchValue ||
                    m.FileName.ToLower().Contains(searchValue.ToLower()) || m.FileType.Contains(searchValue.ToLower()));
                }
                



                //total number of rows count   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize).ToList();


              //  var detalles = _context.Attachments.Where(c => c.Typeid == "01" && c.StringId == id);

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}