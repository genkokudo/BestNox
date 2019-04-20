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
            // 背景色の設定を取得する
            ViewBag.BackList = getKeyValueList(SystemConstants.SystemPatameterMemoBack);

            // 自分が作成した、または誰かの公開設定。それを優先度降順->更新日時降順で表示
            ViewBag.SelectList = getSelectList(SystemConstants.SystemPatameterMemo, 0);
            return View(await _context.QaDatas.Where(d => d.CreatedBy == User.Identity.Name || d.IsPublic).OrderByDescending(d => d.UpdatedDate).OrderBy(d => d.RelativeNo).ToListAsync());
        }

        // GET: QaDatas/1
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            ViewBag.BackList = getKeyValueList(SystemConstants.SystemPatameterMemoBack);
            // リストボックスで選択した条件で表示
            if (categoryId.HasValue)
            {
                ViewBag.SelectList = getSelectList(SystemConstants.SystemPatameterMemo, categoryId.Value);
                return View(await _context.QaDatas.Where(d => d.CategoryId == categoryId).Where(d => d.CreatedBy == User.Identity.Name || d.IsPublic).OrderByDescending(d => d.UpdatedDate).OrderBy(d => d.RelativeNo).ToListAsync());
            }
            return await Index();
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
            if (qaData.Answer != null)
            {
                ViewBag.Answer = Markdown.ToHtml(qaData.Answer);
            }

            ViewBag.SelectList = getSelectList(SystemConstants.SystemPatameterMemo, qaData.CategoryId);
            return View(qaData);
        }

        // GET: QaDatas/Create
        public IActionResult Create()
        {
            ViewBag.SelectList = getSelectList(SystemConstants.SystemPatameterMemo, 0);
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
            ViewBag.SelectList = getSelectList(SystemConstants.SystemPatameterMemo, 0);
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

            ViewBag.SelectList = getSelectList(SystemConstants.SystemPatameterMemo, qaData.CategoryId);
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
            ViewBag.SelectList = getSelectList(SystemConstants.SystemPatameterMemo, qaData.CategoryId);
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

            ViewBag.SelectList = getSelectList(SystemConstants.SystemPatameterMemo, qaData.CategoryId);
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

        #region 共通処理：スーパークラス作成した方が良いかも
        /// <summary>
        /// プルダウンを作成する
        /// </summary>
        /// <param name="category">カテゴリID</param>
        /// <param name="select">初期選択値、ない場合は0</param>
        /// <returns>プルダウンの選択肢リスト</returns>
        private List<SelectListItem> getSelectList(int category, int select)
        {
            var parameters = _context.SystemParameters.Where(p => p.CategoryId == category).OrderBy(p => p.OrderNo).ToList();
            var selectList = new List<SelectListItem>();
            foreach (var item in parameters)
            {
                var selectItem = new SelectListItem(item.Display, item.CurrentValue);
                if(int.Parse(item.CurrentValue) == select)
                {
                    selectItem.Selected = true;
                }
                selectList.Add(selectItem);
            }

            return selectList;
        }

        /// <summary>
        /// 辞書データを作成
        /// </summary>
        /// <param name="category">カテゴリID</param>
        /// <returns>辞書データ</returns>
        private Dictionary<string, string> getKeyValueList(int category)
        {
            var parameters = _context.SystemParameters.Where(p => p.CategoryId == category).ToList();
            var dictionary = new Dictionary<string, string>();

            foreach (var item in parameters)
            {
                dictionary.Add(item.CurrentValue, item.Display);
            }

            return dictionary;
        }
        #endregion
    }
}
