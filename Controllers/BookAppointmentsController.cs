using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Plugins;
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
                return View("Error");
            }

            var bookAppointment = await _context.Appointments
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (bookAppointment == null)
            {
                return View("Error");
            }

            return View(bookAppointment);
        }

        // GET: BookAppointments/Create
        public IActionResult Create(String managerId, String tenantID, String propertyID)
        {

            if (managerId == null && tenantID == null && propertyID == null)
            {
                return View("Error");
            }

            var stenantID = HttpContext.Session.GetString("tenantID");
            var employeeID = HttpContext.Session.GetString("employeeID");

            if (stenantID == null && employeeID == null)
            {
                return View("AccessDenied");

            }

            var isManager = _context.Managers.Any(m => m.ManagerId.ToString() == managerId);

            if (isManager)
            {
                if (employeeID != managerId)
                {
                    return View("AccessDenied");

                }

                ViewData["managerID"] = managerId;
            }
            else
            {
                if (stenantID != tenantID)
                {
                    return View("AccessDenied");

                }

                var managers = _context.Managers
                                    .Where(m => m.Properties.Any(p => p.PropertyId == managerId))
                                    .Select(p => new
                                    {
                                        ManagerID = p.ManagerId,
                                        ManagerValue = $"{p.ManagerNavigation.FirstName} {p.ManagerNavigation.LastName}"
                                    })
                                    .ToList();

                ViewData["managersList"] = new SelectList(managers, "ManagerID", "ManagerValue");
            }

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

            if (stenantID != null)
            {
                HttpContext.Session.SetString("tenantID", stenantID);
            }

            if (employeeID != null)
            {
                HttpContext.Session.SetString("employeeID", employeeID);
            }

            return View();
        }

        // POST: BookAppointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,ManagerId,TenantId,ScheduleId,AppointmentDate,StatusId,ApartmentId")] BookAppointment bookAppointment)
        {

            var stenantID = HttpContext.Session.GetString("tenantID");
            var employeeID = HttpContext.Session.GetString("employeeID");

            if (stenantID == null && employeeID == null)
            {
                return View("AccessDenied");

            }

            if (ModelState.IsValid)
            {

                if (bookAppointment == null)
                {
                    return View("Error");
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

                var returnUrl = ViewData["returnUrl"] as string;
                if (returnUrl != null)
                {
                    return Redirect((string)returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }

            if (stenantID != null)
            {
                HttpContext.Session.SetString("tenantID", stenantID);
            }

            if (employeeID != null)
            {
                HttpContext.Session.SetString("employeeID", employeeID);
            }

            return View(bookAppointment);
        }

        // GET: BookAppointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var bookAppointment = await _context.Appointments.FindAsync(id);
            if (bookAppointment == null)
            {
                return View("Error");
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
                return View("Error");
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
                        return View("Error");
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
                return View("Error");
            }

            var bookAppointment = await _context.Appointments
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (bookAppointment == null)
            {
                return View("Error");
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
