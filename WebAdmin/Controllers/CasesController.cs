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
using X.PagedList;


namespace WebAdmin.Controllers
{
    public class CasesController : Controller
    {
        private readonly DBAdminContext _context;

        public static bool IsTechSupport { get; set; }
       
       public static int? Pagina { get; set; }
        

        public CasesController(DBAdminContext context)
        {
            _context = context;
           
        }
     

        #region Cases
        // GET: Cases
        public async Task<IActionResult> Index(string sorOrder,
            string currentFilter, string searchString, int? page)
        {
            if (page != null)
            {
                Pagina = page;
            }

            ViewBag.CurrentSort = sorOrder;
            //validando si nuestro parametro sororder es nulo o esta vacio
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sorOrder) ? "nombre_desc" : "";
            ViewData["DescripcionSortParm"] = sorOrder == "descripcion_asc" ? "descripcion_desc" : "descripcion_asc";
            
            var lEmail = this.User.FindFirstValue(ClaimTypes.Name);

            ViewBag.User = lEmail; // HttpContext.Session.GetString("Email");
            if (lEmail == null)
            {
                //return RedirectToAction("Login", "SegUsuarios");
                return RedirectToAction("../Identity/Account/Login");
                //return RedirectToAction("Index", "Login");
            }

           // var TechFiles = _context.Techfiles.ToList();

            var StatusFilter = "Active";
            var dBAdminContext = _context.Cases
                .Where(c => c.Status == StatusFilter)
                .Include(c => c.AssignedToNavigation)
                .Include(c => c.Company)
                .Include(c => c.Location)
                .Include(c => c.CasesDetails);


            


            //var dBAdminContext = _context.Cases
            //    .OrderByDescending(c => c.CasesID )
            //    .Include(c => c.AssignedToNavigation)
            //    .Include(c => c.Company)
            //    .Include(c => c.Location)
            //    .Include(c => c.CasesDetails);

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Email == Email);
            if (User == null)
            {
                return NotFound();
            }
            Int64 IDUser = User.UserID;

            //var UserData = FindDataUser(User.UserID);

            //HttpContext.Session.SetInt32("UserID", User.UserID);
            //HttpContext.Session.SetString("User", Email);

            ViewBag.id = User.UserID;// HttpContext.Session.GetInt32("UserID");

            ViewBag.DDlCompanies = new SelectList(_context.SalesCompany, "CompanyId", "CompanyName");
            ViewBag.DDLUsers = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario");

            //comprobando que cumple los requisitos
            //Si el usuario es de Technical Support
            var segsistemausuario = from x in _context.SegSistemaUsuario
                                    where x.IdUsuario == IDUser &&
                                      x.CodigoSistema == 2
                                    select x;
            
            
            var admin = segsistemausuario.Count();
            
