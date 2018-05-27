using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaySystem.Data;
using PaySystem.Models.BusinessModels;

namespace PaySystem.Controllers
{
    public class FeeInfoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeeInfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FeeInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.FeeInfo.ToListAsync());
        }

        // GET: FeeInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feeInfo = await _context.FeeInfo
                .SingleOrDefaultAsync(m => m.Id == id);
            if (feeInfo == null)
            {
                return NotFound();
            }

            return View(feeInfo);
        }

        // GET: FeeInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FeeInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CheckId,Name,Comment,Amount")] FeeInfo feeInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feeInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feeInfo);
        }

        // GET: FeeInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feeInfo = await _context.FeeInfo.SingleOrDefaultAsync(m => m.Id == id);
            if (feeInfo == null)
            {
                return NotFound();
            }
            return View(feeInfo);
        }

        // POST: FeeInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CheckId,Name,Comment,Amount")] FeeInfo feeInfo)
        {
            if (id != feeInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feeInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeeInfoExists(feeInfo.Id))
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
            return View(feeInfo);
        }

        // GET: FeeInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feeInfo = await _context.FeeInfo
                .SingleOrDefaultAsync(m => m.Id == id);
            if (feeInfo == null)
            {
                return NotFound();
            }

            return View(feeInfo);
        }

        // POST: FeeInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feeInfo = await _context.FeeInfo.SingleOrDefaultAsync(m => m.Id == id);
            _context.FeeInfo.Remove(feeInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeeInfoExists(int id)
        {
            return _context.FeeInfo.Any(e => e.Id == id);
        }
    }
}
