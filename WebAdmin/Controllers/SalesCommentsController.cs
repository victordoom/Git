using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class SalesCommentsController : Controller
    {
        private readonly DBAdminContext _context;

        public SalesCommentsController(DBAdminContext context)
        {
            _context = context;
        }

        // GET: SalesComments
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

            var Employees = _context.Employees.Where(x => x.DepartmentId == 2);

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == Email);
            if (User == null)
            {
                return NotFound();
            }
            Int64 IDUser = User.UserID;

            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;



            return View(await Employees.ToListAsync());
        }

        
        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        public async Task<List<SalesComments>> mostarComments(int id)
        {
            List<SalesComments> Comments = new List<SalesComments>();

            var consul = await _context.SalesComments.ToListAsync();
            Comments.AddRange(consul);
            

            return  Comments;
        }

        public async Task<bool> CrearComments(int salesid, int commentby, string title, string comment)
        {
            var NewComment = new SalesComments
            {
                SalesId = salesid,
                CommentBy = commentby,
                Title = title,
                Comment = comment
            };

            var result = await _context.AddAsync(NewComment);
            await _context.SaveChangesAsync();

            return true;

        }
        
        public bool getByTo (int by, int to)
        {
            bool resp = false;



            return resp;
        }

        public async Task<List<SelectListItem>> GetAdmin()
        {

            
            List<SelectListItem> admins = new List<SelectListItem>();

           

            return admins;
        }
    }
}
