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
    public class AppointmentsController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public AppointmentsController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        // POST: Appointments/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int appointmentId, string statusId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            var status = await _context.Statuses.FindAsync(statusId);
            if (status == null)
            {
                return NotFound();
            }

            appointment.StatusId = statusId;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

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
    }
}
