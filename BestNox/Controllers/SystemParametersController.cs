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
using Microsoft.Extensions.DependencyInjection;
using BestNox.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BestNox.Controllers
{
    [Authorize(Roles = SystemConstants.Administrator)]
    public class SystemParametersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly DefaultParameters _defaultParameters = null;

        public SystemParametersController(ApplicationDbContext context, IOptions<DefaultParameters> defaultParameters)
        {
            _context = context;
            _defaultParameters = defaultParameters.Value;
        }

        // GET: SystemParameters
        public async Task<IActionResult> Index()
        {
            var list = await _context.SystemParameters.Where(d => !d.IsDeleted).OrderBy(d => d.OrderNo).OrderBy(d => d.CategoryId).ToListAsync();
            if(list.Count == 0)
            {
                // 初期状態なので、初期化
                foreach (var item in _defaultParameters.DefaultValues)
                {
                    var systemParameter = new SystemParameter();
                    systemParameter.CategoryId = int.Parse(item[0] as string);
                    systemParameter.OrderNo = int.Parse(item[1] as string);
                    systemParameter.CurrentValue = item[2] as string;
                    systemParameter.Display = item[3] as string;
                    _context.Add(systemParameter);
                }
                await _context.SaveChangesAsync(SystemConstants.DefaultParameterUserName);

                // 登録後再検索
                list = await _context.SystemParameters.Where(d => !d.IsDeleted).OrderBy(d => d.OrderNo).OrderBy(d => d.CategoryId).ToListAsync();
            }
            return View(list);
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
        public async Task<IActionResult> Create([Bind("Id,CategoryId,OrderNo,Display,CurrentValue,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] SystemParameter systemParameter)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,OrderNo,Display,CurrentValue,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] SystemParameter systemParameter)
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
