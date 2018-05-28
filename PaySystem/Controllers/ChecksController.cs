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
    [Authorize(Roles = "time")]
    public class ChecksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChecksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Checks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Check.Include(c => c.Worker);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Checks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Check
                .Include(c => c.Worker)
                .Include(c=> c.FeeInfos)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (check == null)
            {
                return NotFound();
            }

            return View(check);
        }

        // GET: Checks/Create
        public IActionResult Create()
        {
            ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name");
            return View();
        }

        // POST: Checks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkerId,Period")] Check check)
        {
            if (ModelState.IsValid)
            {
                var hours = _context.WorkLog
                    .Where(x => check.Period.Date <= x.Date && x.Date < check.Period.Date.AddMonths(1))
                    .Sum(x=> x.HoursWorked);

                var infos = new List<FeeInfo>();

                var card = _context.Card.SingleOrDefault(x=> x.WorkerId == check.WorkerId);

                if (card.IsHourPay)
                {
                    var rate = 100;

                    infos.Add(new FeeInfo
                    {
                        Name = "Hours fee",
                        Amount = hours * rate,
                        Comment = $"{hours} h, {rate} rate"
                    });
                } else
                {
                    var total = GetBusinessTimespanBetween(check.Period.Date, check.Period.Date.AddMonths(1), TimeSpan.FromHours(8), TimeSpan.FromHours(16)).TotalHours;
                    var salary = 16000;
                    var salMul = (decimal)(hours / total);
                    var overMul = 0.0m;

                    if (salMul > 1)
                    {
                        overMul = salMul - 1;
                        salMul = 1;
                    }

                    infos.Add(new FeeInfo
                    {
                        Name = "Salary fee",
                        Amount = salMul * salary,
                        Comment = $"{hours}/{total} h, salary {salary}"
                    });

                    infos.Add(new FeeInfo
                    {
                        Name = "Overtime fee",
                        Amount = overMul * salary,
                        Comment = $"{(decimal)total * overMul} overtimed, salary {salary}"
                    });
                }

                infos.Add(new FeeInfo
                {
                    Name = "Tax",
                    Amount = infos.Sum(x => x.Amount) * 0.1m
                });

                check.FeeInfos = infos;

                _context.Add(check);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name", check.WorkerId);
            return View(check);
        }

        public static TimeSpan GetBusinessTimespanBetween(
    DateTime start, DateTime end,
    TimeSpan workdayStartTime, TimeSpan workdayEndTime,
    List<DateTime> holidays = null)
        {
            if (end < start)
                throw new ArgumentException("start datetime must be before end datetime.");

            // Just create an empty list for easier coding.
            if (holidays == null) holidays = new List<DateTime>();

            if (holidays.Where(x => x.TimeOfDay.Ticks > 0).Any())
                throw new ArgumentException("holidays can not have a TimeOfDay, only the Date.");

            var nonWorkDays = new List<DayOfWeek>() { DayOfWeek.Saturday, DayOfWeek.Sunday };

            var startTime = start.TimeOfDay;

            // If the start time is before the starting hours, set it to the starting hour.
            if (startTime < workdayStartTime) startTime = workdayStartTime;

            var timeBeforeEndOfWorkDay = workdayEndTime - startTime;

            // If it after the end of the day, then this time lapse doesn't count.
            if (timeBeforeEndOfWorkDay.TotalSeconds < 0) timeBeforeEndOfWorkDay = new TimeSpan();
            // If start is during a non work day, it doesn't count.
            if (nonWorkDays.Contains(start.DayOfWeek)) timeBeforeEndOfWorkDay = new TimeSpan();
            else if (holidays.Contains(start.Date)) timeBeforeEndOfWorkDay = new TimeSpan();

            var endTime = end.TimeOfDay;

            // If the end time is after the ending hours, set it to the ending hour.
            if (endTime > workdayEndTime) endTime = workdayEndTime;

            var timeAfterStartOfWorkDay = endTime - workdayStartTime;

            // If it before the start of the day, then this time lapse doesn't count.
            if (timeAfterStartOfWorkDay.TotalSeconds < 0) timeAfterStartOfWorkDay = new TimeSpan();
            // If end is during a non work day, it doesn't count.
            if (nonWorkDays.Contains(end.DayOfWeek)) timeAfterStartOfWorkDay = new TimeSpan();
            else if (holidays.Contains(end.Date)) timeAfterStartOfWorkDay = new TimeSpan();

            // Easy scenario if the times are during the day day.
            if (start.Date.CompareTo(end.Date) == 0)
            {
                if (nonWorkDays.Contains(start.DayOfWeek)) return new TimeSpan();
                else if (holidays.Contains(start.Date)) return new TimeSpan();
                return endTime - startTime;
            }
            else
            {
                var timeBetween = end - start;
                var daysBetween = (int)Math.Floor(timeBetween.TotalDays);
                var dailyWorkSeconds = (int)Math.Floor((workdayEndTime - workdayStartTime).TotalSeconds);

                var businessDaysBetween = 0;

                // Now the fun begins with calculating the actual Business days.
                if (daysBetween > 0)
                {
                    var nextStartDay = start.AddDays(1).Date;
                    var dayBeforeEnd = end.AddDays(-1).Date;
                    for (DateTime d = nextStartDay; d <= dayBeforeEnd; d = d.AddDays(1))
                    {
                        if (nonWorkDays.Contains(d.DayOfWeek)) continue;
                        else if (holidays.Contains(d.Date)) continue;
                        businessDaysBetween++;
                    }
                }

                var dailyWorkSecondsToAdd = dailyWorkSeconds * businessDaysBetween;

                var output = timeBeforeEndOfWorkDay + timeAfterStartOfWorkDay;
                output = output + new TimeSpan(0, 0, dailyWorkSecondsToAdd);

                return output;
            }
        }

        // GET: Checks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Check
                .Include(c => c.Worker)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (check == null)
            {
                return NotFound();
            }

            return View(check);
        }

        // POST: Checks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var check = await _context.Check.SingleOrDefaultAsync(m => m.Id == id);
            _context.Check.Remove(check);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckExists(int id)
        {
            return _context.Check.Any(e => e.Id == id);
        }
    }
}
