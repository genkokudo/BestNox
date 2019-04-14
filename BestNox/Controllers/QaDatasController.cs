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
    public class QaDatasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QaDatasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QaDatas
        public async Task<IActionResult> Index()
        {
            // TODO:自分が作成した、または誰かの公開設定。それを優先度降順->更新日時降順で表示
            // TODO:リストボックスで選択した条件で表示
            return View(await _context.QaDatas.ToListAsync());
        }

        // GET: QaDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qaData = await _context.QaDatas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qaData == null)
            {
                return NotFound();
            }

            ViewBag.Question = Markdown.ToHtml(qaData.Question);
            ViewBag.Answer = Markdown.ToHtml(qaData.Answer);
            return View(qaData);
        }

        // GET: QaDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QaDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Question,Answer,IsSolved,RelativeNo,CategoryId,IsPublic,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] QaData qaData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qaData);
                await _context.SaveChangesAsync(User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(qaData);
        }

        // GET: QaDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qaData = await _context.QaDatas.FindAsync(id);
            if (qaData == null)
            {
                return NotFound();
            }

            return View(qaData);
        }

        // POST: QaDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Question,Answer,IsSolved,RelativeNo,CategoryId,IsPublic,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] QaData qaData)
        {
            if (id != qaData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qaData);
                    await _context.SaveChangesAsync(User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QaDataExists(qaData.Id))
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
            return View(qaData);
        }

        // GET: QaDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qaData = await _context.QaDatas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qaData == null)
            {
                return NotFound();
            }

            return View(qaData);
        }

        // POST: QaDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qaData = await _context.QaDatas.FindAsync(id);
            _context.QaDatas.Remove(qaData);
            await _context.SaveChangesAsync(User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool QaDataExists(int id)
        {
            return _context.QaDatas.Any(e => e.Id == id);
        }
    }
}
