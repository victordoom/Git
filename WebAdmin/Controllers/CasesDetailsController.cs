using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class CasesDetailsController : Controller
    {
        private readonly DBAdminContext _context;

        public CasesDetailsController(DBAdminContext context)
        {
            _context = context;
        }

        // GET: CasesDetails
        public async Task<IActionResult> Index()
        {
            ViewBag.id = HttpContext.Session.GetInt32("UserID");
            ViewBag.User = HttpContext.Session.GetString("User");
            var dBAdminContext = _context.CasesDetails.Include(c => c.Cases).Include(c => c.SegUsuarios);
            if (ViewBag.User == null)
            {
                //return RedirectToAction("Login", "SegUsuarios");
                return RedirectToPage("./Login");
            }
            return View(await dBAdminContext.ToListAsync());
        }

        // GET: CasesDetails/Details/5
        public async Task<IActionResult> Details(int? id)
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
            ViewBag.id = HttpContext.Session.GetInt32("UserID");
            ViewBag.User = HttpContext.Session.GetString("User");
            return View(casesDetails);
        }

        // GET: CasesDetails/Create
        public IActionResult Create()
        {
            ViewBag.id = HttpContext.Session.GetInt32("UserID");
            ViewBag.User = HttpContext.Session.GetString("User");
            ViewData["CasesID"] = new SelectList(_context.Cases, "Id", "Id");
            ViewData["UserID"] = new SelectList(_context.SegUsuarios, "UserID", "UserID");
            return View();
        }

        // POST: CasesDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetailId,CasesID,DetailDatetime,Comment,UserID,Status,Suggestion,NormalRow")] CasesDetails casesDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(casesDetails);

                await _context.SaveChangesAsync();

                var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@int_CaseID",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = casesDetails.CasesID
                        }};
                var result = _context.Database.ExecuteSqlCommand("[dbo].[SendEmail] @int_CaseID", param);

                return RedirectToAction(nameof(Index));
            }
            ViewData["CasesID"] = new SelectList(_context.Cases, "Id", "Id", casesDetails.CasesID);
            ViewData["UserID"] = new SelectList(_context.SegUsuarios, "UserID", "UserID", casesDetails.UserID);
            return View(casesDetails);
        }

        // GET: CasesDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["UserID"] = new SelectList(_context.SegUsuarios, "UserID", "UserID", casesDetails.UserID);
            return View(casesDetails);
        }

        // POST: CasesDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DetailId,CasesID,DetailDatetime,Comment,UserID,Status,Suggestion,NormalRow")] CasesDetails casesDetails)
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

                    var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@int_CaseID",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = casesDetails.CasesID
                        }};
                    var result = _context.Database.ExecuteSqlCommand("[dbo].[SendEmail] @int_CaseID", param);



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
        public async Task<IActionResult> Delete(int? id)
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

        // POST: CasesDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
    }
}
