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
using Markdig;

namespace BestNox.Controllers
{
    [Authorize]
    public class DailyRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DailyRecords
        public async Task<IActionResult> Index()
        {
            // 自分が書いたものを、日付降順でソート
            return View(await _context.DailyRecords.Where(d => d.CreatedBy == User.Identity.Name).OrderByDescending(d => d.DocumentDate).ToListAsync());
        }

        // GET: DailyRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyRecord = await _context.DailyRecords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyRecord == null)
            {
                return NotFound();
            }

            ViewBag.Markdown = Markdown.ToHtml(dailyRecord.Detail);
            return View(dailyRecord);
        }

        // GET: DailyRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DailyRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DocumentDate,Title,Detail,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] DailyRecord dailyRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyRecord);
                await _context.SaveChangesAsync(User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(dailyRecord);
        }

        // GET: DailyRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyRecord = await _context.DailyRecords.FindAsync(id);
            if (dailyRecord == null)
            {
                return NotFound();
            }
            return View(dailyRecord);
        }

        // POST: DailyRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DocumentDate,Title,Detail,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] DailyRecord dailyRecord)
        {
            if (id != dailyRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyRecord);
                    await _context.SaveChangesAsync(User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyRecordExists(dailyRecord.Id))
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
            return View(dailyRecord);
        }

        // GET: DailyRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyRecord = await _context.DailyRecords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyRecord == null)
            {
                return NotFound();
            }

            return View(dailyRecord);
        }

        // POST: DailyRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyRecord = await _context.DailyRecords.FindAsync(id);
            _context.DailyRecords.Remove(dailyRecord);
            await _context.SaveChangesAsync(User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool DailyRecordExists(int id)
        {
            return _context.DailyRecords.Any(e => e.Id == id);
        }
    }
}
