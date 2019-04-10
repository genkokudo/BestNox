using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BestNox.Data;
using BestNox.Models;
using Microsoft.AspNetCore.Authorization;

namespace BestNox.Controllers
{
    [Authorize(Roles = SystemConstants.Administrator)]
    public class SystemParametersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SystemParametersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SystemParameters
        public async Task<IActionResult> Index()
        {
            return View(await _context.SystemParameters.Where(d => !d.IsDeleted).OrderBy(d => d.OrderNo).OrderBy(d => d.CategoryId).ToListAsync());
        }

        // GET: SystemParameters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemParameter = await _context.SystemParameters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemParameter == null)
            {
                return NotFound();
            }

            return View(systemParameter);
        }

        // GET: SystemParameters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemParameters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,OrderNo,CurrentValue,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] SystemParameter systemParameter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(systemParameter);
                await _context.SaveChangesAsync(User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(systemParameter);
        }

        // GET: SystemParameters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemParameter = await _context.SystemParameters.FindAsync(id);
            if (systemParameter == null)
            {
                return NotFound();
            }
            return View(systemParameter);
        }

        // POST: SystemParameters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,OrderNo,CurrentValue,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] SystemParameter systemParameter)
        {
            if (id != systemParameter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemParameter);
                    await _context.SaveChangesAsync(User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemParameterExists(systemParameter.Id))
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
            return View(systemParameter);
        }

        // GET: SystemParameters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemParameter = await _context.SystemParameters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemParameter == null)
            {
                return NotFound();
            }

            return View(systemParameter);
        }

        // POST: SystemParameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var systemParameter = await _context.SystemParameters.FindAsync(id);
            _context.SystemParameters.Remove(systemParameter);
            await _context.SaveChangesAsync(User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool SystemParameterExists(int id)
        {
            return _context.SystemParameters.Any(e => e.Id == id);
        }
    }
}
