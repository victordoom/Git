﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAdmin.Models;
using WebAdmin.Models.Functions;
using WebAdmin.UserRol;

namespace WebAdmin.Controllers
{
    public class OpportunitiesController : Controller
    {
        private readonly DBAdminContext _context;
        private static string LeadOnline { get; set; }
        private  string SuccessWin { get; set; }
       

        public IConfiguration Configuration { get; private set; }

        public OpportunitiesController(DBAdminContext context, IConfiguration con)
        {
            _context = context;
            Configuration = con;
          
        }

        
        #region Opportunities

        public async Task<IActionResult> Dash()
        {
            var lEmail = this.User.FindFirstValue(ClaimTypes.Name);

            ViewBag.User = lEmail; // HttpContext.Session.GetString("Email");
            ViewBag.Email = lEmail;

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == Email);
            if (User == null)
            {
                return NotFound();
            }
            Int64 IDUser = User.UserID;

            //para almacenar sesion en la tabla oppor
            ViewBag.Oppordata = IDUser;
            // si es de Sales y Admin
            var segsistemausuario = from x in _context.SegSistemaUsuario
                                    where x.IdUsuario == IDUser &&
                                      x.CodigoSistema == 3 && x.CodigoPerfil == 1
                                    select x;


            var usersales = from s in _context.SegUsuarios
                            from a in _context.Employees
                            where s.IdPersona == a.Id.ToString() where a.DepartmentId == 2 &&  a.Status == true
                            select new Administradores { Admin = s.NombreUsuario + " " + s.ApellidoUsuario, AdminID = s.UserID };




            List<SelectListItem> admins = new List<SelectListItem>();
            //var consul = from x in _context.SegSistemaUsuario
            //             join y in _context.SegUsuarios on x.IdUsuario equals y.UserID
            //             where x.CodigoSistema == 3
            //             select new Administradores { Admin = y.NombreUsuario + " " + y.ApellidoUsuario, AdminID = y.UserID };

            var sales = new List<SelectListItem>();

            sales.Add( new SelectListItem {
                Value = "0",
                Text = "All"
            });

            foreach (var item in usersales)
            {
                sales.Add(new SelectListItem
                {
                    Value = item.AdminID.ToString(),
                    Text = item.Admin
                });
            }

            ViewData["Sales"] = sales;
            ViewBag.UserID = User.UserID;
            ViewBag.Adminsales = segsistemausuario.Count();

            if (ViewBag.Adminsales == 1)
            {
                ViewBag.Esadmin = 0;
            }
            else
            {
                ViewBag.Esadmin = IDUser;
            }
            // le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;

            return View();
        }
        // GET: Opportunities
        public async Task<IActionResult> Index(Opportunities opportunities, string Success)
        {
            if (!string.IsNullOrEmpty(Success))
            {
                ViewBag.Success = Success;
            }
            else
            {
                ViewBag.Success = opportunities.Alerta;
            }

            var lEmail = this.User.FindFirstValue(ClaimTypes.Name);

            ViewBag.User = lEmail; // HttpContext.Session.GetString("Email");
            if (lEmail == null)
            {
                return RedirectToAction("../Identity/Account/Login");

            }

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == Email);
            if (User == null)
            {
                return NotFound();
            }
            Int64 IDUser = User.UserID;

            ViewBag.UserID = User.UserID;


            //Oppor Clopsing Reason
            var reason = from x in _context.OpportunitiesClosingReason
                         select x;

            var closingReason = new List<SelectListItem>();

            closingReason.Add(new SelectListItem
            {
                Value = "0",
                Text = "Select Closing Reason"
            });

            foreach (var item in reason)
            {
                closingReason.Add(new SelectListItem
                {
                    Value = item.ClosingReasonID.ToString(),
                    Text = item.ClosingReasonDescription
                });
            }

            ViewData["ClosingReason"] = closingReason;