            //si encontro un usuario Significa que es de Technical Support por logica si tiene Acceso A Cases
            if (admin == 1)
            {
                //guardamos si es de technical
                IsTechSupport = true;


                int pageSize = 10;



              
                //page ?? quiere decir que devolver el valor de page si tiene un valor o devolver 1 si es nulo
               return View(await Paginacion<Cases>.CreateAsync(dBAdminContext.AsNoTracking(), Pagina ?? 1, pageSize));
                // return View(await dBAdminContext.ToListAsync());
            }
            else
            {
               
                IsTechSupport = false;
                return RedirectToAction("../Home/Privacy");
                // return NotFound();
            }
        }



        // GET: Cases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cases = await _context.Cases
                .Include(c => c.AssignedToNavigation)
                .Include(c => c.Company)
                .Include(c => c.Location)
                .Include(c => c.CasesDetails)
                .FirstOrDefaultAsync(m => m.CasesID == id);
            if (cases == null)
            {
                return NotFound();
            }

            // IActionResult x = await getDetailsc(id);
            //var cases = await _context.Cases.FindAsync(id);
            //if (cases == null)
            //{
            //    return NotFound();
            //}
            ViewBag.Newdetails = new CasesDetails();
            ViewBag.DDLdetails = _context.CasesDetails.Where(x => x.CasesID == cases.CasesID);
            ViewBag.DDlCompanies = new SelectList(_context.SalesCompany, "CompanyId", "CompanyName");
            ViewBag.DDLUsers = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario");

            //string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            //var User = _context.AspNetUsers.Single(m => m.Email == Email);

           // ViewBag.id = User.UserID;// HttpContext.Session.GetInt32("UserID");
            ViewBag.User = Email; // HttpContext.Session.GetString("User");
           // ViewBag.User = HttpContext.Session.GetString("User");
            if (ViewBag.User == null)
            {
                return RedirectToAction("../Identity/Account/Login");
            }
            return View(cases);


          
        }

        [HttpPost]
        public ActionResult Details(int? id, string action)
        {
        if (action == "NewDetail")
            {
                // Si no ha pasado nuestra validación, mostramos el mensaje personalizado de error
            }
            return View();
        }

        // GET: Cases/Create
        public IActionResult Create()

       {

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User =  _context.AspNetUsers.Single(m => m.Email == Email);
            if (User == null)
            {
                return NotFound();
            }
   
           //cases.OpenedDate = DateTime.Now;
           Cases model = new Cases();
            model.OpenedDate= DateTime.Now;

            // Pass your model to this View
          
            ViewBag.id = User.UserID; //HttpContext.Session.GetInt32("UserID");
            ViewBag.User = Email;//this.User.FindFirstValue(ClaimTypes.Name);  //HttpContext.Session.GetString("User");

            ViewData["AssignedTo"] = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario", ViewBag.id);
            ViewData["CompanyId"] = new SelectList(_context.SalesCompany, "CompanyId", "CompanyName", "Selected");
            ViewData["LocationId"] = new SelectList(_context.SalesLocations, "LocationId", "Address", "Select Item");
            ViewData["OpenedBy"] = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario", ViewBag.id);
            ViewData["Category"] = new SelectList(_context.CasesCategories, "Description", "Description", "Select Item");
            ViewData["Priority"] = new SelectList(_context.CasesPriority, "Description", "Description", "Select Item");
            ViewData["Status"] = new SelectList(_context.CasesStatus, "Description", "Description", "Select Item");
            ViewData["UserID"] = new SelectList(_context.SegUsuarios.OrderBy(x => x.Login), "UserID", "Login");

            if (ViewBag.User == null)
            {
                return RedirectToAction("../Identity/Account/Login");
            }
            Int64 IDUser = User.UserID;

            //comprobando que cumple los requisitos
            //Si el usuario es de Technical Support
            var segsistemausuario = from x in _context.SegSistemaUsuario
                                    where x.IdUsuario == IDUser &&
                                      x.CodigoSistema == 2
                                    select x;


            var admin = segsistemausuario.Count();


            if (admin != 1)
            {
                return RedirectToAction("../Home/Privacy");
            }


            return View(model);
        }

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cases cases, string Comment, string SelectStatus)
        {
            CasesDetails detail = new CasesDetails();
            if (ModelState.IsValid)
            {
                cases.OpenedDate = DateTime.Now;
                cases.TypeId = "1";
                cases.Web = "1";
                _context.Add(cases);

                var resul =  await _context.SaveChangesAsync();

                if (resul>0 && !string.IsNullOrEmpty(Comment)  && !string.IsNullOrEmpty(SelectStatus))
                {
                    var id = cases.CasesID;
                    detail.CasesID = id;
                    detail.Comment = Comment;
                    detail.Status = SelectStatus;
                    detail.DetailDatetime = DateTime.Now;
                    detail.UserID = (int)cases.OpenedBy;

                    _context.Add(detail);

                    //send mail
                    await _context.SaveChangesAsync();

                    sendMail(cases.CasesID);

                    return RedirectToAction(nameof(Index));
                }

             
             return RedirectToAction(nameof(Index));
            }
            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = _context.AspNetUsers.Single(m => m.Email == Email);

            ViewBag.id = User.UserID;// HttpContext.Session.GetInt32("UserID");
            ViewBag.User = Email;//HttpContext.Session.GetString("User");
            ViewData["AssignedTo"] = new SelectList(_context.SegUsuarios, "UserID", "UserID", cases.AssignedTo);
            ViewData["CompanyId"] = new SelectList(_context.SalesCompany, "CompanyId", "CompanyId", cases.CompanyId);
            ViewData["LocationId"] = new SelectList(_context.SalesLocations, "LocationId", "LocationId", cases.LocationId);

            return View(cases);
        }

        // GET: Cases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cases = await _context.Cases.FindAsync(id);
            if (cases == null)
            {
                return NotFound();
            }

            ViewData["AssignedTo"] = new SelectList(_context.SegUsuarios, "UserID", "Login", cases.AssignedTo);
            ViewData["CompanyId"] = new SelectList(_context.SalesCompany, "CompanyId", "CompanyName", cases.CompanyId);
            ViewData["LocationId"] = new SelectList(_context.SalesLocations, "LocationId", "DbaName", cases.LocationId);
            ViewData["OpenedBy"] = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario", cases.OpenedBy);
            ViewData["Category"] = new SelectList(_context.CasesCategories, "Description", "Description", cases.Category);
            ViewData["Priority"] = new SelectList(_context.CasesPriority, "Description", "Description", cases.Priority);
            ViewData["Status"] = new SelectList(_context.CasesStatus, "Description", "Description", cases.Status);

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = _context.AspNetUsers.Single(m => m.Email == Email);

            ViewBag.id = User.UserID;// HttpContext.Session.GetInt32("UserID");
            ViewBag.User = Email; // HttpContext.Session.GetString("User");
            if (ViewBag.User == null)
            {
                //return RedirectToAction("Login", "SegUsuarios");
                return RedirectToAction("../Identity/Account/Login");
            }
            return View(cases);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( Cases cases, int pagina)
        {
            //if (id != cases.CasesID)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cases);
                    await _context.SaveChangesAsync();

                    //_context.Database.ExecuteSqlCommand("dbo.[SendEmail] @int_CaseID", cases.CasesID);
                    //var id = cases.CasesID;
                    ////send mail
                    //_context.Database.ExecuteSqlCommand("dbo.SendEmail @p0", id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasesExists(cases.CasesID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof (Index));
            }
            ViewData["AssignedTo"] = new SelectList(_context.SegUsuarios, "UserID", "Login", cases.AssignedTo);
            ViewData["CompanyId"] = new SelectList(_context.SalesCompany, "CompanyId", "CompanyName", cases.CompanyId);
            ViewData["LocationId"] = new SelectList(_context.SalesLocations, "LocationId", "LocationId", cases.LocationId);
            if (ViewBag.User == null)
            {
                //return RedirectToAction("Login", "SegUsuarios");
                return RedirectToAction("../Identity/Account/Login");
            }
            return View(cases);
        }

        // GET: Cases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            //var User = _context.AspNetUsers.Single(m => m.Email == Email);

            ViewBag.User = Email;// HttpContext.Session.GetString("User");
            if (id == null)
            {
                return NotFound();
            }

            var cases = await _context.Cases
                .Include(c => c.AssignedToNavigation)
                .Include(c => c.Company)
                .Include(c => c.Location)
                .FirstOrDefaultAsync(m => m.CasesID == id);
            if (cases == null)
            {
                return NotFound();
            }
            if (ViewBag.User == null)
            {
                //return RedirectToAction("Login", "SegUsuarios");
                return RedirectToAction("../Identity/Account/Login");
            }
            return View(cases);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cases = await _context.Cases.FindAsync(id);
            _context.Cases.Remove(cases);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasesExists(int? id)
        {
            return _context.Cases.Any(e => e.CasesID == id);
        }


        public async Task<IActionResult> getDetailsc(int? id)
        {

            var cases = await _context.Cases.FindAsync(id);
            if (cases == null)
            {
                return NotFound();
            }
            var view = _context.CasesDetails.Where(x => x.CasesID == cases.CasesID);  

            return View(view);


        }

  
        #endregion

        #region CasesDetailsController

        // GET: CasesDetails
        public async Task<IActionResult> IndexCasesDetails()
        {
            var dBAdminContext = _context.CasesDetails.Include(c => c.Cases).Include(c => c.SegUsuarios);
            return View(await dBAdminContext.ToListAsync());
        }

        // GET: CasesDetails/Details/5
        public async Task<IActionResult> DetailsCasesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casesDetails = await _context.CasesDetails
                .Include(c => c.Cases)
                .Include(c => c.SegUsuarios)
                .FirstOrDefaultAsync(m => m.DetailId == id);
            if (casesDetails == null)
            {
                return NotFound();
            }

   

            return View(casesDetails);
        }

        // GET: CasesDetails/Create
        public async Task<IActionResult> CreateCasesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cases = await _context.Cases.FindAsync(id);
            if (cases == null)
            {
                return NotFound();
            }

            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = _context.AspNetUsers.Single(m => m.Email == Email);

            ViewBag.id = User.UserID;// HttpContext.Session.GetInt32("UserID");
            ViewBag.User = Email; // HttpContext.Session.GetString("User");          
            //ViewData["OpenedBy"] = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario", ViewBag.id);
            //ViewData["UserID"] = new SelectList(_context.SegUsuarios.OrderBy(x=>x.Login), "UserID", "Login");

            var view = new CasesDetails { CasesID = cases.CasesID };
            //ViewBag.id = HttpContext.Session.GetInt32("UserID");
            //ViewBag.User = HttpContext.Session.GetString("User");
            ViewBag.OpenedBy = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario", ViewBag.id);

            view.UserID = ViewBag.id;

            return View(view);
        }

        // POST: CasesDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCasesDetails(CasesDetails casesDetails)
        {
            casesDetails.DetailDatetime = DateTime.Now;
            string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            var User = _context.AspNetUsers.Single(m => m.Email == Email);


            var idu = User.UserID;// (int)HttpContext.Session.GetInt32("UserID");
            casesDetails.UserID = idu;
            if (ModelState.IsValid)
            {
                _context.Add(casesDetails);
                await _context.SaveChangesAsync();
            
                sendMail(casesDetails.CasesID);
    

            return RedirectToAction(string.Format("Details/{0}", casesDetails.CasesID));
            }
            ViewData["UserID"] = new SelectList(_context.SegUsuarios, "Login", "UserID");
            return View(casesDetails);
        }

        public virtual int sendMail(int casesID)
        {
            return _context.Database.ExecuteSqlCommand("SendEmail @p0", casesID);
        }


        // GET: CasesDetails/Edit/5
        public async Task<IActionResult> EditCasesDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casesDetails = await _context.CasesDetails.FindAsync(id);
            if (casesDetails == null)
            {
                return NotFound();
            }
            ViewData["CasesID"] = new SelectList(_context.Cases, "Id", "Id", casesDetails.CasesID);
            ViewData["UserID"] = new SelectList(_context.SegUsuarios, "Login", "UserID", casesDetails.UserID);
            return View(casesDetails);
        }

        // POST: CasesDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCasesDetails(int id, [Bind("DetailId,CasesID,DetailDatetime,Comment,UserID,Status,Suggestion,NormalRow")] CasesDetails casesDetails)
        {
            if (id != casesDetails.DetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(casesDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasesDetailsExists(casesDetails.DetailId))
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
            ViewData["CasesID"] = new SelectList(_context.Cases, "Id", "Id", casesDetails.CasesID);
            ViewData["UserID"] = new SelectList(_context.SegUsuarios, "UserID", "UserID", casesDetails.UserID);
            return View(casesDetails);
        }

        // GET: CasesDetails/Delete/5
        public async Task<IActionResult> DeleteCasesDetails(int? id)
        {

            //string Id_of_AspNetUser = _clasess.ExtensionMethods.getUserId(this.User);
            string Email = this.User.FindFirstValue(ClaimTypes.Name);
            //var User = _context.AspNetUsers.Single(m => m.Email == Email);

            //ViewBag.id = User.UserID;// HttpContext.Session.GetInt32("UserID");
            ViewBag.User = Email; // HttpContext.Session.GetString("User");

            //ViewBag.User = HttpContext.Session.GetString("User");
            if (id == null)
            {
                return NotFound();
            }

            var casesDetails = await _context.CasesDetails
                .Include(c => c.Cases)
                .Include(c => c.SegUsuarios)
                .FirstOrDefaultAsync(m => m.DetailId == id);
            if (casesDetails == null)
            {
                return NotFound();
            }

            return View(casesDetails);
        }

        // POST: CasesDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedCasesDetails(int id)
        {
            var casesDetails = await _context.CasesDetails.FindAsync(id);
            _context.CasesDetails.Remove(casesDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasesDetailsExists(int id)
        {
            return _context.CasesDetails.Any(e => e.DetailId == id);
        }

        #endregion

        #region Json
        public JsonResult LlenarSelect(int? companyid)
        {
            object consulta;

             consulta = from s in _context.SalesLocations
                               where s.CompanyId == companyid
                               select s;
           
         
            //var list = consulta.ToArray();
            return Json(consulta);
        }

        public JsonResult LlenarText(int? userid)
        {
            object consulta;

            consulta = from su in _context.SegUsuarios
                       where su.UserID == userid
                       select su;


            //var list = consulta.ToArray();
            return Json(consulta);
        }
        #endregion
        //public JsonResult FindDataUser2(int? UserID)
        //{
        //    object consulta;

        //    consulta = from s in _context.SegUsuarios
        //               where s.UserID == UserID
        //               select s;


        //    //var list = consulta.ToArray();
        //    return Json(consulta);
        //}


        //public JsonResult FindDataUser(int? UserID)
        //{
        //    return Json(FindDataUser2(UserID));
        //}


        //public List<SegUsuarios> FindDataUser2(int? pUserID)
        //{
        //        var productos = _context.SegUsuarios.OrderBy(x => x.Login)
        //                               .Where(x => x.UserID == pUserID).ToList();
        //        return productos;

        //}

        //[HttpPost]
        //public ActionResult GetUserData(int UserID)
        //{
        //    return Json(FindDataUser2(2));

        //}
        public async Task<IActionResult> IndexTechFiles()
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

            return View(await _context.Techfiles.ToListAsync());
        }

    }
}
