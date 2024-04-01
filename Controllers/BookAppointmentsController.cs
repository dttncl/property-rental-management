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
        public IActionResult Create(String managerId, String tenantID, String propertyID)
        {
            ViewData["managerID"] = managerId;

            if (tenantID != null)
            {
                ViewData["tenantID"] = tenantID;

            } else
            {
                ViewData["tenantIDList"] = new SelectList(_context.Tenants.Select(t => new {
                    TenantID = t.TenantId,
                    FullNameWithEmail = $"{t.Email} [{t.FirstName} {t.LastName}]"
                }), "TenantID", "FullNameWithEmail");
            }

            ViewData["ScheduleId"] = new SelectList(_context.Schedules
                .Select(s => new { s.ScheduleId, DisplayText = $"{s.StartTime} - {s.EndTime}" }),
                "ScheduleId",
                "DisplayText");

            ViewData["ApartmentId"] = new SelectList(_context.Apartments
                .Where(a => a.Properties.Any(p => p.PropertyId == propertyID))
                .Select(a => new
                {
                    a.ApartmentId,
                    DisplayText = $"{a.Bedrooms} Beds - {a.Bathrooms} Baths - {a.FloorArea} sqm"
                }),
                "ApartmentId",
                "DisplayText");

            ViewData["statusId"] = "S1";

            return View();
        }

        // POST: BookAppointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,ManagerId,TenantId,ScheduleId,AppointmentDate,StatusId,ApartmentId")] BookAppointment bookAppointment)
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
                    StatusId = bookAppointment.StatusId,
                    ApartmentId = bookAppointment.ApartmentId
                };

                _context.Appointments.Add(newAppointment);
                await _context.SaveChangesAsync();


                //HttpContext.Session.SetString("userEmail", userAccount.Email);
                //HttpContext.Session.SetString("userType", userAccount.UserType);
                //HttpContext.Session.SetString("tenantID", tenant.TenantId);


                var returnUrl = TempData["returnUrl"] as string;
                if (returnUrl != null)
                {
                    return Redirect((string)returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

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

            var bookAppointment = await _context.Appointments.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,ManagerId,TenantId,ScheduleId,AppointmentDate,StatusId,ApartmentId")] BookAppointment bookAppointment)
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

            var bookAppointment = await _context.Appointments
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
            var bookAppointment = await _context.Appointments.FindAsync(id);
            if (bookAppointment != null)
            {
                _context.Appointments.Remove(bookAppointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookAppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}