            //Users opportunities sales
            var usersales = from s in _context.SegUsuarios
                            from a in _context.Employees
                            where s.IdPersona == a.Id.ToString()
                            where a.DepartmentId == 2 && a.Status == true
                            select new Administradores { Admin = s.NombreUsuario + " " + s.ApellidoUsuario, AdminID = s.UserID };

            var sales = new List<SelectListItem>();

            
            foreach (var item in usersales)
            {
                sales.Add(new SelectListItem
                {
                    Value = item.AdminID.ToString(),
                    Text = item.Admin
                });
            }

            
            ViewBag.DDLUsers = sales;

            var dat = new DateTime(2015, 12, 31);
            for (int ctr = 0; ctr <= 15; ctr++)
                Console.WriteLine(dat.AddMonths(ctr).ToString("d"));
            //modal create
            Opportunities model = new Opportunities();
            model.CreatedDate = DateTime.Today;
            model.VisitedDate = DateTime.Today;
            model.OpenDate = DateTime.Today;
            model.UserID = User.UserID;
            model.Rating = "Cold";
            model.EstRevenue = 0;



            ViewBag.Users = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario");
            ViewBag.DDLCategories = new SelectList(_context.OpportunitiesCategories, "CategoryID", "CategoryDescription");
            ViewBag.DDLHowFound = new SelectList(_context.OpportunitiesHowFound, "HowFoundID", "HowFoundDescription");
            ViewBag.DDLPrograms = new SelectList(_context.Programs, "ProgramID", "ProgramShortName");
            //record since the last twelve month for opportunities that is equal to online
            var DateFromOnline = DateTime.Today.AddMonths(-12);
            // record since the last three month for opportunities that is not equal to online
            var DateFromOthers = DateTime.Today.AddMonths(-3);
            //var lastmonth = new DateTime(today.Year, today.Month - 3,today.Day);

            var today = DateFromOthers;
            var todayOnline = DateFromOnline;

            
            // si es de Sales y Admin
            var segsistemausuario = from x in _context.SegSistemaUsuario
                                    where x.IdUsuario == IDUser &&
                                      x.CodigoSistema == 3 && x.CodigoPerfil == 1
                                    select x;


            // si es solo Sales
            var segsistemausuarionormal = from x in _context.SegSistemaUsuario
                                          where x.IdUsuario == IDUser &&
                                            x.CodigoSistema == 3
                                          select x;

            var normal = segsistemausuarionormal.Count();

            var admin = segsistemausuario.Count();


            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;

            
            //si encontro un usuario Significa que es de Sales por logica si tiene Acceso Sales
            if (admin == 1)
            {
                //consulta para admin
                var dBAdminContext = _context.Opportunities
                    //.Where(c => c.UserID == IDUser)
                    .Where(c => (c.VisitedDate >= todayOnline && c.HowFoundID == 10) || (c.VisitedDate >= today && c.HowFoundID != 10))
                    .OrderByDescending(c => c.ID)
                    .Include(c => c.OpportunitiesDetails);

                ViewBag.Esadmin = 0;
                ViewBag.Rol = "Administrador";
                return View(model);
            }

