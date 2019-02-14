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
                ViewBag.Admin = "Admin";
                return View(await Employees.ToListAsync());
            }

            if (normal == 1)
            {
                ViewBag.Admin = "No";
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

        public async Task<List<InfoSalesComments>> mostarComments(string email)
        {
            //identificar el usuario seleccionado
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == email);

            //identificar usuario logeado
            var lEmail = this.User.FindFirstValue(ClaimTypes.Name);
            var UserLog = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == lEmail);

             var NombreUserLog = _context.SegUsuarios.SingleOrDefault(x => x.UserID == UserLog.UserID);

            var NombreSelect = _context.SegUsuarios.SingleOrDefault(y => y.UserID == User.UserID);
            //join
            var join = from comments in _context.SalesComments
                       join usu in _context.SegUsuarios on comments.CommentBy equals usu.UserID
                       select new InfoSalesComments
                       {
                           CommentId = comments.CommentId,
                           SalesId = comments.SalesId,
                           CommentBy = comments.CommentBy,
                           CommentDatetime = comments.CommentDatetime,
                           Title = comments.Title,
                           Comment = comments.Comment,
                           Nombre = usu.NombreUsuario + " " + usu.ApellidoUsuario,
                           UserLogeado = UserLog.UserID,
                           UserLogeadoNombre = NombreUserLog.NombreUsuario +" "+ NombreUserLog.ApellidoUsuario,
                           UserSelect = NombreSelect.UserID,
                           UserSelectNombre = NombreSelect.NombreUsuario +" "+ NombreSelect.ApellidoUsuario
                       };
            

            var con = await join.Where(x => x.SalesId == User.UserID || x.CommentBy == User.UserID).OrderBy(x => x.CommentDatetime).ToListAsync();

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

            
            List<InfoSalesComments> Comments = new List<InfoSalesComments>();

           // var consul = await _context.SalesComments.ToListAsync();
            Comments.AddRange(con);
            

            return  Comments;
        }

        public async Task<bool> CrearComments(int idby, int idto,  string comment, string title)
        {
            bool Exito = false;
            var NewComment = new SalesComments
            {
                SalesId = idto,
                CommentBy = idby,
                Title = title,
                Comment = comment
                
                
            };

             _context.Add(NewComment);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                Exito = true;
            } else
            {
                Exito = false;
            }
            

            return Exito;

        }
        
        public bool getByTo (int by, int to)
        {
            bool resp = false;



            return resp;
        }

        public async Task<List<SelectListItem>> GetAdmin()
        {
            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == Email);



            List<SelectListItem> admins = new List<SelectListItem>();
            var consul = from x in _context.SegSistemaUsuario
                         join y in _context.SegUsuarios on x.IdUsuario equals y.UserID
                         where x.CodigoSistema == 3 && x.CodigoPerfil == 1
                         select new Administradores { Admin = y.NombreUsuario + " " + y.ApellidoUsuario, AdminID = y.UserID };


            foreach (var item in consul)
            {
                if (item.AdminID != User.UserID)
                {
                    admins.Add(new SelectListItem()
                    {
                        Value = item.AdminID.ToString(),
                        Text = item.Admin

                    });
                }
               
            }




            return admins;
        }

            class Administradores
            {
            public int AdminID { get; set; }
            public string Admin { get; set; }


            }

        public class InfoSalesComments
        {
            public int CommentId { get; set; }
            public int? SalesId { get; set; }
            public int? CommentBy { get; set; }
            public DateTime? CommentDatetime { get; set; }
            public string Comment { get; set; }
            public string Title { get; set; }




            public string Nombre { get; set; }
            public int UserLogeado { get; set; }
            public string UserLogeadoNombre { get; set; }
            public int UserSelect { get; set; }
            public string UserSelectNombre { get; set; }
        }
    }

}

