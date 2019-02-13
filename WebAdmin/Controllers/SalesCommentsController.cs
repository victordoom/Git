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

            //departament Sales
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


            //comprobando que cumple los requisitos
            //Si el usuario es de Sales Deparment y Admin
            var segsistemausuario = from x in _context.SegSistemaUsuario
                                    where x.IdUsuario == IDUser &&
                                      x.CodigoSistema == 3 && x.CodigoPerfil == 1
                                    select x;

            //filtro solo el empleado sales logeado
            var empleadosales = from x in _context.Employees
                                where x.EmailAddress == Email
                                select x;

            var normal = empleadosales.Count();
            var admin = segsistemausuario.Count();
            if (admin == 1)
            {
                return View(await Employees.ToListAsync());
            }

            if (normal == 1)
            {
                return View(await empleadosales.ToListAsync());
            }

            else
            {
                return RedirectToAction("../Home/Privacy");
                // return NotFound();
            }







        }

        
        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        public async Task<List<SalesComments>> mostarComments(string email)
        {
            //identificar el usuario logeado
            
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == email);

            //join
            var join = from comments in _context.SalesComments
                       join usu in _context.SegUsuarios on comments.SalesId equals usu.UserID
                       select new SalesComments
                       {
                           CommentId = comments.CommentId,
                           SalesId = comments.SalesId,
                           CommentBy = comments.CommentBy,
                           CommentDatetime = comments.CommentDatetime,
                           Title = comments.Title,
                           Comment = comments.Comment,
                           Nombre = usu.NombreUsuario + " " + usu.ApellidoUsuario
                       };
            var con = await join.Where(x => x.SalesId == User.UserID || x.CommentBy == User.UserID).ToListAsync();

            //var consulta = await _context.SalesComments.Where(x => x.SalesId == User.UserID || x.CommentBy == User.UserID).ToListAsync();

            ////Usuarios con perfil de administrador
            //var segsistemausuario = from x in _context.SegSistemaUsuario
            //                        where  x.CodigoSistema == 3 && x.CodigoPerfil == 1
            //                        select x;
            //List<string> Administradores = new List<string>();
            //foreach (var item in segsistemausuario)
            //{
            //    var nombreadmin = _context.SegUsuarios.Where(x => x.UserID == item.IdUsuario);
            //    foreach (var nom in nombreadmin)
            //    {
            //        Administradores.Add(nom.NombreUsuario + nom.ApellidoUsuario);
            //    }
                
            //}

            
            List<SalesComments> Comments = new List<SalesComments>();

           // var consul = await _context.SalesComments.ToListAsync();
            Comments.AddRange(con);
            

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