            if (normal == 1)
            {
                //consulta para user
                var consulSales = _context.Opportunities
                    .Where(c => c.UserID == IDUser)
                    .Where(c => (c.VisitedDate >= todayOnline && c.HowFoundID == 10) || (c.VisitedDate >= today && c.HowFoundID != 10))
                    .OrderByDescending(c => c.ID)
                    .Include(c => c.OpportunitiesDetails);

                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Privacy");
                // return NotFound();
            }

        }

        // GET: Opportunities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunities = await _context.Opportunities
                .Include(c => c.OpportunitiesDetails)
                .FirstOrDefaultAsync(m => m.ID == id);
                

            if (opportunities == null)
            {
                return NotFound();
            }

            ViewBag.DDLUsers = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario");
            ViewBag.DDLCategories = new SelectList(_context.OpportunitiesCategories, "CategoryID", "CategoryDescription");
            ViewBag.DDLHowFound = new SelectList(_context.OpportunitiesHowFound, "HowFoundID", "HowFoundDescription");
            ViewBag.DDLPrograms = new SelectList(_context.Programs, "ProgramID", "ProgramShortName");
            ViewBag.DDLClosingreason = new SelectList(_context.OpportunitiesClosingReason, "ClosingReasonID", "ClosingReasonDescription");

            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;

            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == Email);
            if (User == null)
            {
                return NotFound();
            }
            Int64 IDUser = User.UserID;

            ViewBag.UserID = User.UserID;

            // si es de Sales y Admin
            var segsistemausuario = from x in _context.SegSistemaUsuario
                                    where x.IdUsuario == IDUser &&
                                      x.CodigoSistema == 3 && x.CodigoPerfil == 1
                                    select x;

            var Admin = segsistemausuario.Count();
            if (Admin >= 1)
            {
                ViewBag.Admin = "Administrador";
            }
            

            return View(opportunities);
        }

        // GET: Opportunities/Create
        public IActionResult Create()
        {
            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = _context.AspNetUsers.Single(m => m.Email == Email);
            if (User == null)
            {
                return NotFound();
            }

            Opportunities model = new Opportunities();
            model.CreatedDate = DateTime.Today;
            model.VisitedDate = DateTime.Today;
            model.OpenDate = DateTime.Today;
            model.UserID = User.UserID;
            model.Rating = "Cold";
            model.EstRevenue = 0;

            ViewBag.DDLUsers = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario");
            ViewBag.DDLCategories = new SelectList(_context.OpportunitiesCategories, "CategoryID", "CategoryDescription");
            ViewBag.DDLHowFound = new SelectList(_context.OpportunitiesHowFound, "HowFoundID", "HowFoundDescription");
            ViewBag.DDLPrograms = new SelectList(_context.Programs, "ProgramID", "ProgramShortName");


            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;


            return View(model);
        }

        // POST: Opportunities/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Opportunities opportunities, string dComment, string dStatus, string dNextVisit, string dEmailnotification)
        {
            string Success = "";
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;

            OpportunitiesDetails detail = new OpportunitiesDetails();
            if (ModelState.IsValid)
            {
                string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
                string Email = this.User.FindFirstValue(ClaimTypes.Name);
                var User = _context.AspNetUsers.Single(m => m.Email == Email);

                opportunities.UserID = User.UserID;
                opportunities.CreatedDate = DateTime.Now;
                opportunities.Web = "1";

                //define probability according to rating
                switch (opportunities.Rating)
                {
                    case "Hot":
                        opportunities.Probability = 0.9;
                        break;
                    case "Warm":
                        opportunities.Probability = 0.7;
                        break;
                    default:
                        opportunities.Probability = 0.3;
                        break;
                }

                _context.Add(opportunities);

                var resul = await _context.SaveChangesAsync();

                if (dEmailnotification == "on")
                {
                    dEmailnotification = "1";
                }
                else { dEmailnotification = "0"; }

                //save followup
                if (resul > 0 && !string.IsNullOrEmpty(dComment) && !string.IsNullOrEmpty(dStatus) && !string.IsNullOrEmpty(dEmailnotification))
                {
                    if (!string.IsNullOrEmpty(dNextVisit))
                    {
                        DateTime datNextVisit = DateTime.Parse(dNextVisit);
                        detail.NextVisitDate = datNextVisit;
                    }
                    var id = opportunities.ID;
                    detail.OpportunitiesID = id;
                    detail.UserID = (int)opportunities.UserID;
                    detail.Comment = dComment;
                    detail.Status = dStatus;
                    detail.CreatedDate = DateTime.Now;
                    detail.DetailDatetime= DateTime.Now;
                    detail.VisitDate = (DateTime)opportunities.VisitedDate;
                    detail.EmailSent = "0";
                    detail.EmailNotification = dEmailnotification;

                    _context.Add(detail);

                    //send mail
                    await _context.SaveChangesAsync();

                    
                    SuccessWin = "Exito";
                   
                    Success = "Exito";


                    return RedirectToAction(nameof(Index), new { Success = SuccessWin } );
                }

                SuccessWin = "Exito";
                
                return RedirectToAction(nameof(Index), new { Success = SuccessWin });
            }
            SuccessWin = "Error";
            opportunities.Alerta = SuccessWin;
            
            return RedirectToAction(nameof(Index), opportunities);
        }

       

        // GET: Opportunities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunities = await _context.Opportunities.FindAsync(id);
            if (opportunities == null)
            {
                return NotFound();
            }

            ViewBag.DDLUsers = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario", opportunities.UserID);
            ViewBag.DDLCategories = new SelectList(_context.OpportunitiesCategories, "CategoryID", "CategoryDescription", opportunities.CategoryID);
            ViewBag.DDLHowFound = new SelectList(_context.OpportunitiesHowFound, "HowFoundID", "HowFoundDescription", opportunities.HowFoundID);
            ViewBag.DDLPrograms = new SelectList(_context.Programs, "ProgramID", "ProgramShortName", opportunities.ProgramID);
            

            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;
            
             //  return View(opportunities);
            return PartialView("_EditView", opportunities);
        }

        // POST: Opportunities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( Opportunities opportunities)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    
                    //define probability according to rating
                    switch (opportunities.Rating)
                    {
                        case "Hot":
                            opportunities.Probability = 0.9;
                            break;
                        case "Warm":
                            opportunities.Probability = 0.7;
                            break;
                        default:
                            opportunities.Probability = 0.3;
                            break;
                    }

                    _context.Update(opportunities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpportunitiesExists(opportunities.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                
                SuccessWin = "ExitoEdit";

                return RedirectToAction(nameof(Index), new { Success = SuccessWin });
            }

            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;

            SuccessWin = "Error";

            return RedirectToAction(nameof(Index), new { Success = SuccessWin });
        }

        // GET: Opportunities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunities = await _context.Opportunities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (opportunities == null)
            {
                return NotFound();
            }

            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;

            return View(opportunities);
        }

        // POST: Opportunities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opportunities = await _context.Opportunities.FindAsync(id);
            _context.Opportunities.Remove(opportunities);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        

        private bool OpportunitiesExists(int id)
        {
            return _context.Opportunities.Any(e => e.ID == id);
        }

        public JsonResult LlenarText(int? userid)
        {
            object consulta;

            consulta = from su in _context.SegUsuarios
                       where su.UserID == userid
                       select su;

            
            return Json(consulta);
        }
        public JsonResult GetDatosPrograma(int? programid)
        {
            object consulta;

            consulta = from su in _context.Programs
                       where su.ProgramID == programid
                       select su;

            
            return Json(consulta);
        }
        #endregion

        #region Closed and Reopen Opportunities
        [HttpPost]
        public string ClosedOpportunities(int? id, int iduser, string closedcomment, int reason)
        {
            string response = "";
            DateTime date = DateTime.Now;

            if (id == null)
            {

            }

            string conString = Microsoft
                               .Extensions
                               .Configuration
                               .ConfigurationExtensions
                               .GetConnectionString(this.Configuration, "dbAdminDatabase");

            string Command = "update Opportunities set Closed = 1, ClosedDate = @Date, ClosedBy = @User, ClosedComment = @Comment, ClosingReasonID = @Reason where ID = @Id";

            using (var sqlConnection = new SqlConnection(conString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand(Command, sqlConnection);

                query.Parameters.AddWithValue("@Id", id);
                query.Parameters.AddWithValue("@User", iduser);
                query.Parameters.AddWithValue("@Comment", closedcomment);
                query.Parameters.AddWithValue("@Date", date);
                query.Parameters.AddWithValue("@Reason", reason);


                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();

                sqlConnection.Close();


                if (result == 1)
                {
                    response = "Exito";
                }
                else
                {
                    response = "Facasamos";
                }

            }
            
            return response;

        }

        //Reopen Opportunities
        [HttpPost, ActionName("ReOpenOpportunities")]

        public async Task<string> ReOpenOpportunities(int? id, int iduser, string reopencomment)
        {
            string response = "";
            DateTime date = DateTime.Now;

            if (id == null)
            {

            }

            OpportunitiesReopenDetails opportunitiesReopen = new OpportunitiesReopenDetails();
            var opportunities = await _context.Opportunities.FindAsync(id);
            if (opportunities == null)
            {

            }

            opportunitiesReopen.ReOpenBy = iduser;
            opportunitiesReopen.ReOpenComment = reopencomment;
            opportunitiesReopen.ReOpenDate = date;

            opportunitiesReopen.OpportunitiesID = opportunities.ID;
            opportunitiesReopen.ClosedComment = opportunities.ClosedComment;
            opportunitiesReopen.ClosedDate = Convert.ToDateTime(opportunities.ClosedDate);
            opportunitiesReopen.ClosedBy = Convert.ToInt32(opportunities.ClosedBy);

            _context.Add(opportunitiesReopen);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                bool status = false;
                opportunities.Closed = status;
                opportunities.ClosedBy = null;
                opportunities.ClosedComment = null;
                opportunities.ClosedDate = null;

                _context.Update(opportunities);
                var resultado = await _context.SaveChangesAsync();
                if (resultado > 0)
                {
                    response = "Exito";
                }
            }
            else
            {
                response = "Facasamos";
            }

            return response;

        }
        #endregion

        #region opportunitiesDetails
        // GET: OpportunitiesDetails
        public async Task<IActionResult> IndexOpportunitiesDetails()
        {
            //return View(await _context.OpportunitiesDetails.ToListAsync());
            var dBAdminContext = _context.OpportunitiesDetails.Include(c => c.Opportunities);

            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;


            return View(await dBAdminContext.ToListAsync());
        }

        // GET: OpportunitiesDetails/Details/5
        public async Task<IActionResult> DetailsOpportunitiesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunitiesDetails = await _context.OpportunitiesDetails
                .FirstOrDefaultAsync(m => m.DetailID == id);
            if (opportunitiesDetails == null)
            {
                return NotFound();
            }
            
            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;


            return View(opportunitiesDetails);
        }

        // GET: OpportunitiesDetails/Create
        public async Task<IActionResult> CreateOpportunitiesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var opportunities = await _context.Opportunities.FindAsync(id);
            if (opportunities == null)
            {
                return NotFound();
            }

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = _context.AspNetUsers.Single(m => m.Email == Email);
            ViewBag.id = User.UserID;// HttpContext.Session.GetInt32("UserID");
            ViewBag.OpenedBy = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario", ViewBag.id);

            var view = new OpportunitiesDetails { OpportunitiesID = opportunities.ID };
            view.UserID = ViewBag.id;
            view.VisitDate =  DateTime.Now;


            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;

            return View(view);
        }

        // POST: OpportunitiesDetails/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOpportunitiesDetails(OpportunitiesDetails opportunitiesDetails)
        {

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = _context.AspNetUsers.Single(m => m.Email == Email);
            var idu = User.UserID;

            opportunitiesDetails.CreatedDate = DateTime.Now;
            opportunitiesDetails.DetailDatetime= DateTime.Now;
            opportunitiesDetails.UserID = idu;


            if (ModelState.IsValid)
            {
                _context.Add(opportunitiesDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", opportunitiesDetails.OpportunitiesID));
                //return RedirectToAction(nameof(Index));
            }
            return View(opportunitiesDetails);
        }

        // GET: OpportunitiesDetails/Edit/5
        public async Task<IActionResult> EditOpportunitiesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunitiesDetails = await _context.OpportunitiesDetails.FindAsync(id);
            if (opportunitiesDetails == null)
            {
                return NotFound();
            }

            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;


            return View(opportunitiesDetails);
        }

        // POST: OpportunitiesDetails/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOpportunitiesDetails(OpportunitiesDetails opportunitiesDetails)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opportunitiesDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpportunitiesDetailsExists(opportunitiesDetails.DetailID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(opportunitiesDetails);
        }

        // GET: OpportunitiesDetails/Delete/5
        public async Task<IActionResult> DeleteOpportunitiesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunitiesDetails = await _context.OpportunitiesDetails
                .FirstOrDefaultAsync(m => m.DetailID == id);
            if (opportunitiesDetails == null)
            {
                return NotFound();
            }
            
            //le damos acceso a las opciones del menu segun el usuario
            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;


            return View(opportunitiesDetails);
        }

        // POST: OpportunitiesDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedOpportunitiesDetails(int id)
        {
            var opportunitiesDetails = await _context.OpportunitiesDetails.FindAsync(id);
            _context.OpportunitiesDetails.Remove(opportunitiesDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpportunitiesDetailsExists(int id)
        {
            return _context.OpportunitiesDetails.Any(e => e.DetailID == id);
        }

        #endregion

        #region comments

        public async Task<List<InfoSalesComments>> mostarCommentsOppor(string email)
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
                           UserLogeadoNombre = NombreUserLog.NombreUsuario + " " + NombreUserLog.ApellidoUsuario,
                           UserSelect = NombreSelect.UserID,
                           UserSelectNombre = NombreSelect.NombreUsuario + " " + NombreSelect.ApellidoUsuario
                       };


            var con = await join.Where(x => x.SalesId == User.UserID || x.CommentBy == User.UserID).OrderBy(x => x.CommentDatetime).Take(50).ToListAsync();
            

            List<InfoSalesComments> Comments = new List<InfoSalesComments>();

            
            Comments.AddRange(con);


            return Comments;
        }


        public async Task<List<InfoSalesComments>> filtromostarCommentsOppor(string email, int usuarionormal)
        {
            //identificar el usuario seleccionado (aqui el del select)
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.UserID == usuarionormal);

            //identificar usuario logeado
            var lEmail = this.User.FindFirstValue(ClaimTypes.Name);
            var UserLog = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == lEmail);

            
            var NombreUserLog = _context.SegUsuarios.SingleOrDefault(x => x.UserID == UserLog.UserID);
            
            //nombre del usuaio del select
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
                           UserLogeadoNombre = NombreUserLog.NombreUsuario + " " + NombreUserLog.ApellidoUsuario,
                           UserSelect = NombreSelect.UserID,
                           UserSelectNombre = NombreSelect.NombreUsuario + " " + NombreSelect.ApellidoUsuario
                       };


            var con = await join.Where(x => x.SalesId == User.UserID || x.CommentBy == User.UserID ).OrderBy(x => x.CommentDatetime).Take(50).ToListAsync();

            
            List<InfoSalesComments> Comments = new List<InfoSalesComments>();
            
            Comments.AddRange(con);
            
            return Comments;
        }

        public async Task<bool> CrearComments(int idby, int idto, string comment, string title)
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
            }
            else
            {
                Exito = false;
            }


            return Exito;

        }

        public bool getByTo(int by, int to)
        {
            bool resp = false;



            return resp;
        }

        public async Task<List<SelectListItem>> GetSales()
        {
            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == Email);



            List<SelectListItem> admins = new List<SelectListItem>();
            var consul = from x in _context.SegSistemaUsuario
                         join y in _context.SegUsuarios on x.IdUsuario equals y.UserID
                         where x.CodigoSistema == 3
                         select new Administradores { Admin = y.NombreUsuario + " " + y.ApellidoUsuario, AdminID = y.UserID };


            if (admins.Count == 0)
            {
                admins.Add(new SelectListItem()
                {
                    Value = "0",
                    Text = "All"

                });
            }

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
        #endregion


        #region ServerSide Proccessing
        
        public string FiltroLeadOnline (string leadonline)
        {
            if (!string.IsNullOrEmpty(leadonline))
            {
                LeadOnline = leadonline;
            }

            if (leadonline == "reset")
            {
                LeadOnline = "";
            }
            string prueba = LeadOnline;

             return prueba;
        }
       
        public async Task<IActionResult> GetList(string filtro)
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


                //record since the last twelve month for opportunities that is equal to online
                var DateFromOnline = DateTime.Today.AddMonths(-12);
                // record since the last three month for opportunities that is not equal to online
                var DateFromOthers = DateTime.Today.AddMonths(-3);
                //var lastmonth = new DateTime(today.Year, today.Month - 3,today.Day);

                var today = DateFromOthers;
                var todayOnline = DateFromOnline;

                string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
                string Email = this.User.FindFirstValue(ClaimTypes.Name);
                var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == Email);
                if (User == null)
                {
                    return NotFound();
                }
                Int64 IDUser = User.UserID;

                // si es de Sales y Admin
                var segsistemausuario = from x in _context.SegSistemaUsuario
                                        where x.IdUsuario == IDUser &&
                                          x.CodigoSistema == 3 && x.CodigoPerfil == 1
                                        select x;


                // si es solo Sales
                var segsistemausuarionormal = from x in _context.SegSistemaUsuario
                                              where x.IdUsuario == IDUser &&
                                                x.CodigoSistema == 3
                                              select x;

                var normal = segsistemausuarionormal.Count();

                var admin = segsistemausuario.Count();

                var customerData = from oppor in _context.Opportunities
                                   where oppor.VisitedDate >= today
                                   select oppor;
                //Getting all  data
                if (admin == 1)
                {
                    customerData = from oppor in _context.Opportunities
                                   where (oppor.VisitedDate >= todayOnline && oppor.HowFoundID == 10) || (oppor.VisitedDate >= DateFromOthers && oppor.HowFoundID != 10)
                                   orderby oppor.ID descending
                                   select oppor;
                } else
                {
                    if (normal == 1)
                    {
                        customerData = from oppor in _context.Opportunities
                                       where ((oppor.VisitedDate >= todayOnline && oppor.HowFoundID == 10) || (oppor.VisitedDate >= DateFromOthers && oppor.HowFoundID != 10)) && oppor.UserID == IDUser
                                       //where (oppor.VisitedDate >= today && oppor.UserID == IDUser)
                                       orderby oppor.ID descending
                                       select oppor;
                    }
                }
                
                
                //recibiendo parametros de busqueda abvanced
                int user = 0;
                var userid = Request.Form["columns[4][search][value]"].FirstOrDefault();
                if (userid == "")
                {

                }
                else
                {
                    user = int.Parse(userid);
                }
                int category = 0;
                var categoryid = Request.Form["columns[5][search][value]"].FirstOrDefault();
                if (categoryid == "")
                {

                }
                else
                {
                    category = int.Parse(categoryid);
                }
                int howfound = 0;
                var howfoundid = Request.Form["columns[6][search][value]"].FirstOrDefault();
                if (howfoundid == "")
                {

                }
                else
                {
                    howfound = int.Parse(howfoundid);
                }

                bool status = false;
                //inicializar con un numero diferente de 0 y 1
                int memostatus = 10;
                var statusid = Request.Form["columns[7][search][value]"].FirstOrDefault();
                if (statusid == "")
                {

                }
                if (statusid == "0")
                {
                    memostatus = 0;
                    status = false;
                }
                if (statusid == "1")
                {
                    memostatus = 1;
                    status = true;
                }
               
                string rating = Request.Form["columns[8][search][value]"].FirstOrDefault();
                int reason = 0;
                //inicializar con un numero diferente de 0 y 1
                int memoreason = 0;
                var reasonid = Request.Form["columns[10][search][value]"].FirstOrDefault();
                string closingreason = reasonid.ToString();
                
                if (!string.IsNullOrEmpty(closingreason))
                {
                    reason = Int32.Parse(closingreason);
                    memoreason = reason;
                }

                
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.OwnerName.ToLower().Contains(searchValue.ToLower()) ||
                    m.CompanyName.ToLower().Contains(searchValue.ToLower()) || m.NumberLeadToFollowUp.Contains(searchValue.ToLower()));
                }
                if (!string.IsNullOrEmpty(Request.Form["columns[4][search][value]"]))
                {
                    customerData = customerData.Where(m => m.UserID == user);
                }
                if (!string.IsNullOrEmpty(Request.Form["columns[5][search][value]"]))
                {
                    customerData = customerData.Where(m => m.CategoryID == category);
                }
                if (!string.IsNullOrEmpty(Request.Form["columns[6][search][value]"]))
                {
                    customerData = customerData.Where(m => m.HowFoundID == howfound);

                    
                    //filtro LeadOnline
                    List<Opportunities> lista = new List<Opportunities>();
                   // List<int> IDS = new List<int>();
                    lista = customerData.ToList();
                    if (!string.IsNullOrEmpty(LeadOnline))
                    {
                        var F = _context.OppVerfyLeadOnlineTmp.FromSql($"SELECT Opportunities.ID as Idoppor, dbo.Opp_verifyLeadOnlineTmp(Opportunities.ID, 1) as F1, dbo.Opp_verifyLeadOnlineTmp(Opportunities.ID, 2) as F2, dbo.Opp_verifyLeadOnlineTmp(Opportunities.ID, 3) as F3, dbo.Opp_verifyLeadOnlineTmp(Opportunities.ID, 4) as F4, dbo.Opp_verifyLeadOnlineTmp(Opportunities.ID, 5) as F5 from Opportunities Where Opportunities.VisitedDate >= {DateFromOnline} and Opportunities.HowFoundID = 10 order by Opportunities.ID desc").ToList();

                        if (LeadOnline == "F0")
                        {

                            string dato = "00";
                            var ff = F.Where(y => y.F1 == dato);

                            var sisi = customerData.Join(ff, x => x.ID, y => y.Idoppor, (x, y) => x);
                            customerData = sisi;


                        }
                        //f1
                        if (LeadOnline == "F1")
                        {
                            
                            string dato = "00";
                            var ff =  F.Where(y => y.F1 != dato && y.F2 == dato);
                            
                            var sisi  =  customerData.Join(ff, x => x.ID, y => y.Idoppor, (x, y) => x);
                            customerData = sisi;
                            

                        }

                        //f2
                        if (LeadOnline == "F2")
                        {

                            string dato = "00";
                            var ff = F.Where(y => y.F2 != dato && y.F3 == dato);

                            var sisi = customerData.Join(ff, x => x.ID, y => y.Idoppor, (x, y) => x);
                            customerData = sisi;

                        }

                        //f3
                        if (LeadOnline == "F3")
                        {

                            string dato = "00";
                            var ff = F.Where(y => y.F3 != dato && y.F4 == dato);

                            var sisi = customerData.Join(ff, x => x.ID, y => y.Idoppor, (x, y) => x);
                            customerData = sisi;

                        }

                        //f4
                        if (LeadOnline == "F4")
                        {

                            string dato = "00";
                            var ff = F.Where(y => y.F4 != dato && y.F5 == dato);

                            var sisi = customerData.Join(ff, x => x.ID, y => y.Idoppor, (x, y) => x);
                            customerData = sisi;

                        }

                        //f5
                        if (LeadOnline == "F5")
                        {

                            string dato = "00";
                            var ff = F.Where(y => y.F5 != dato);

                            var sisi = customerData.Join(ff, x => x.ID, y => y.Idoppor, (x, y) => x);
                            customerData = sisi;

                        }
                      
                    }

                 
                }
                if (!string.IsNullOrEmpty(Request.Form["columns[7][search][value]"]))
                {
                    customerData = customerData.Where(m => m.Closed == status);
                }
                if (!string.IsNullOrEmpty(Request.Form["columns[8][search][value]"]))
                {
                    customerData = customerData.Where(m => m.Rating == rating);
                }

                if (!string.IsNullOrEmpty(Request.Form["columns[10][search][value]"]) && reason != 0)
                {
                    customerData = customerData.Where(m => m.ClosingReasonID == reason);
                }
                
                //total number of rows count   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data, us = user, ca = category, how = howfound,
                sta = memostatus, ra = rating, rea = memoreason});

            }
            catch (Exception)
            {
                throw;
            }


        }
        #endregion

        



    }
}
