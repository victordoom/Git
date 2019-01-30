using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class OpportunitiesDetailsController : Controller
    {
        private readonly DBAdminContext _context;

        public OpportunitiesDetailsController(DBAdminContext context)
        {
            _context = context;
        }

        // GET: OpportunitiesDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.OpportunitiesDetails.ToListAsync());
        }

        // GET: OpportunitiesDetails/Details/5
        public async Task<IActionResult> Details(int? id)
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
            ViewBag.DDLUsers = new SelectList(_context.SegUsuarios, "UserID", "NombreUsuario");
            return View(opportunitiesDetails);
        }

        // GET: OpportunitiesDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OpportunitiesDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetailID,OpportunitiesID,DetailDatetime,Comment,UserID,Status,VisitDate,NextVisitDate,EmailNotification,EmailSent,CreatedDate")] OpportunitiesDetails opportunitiesDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opportunitiesDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(opportunitiesDetails);
        }

        // GET: OpportunitiesDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            return View(opportunitiesDetails);
        }

        // POST: OpportunitiesDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DetailID,OpportunitiesID,DetailDatetime,Comment,UserID,Status,VisitDate,NextVisitDate,EmailNotification,EmailSent,CreatedDate")] OpportunitiesDetails opportunitiesDetails)
        {
            if (id != opportunitiesDetails.DetailID)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Delete(int? id)
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

            return View(opportunitiesDetails);
        }

        // POST: OpportunitiesDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
    }
}
