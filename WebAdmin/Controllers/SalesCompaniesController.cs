﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class SalesCompaniesController : Controller
    {
        private readonly DBAdminContext _context;

        public SalesCompaniesController(DBAdminContext context)
        {
            _context = context;
        }

        // GET: SalesCompanies
        public async Task<IActionResult> Index()
        {
            //ViewBag.id = HttpContext.Session.GetInt32("UserID");
            //ViewBag.User = HttpContext.Session.GetString("User");
            //if (ViewBag.User == null)
            //{
            //    //return RedirectToAction("Login", "SegUsuarios");
            //    return RedirectToPage("./Login");
            //}
            return View(await _context.SalesCompany.ToListAsync());
        }

        // GET: SalesCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesCompany = await _context.SalesCompany
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (salesCompany == null)
            {
                return NotFound();
            }

            return View(salesCompany);
        }

        // GET: SalesCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalesCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,OwnerId")] SalesCompany salesCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salesCompany);
        }

        // GET: SalesCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesCompany = await _context.SalesCompany.FindAsync(id);
            if (salesCompany == null)
            {
                return NotFound();
            }
            return View(salesCompany);
        }

        // POST: SalesCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,CompanyName,OwnerId")] SalesCompany salesCompany)
        {
            if (id != salesCompany.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesCompanyExists(salesCompany.CompanyId))
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
            return View(salesCompany);
        }

        // GET: SalesCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesCompany = await _context.SalesCompany
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (salesCompany == null)
            {
                return NotFound();
            }

            return View(salesCompany);
        }

        // POST: SalesCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesCompany = await _context.SalesCompany.FindAsync(id);
            _context.SalesCompany.Remove(salesCompany);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesCompanyExists(int id)
        {
            return _context.SalesCompany.Any(e => e.CompanyId == id);
        }
    }
}
