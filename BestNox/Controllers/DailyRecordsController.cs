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
            // 投稿ロックかどうか
            var isLocked = ControllerHelper.GetSubmitLocked(_context);
            ViewData[SystemConstants.IsSubmitLocked] = isLocked;

            // 自分が書いたものを、更新日時でソートしてから日付降順でソート(最近の日付が上に来て、その中で更新日時が新しい順になる)
            return View(await _context.DailyRecords.Where(d => d.CreatedBy == User.Identity.Name).OrderByDescending(d => d.UpdatedDate).OrderByDescending(d => d.DocumentDate).ToListAsync());
        }

        // GET: DailyRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // 投稿ロックかどうか
            ViewData[SystemConstants.IsSubmitLocked] = ControllerHelper.GetSubmitLocked(_context);
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

            // 投稿ロックかどうか
            ViewData[SystemConstants.IsSubmitLocked] = ControllerHelper.GetSubmitLocked(_context);

            // マークダウンをhtmlに変換
            ViewBag.Markdown = Markdown.ToHtml(dailyRecord.Detail);
            return View(dailyRecord);
        }

        // GET: DailyRecords/Create
        public IActionResult Create()
        {
            // 投稿ロック判定
            var isLocked = ControllerHelper.GetSubmitLocked(_context);
            if(isLocked == "1")
            {
                return NotFound();
            }
            return View();
        }

        // POST: DailyRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DocumentDate,Title,Detail,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] DailyRecord dailyRecord)
        {
            // 投稿ロック判定
            var isLocked = ControllerHelper.GetSubmitLocked(_context);
            if (isLocked == "1")
            {
                return NotFound();
            }
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
            // 投稿ロック判定
            var isLocked = ControllerHelper.GetSubmitLocked(_context);
            if (isLocked == "1")
            {
                return NotFound();
            }
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
            // 投稿ロック判定
            var isLocked = ControllerHelper.GetSubmitLocked(_context);
            if (isLocked == "1")
            {
                return NotFound();
            }
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
            // 投稿ロック判定
            var isLocked = ControllerHelper.GetSubmitLocked(_context);
            if (isLocked == "1")
            {
                return NotFound();
            }
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
            // 投稿ロック判定
            var isLocked = ControllerHelper.GetSubmitLocked(_context);
            if (isLocked == "1")
            {
                return NotFound();
            }
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
