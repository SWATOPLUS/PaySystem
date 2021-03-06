﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaySystem.Data;
using PaySystem.Models.BusinessModels;

namespace PaySystem.Controllers
{
    [Authorize(Roles = "acc")]
    public class WorkLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkLogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WorkLog.Include(w => w.Card).Include(x=> x.Card.Worker);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WorkLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workLog = await _context.WorkLog
                .Include(w => w.Card)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (workLog == null)
            {
                return NotFound();
            }

            return View(workLog);
        }

        // GET: WorkLogs/Create
        public IActionResult Create()
        {
            ViewData["CardId"] = new SelectList(_context.Card.Include(x => x.Worker).Select(x => new KeyValuePair<int, string>(x.Id, x.Worker.Name)), "Key", "Value");
            return View();
        }

        // POST: WorkLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CardId,Date,HoursWorked")] WorkLog workLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CardId"] = new SelectList(_context.Card.Include(x => x.Worker).Select(x => new KeyValuePair<int, string>(x.Id, x.Worker.Name)), "Key", "Value", workLog.CardId);
            return View(workLog);
        }

        // GET: WorkLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workLog = await _context.WorkLog.SingleOrDefaultAsync(m => m.Id == id);
            if (workLog == null)
            {
                return NotFound();
            }
            ViewData["CardId"] = new SelectList(_context.Card.Include(x=> x.Worker).Select(x=> new KeyValuePair<int, string>(x.Id, x.Worker.Name)), "Key", "Value", workLog.CardId);
            return View(workLog);
        }

        // POST: WorkLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CardId,Date,HoursWorked")] WorkLog workLog)
        {
            if (id != workLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkLogExists(workLog.Id))
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
            ViewData["CardId"] = new SelectList(_context.Card.Include(x => x.Worker).Select(x => new KeyValuePair<int, string>(x.Id, x.Worker.Name)), "Key", "Value", workLog.CardId);
            return View(workLog);
        }

        // GET: WorkLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workLog = await _context.WorkLog
                .Include(w => w.Card)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (workLog == null)
            {
                return NotFound();
            }

            return View(workLog);
        }

        // POST: WorkLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workLog = await _context.WorkLog.SingleOrDefaultAsync(m => m.Id == id);
            _context.WorkLog.Remove(workLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkLogExists(int id)
        {
            return _context.WorkLog.Any(e => e.Id == id);
        }
    }
}
