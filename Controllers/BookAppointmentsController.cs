using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using property_rental_management.Models;

namespace property_rental_management.Controllers
{
    public class BookAppointmentsController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public BookAppointmentsController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        // GET: BookAppointments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Appointments.ToListAsync());
        }

        // GET: BookAppointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookAppointment = await _context.Appointments
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (bookAppointment == null)
            {
                return NotFound();
            }

            return View(bookAppointment);
        }

        // GET: BookAppointments/Create
        public IActionResult Create(String managerId, String tenantID)
        {
            ViewData["tenantID"] = tenantID;
            ViewData["managerID"] = managerId;

            //ViewData["ScheduleId"] = new SelectList(_context.Schedules, "ScheduleId", "WeekDay");

            ViewData["ScheduleId"] = new SelectList(_context.Schedules
                .Select(s => new { s.ScheduleId, DisplayText = $"{s.StartTime} - {s.EndTime}" }),
                "ScheduleId",
                "DisplayText");

            //ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId");
            ViewData["statusId"] = "S1";

            return View();
        }

        // POST: BookAppointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,ManagerId,TenantId,ScheduleId,AppointmentDate,StatusId")] BookAppointment bookAppointment)
        {
            if (ModelState.IsValid)
            {

                if (bookAppointment == null)
                {
                    return NotFound();
                }

                var newAppointment = new Appointment
                {
                    AppointmentId = bookAppointment.AppointmentId,
                    ManagerId = bookAppointment.ManagerId,
                    TenantId = bookAppointment.TenantId,
                    ScheduleId = bookAppointment.ScheduleId,
                    AppointmentDate = bookAppointment.AppointmentDate,
                    StatusId = bookAppointment.StatusId
                };

                _context.Appointments.Add(newAppointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(bookAppointment);
        }

        // GET: BookAppointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookAppointment = await _context.BookAppointment.FindAsync(id);
            if (bookAppointment == null)
            {
                return NotFound();
            }
            return View(bookAppointment);
        }

        // POST: BookAppointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,ManagerId,TenantId,ScheduleId,AppointmentDate,StatusId")] BookAppointment bookAppointment)
        {
            if (id != bookAppointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookAppointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookAppointmentExists(bookAppointment.AppointmentId))
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
            return View(bookAppointment);
        }

        // GET: BookAppointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookAppointment = await _context.BookAppointment
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (bookAppointment == null)
            {
                return NotFound();
            }

            return View(bookAppointment);
        }

        // POST: BookAppointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookAppointment = await _context.BookAppointment.FindAsync(id);
            if (bookAppointment != null)
            {
                _context.BookAppointment.Remove(bookAppointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookAppointmentExists(int id)
        {
            return _context.BookAppointment.Any(e => e.AppointmentId == id);
        }
    }
}
