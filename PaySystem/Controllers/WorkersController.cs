using System;
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
    [Authorize(Roles = "admin")]
    public class WorkersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ForeignDbContext _foreignDbContext;

        public WorkersController(ApplicationDbContext context, ForeignDbContext foreignDbContext)
        {
            _context = context;
            _foreignDbContext = foreignDbContext;
        }

        // GET: Workers
        public async Task<IActionResult> Index(DateTime? updateDate)
        {
            if (updateDate == null)
            {
                return View(await _context.Worker.ToListAsync());
            }

            var globals = _foreignDbContext.Worker
                .Where(x => x.UpdateTimeDateTime >= updateDate)
                .ToArray();

            ViewBag.UpdateDate = updateDate;

            return View(await _context.Worker.Where(x => globals.Any(g => g.Id == x.Id)).ToListAsync());
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker
                .SingleOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            var locals = _context.Worker.ToArray();

            ViewBag.WorkerId = new SelectList(_foreignDbContext.Worker.Where(x => !locals.Any(w => w.GlobalId == x.Id)).ToArray(), "Id", "Name");

            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GlobalId,Job")] Worker worker)
        {
            var globalWorker = _foreignDbContext.Worker.FirstOrDefault(x => x.Id == worker.GlobalId);

            if (ModelState.IsValid && globalWorker != null && !_context.Worker.Any(x => x.GlobalId == worker.GlobalId))
            {
                worker.DateOfBirth = globalWorker.DateOfBirth;
                worker.Name = globalWorker.Name;
                _context.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var locals = _context.Worker.ToArray();

            ViewData["WorkerId"] = new SelectList(_foreignDbContext.Worker.Where(x => !locals.Any(w => w.GlobalId == x.Id)), "Id", "Name", worker.Id);

            return View(worker);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker.SingleOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            ViewBag.GlobalModel = await _foreignDbContext.Worker.SingleOrDefaultAsync(m => m.Id == worker.GlobalId);

            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Job,DateOfBirth")] Worker worker)
        {
            if (id != worker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var e = _context.Worker.SingleOrDefault(x => x.Id == worker.Id);
                    e.Job = worker.Job;
                    e.Name = worker.Name;
                    e.DateOfBirth = worker.DateOfBirth;

                    _context.Update(e);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.Id))
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

            ViewBag.GlobalModel = await _foreignDbContext.Worker.SingleOrDefaultAsync(m => m.Id == worker.GlobalId);

            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker
                .SingleOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worker = await _context.Worker.SingleOrDefaultAsync(m => m.Id == id);
            _context.Worker.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
            return _context.Worker.Any(e => e.Id == id);
        }
    }
}
