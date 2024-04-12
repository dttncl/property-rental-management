using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using property_rental_management.Models;

namespace property_rental_management.Controllers
{
    public class TenantsController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public TenantsController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        private async Task<String> GetUserDetails(string userId)
        {
            bool isManager = await _context.Managers
                .AnyAsync(m => m.ManagerId.ToString() == userId);

            if (isManager)
            {
                var managerDetails = await _context.Employees
                    .FirstOrDefaultAsync(m => m.EmployeeId.ToString() == userId);

                return $"{managerDetails?.FirstName} {managerDetails?.LastName}";

            }
            else
            {
                var tenantDetails = await _context.Tenants
                    .FirstOrDefaultAsync(t => t.TenantId == userId);

                return $"{tenantDetails?.FirstName} {tenantDetails?.LastName}";
            }
        }


        // GET: Tenants
        public async Task<IActionResult> Index(string s)
        {
            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || jobID != "500")
            {
                return View("AccessDenied");
            }

            var tenants = _context.Tenants.AsQueryable();

            if (!string.IsNullOrEmpty(s))
            {

                tenants = _context.Tenants
                            .Where(q =>
                                q.FirstName.Contains(s) ||
                                q.LastName.Contains(s) ||
                                q.EmailNavigation.Email.Contains(s))
                            .Include(e => e.EmailNavigation);
            } else
            {
                tenants = _context.Tenants
                            .Include(t => t.EmailNavigation);
            }

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);


            return View(await tenants.ToListAsync());

        }


        // GET: Tenants/Profile
        public async Task<IActionResult> Profile(string id)
        {
            var tenantID = HttpContext.Session.GetString("tenantID");

            if (id == null)
            {
                return View("Error");
            }

            if (tenantID == null)
            {
                return View("AccessDenied");

            }

            var tenant = await _context.Tenants
                .Include(t => t.EmailNavigation)
                .FirstOrDefaultAsync(m => m.TenantId == id);

            if (tenant == null)
            {
                return View("Error");
            }

            HttpContext.Session.SetString("tenantID", tenantID);

            return View(tenant);
        }

        // GET: Tenants/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            if (id == null)
            {
                return View("Error");
            }

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");
            var tenantID = HttpContext.Session.GetString("tenantID");

            if ((employeeID == null || jobID != "500") && tenantID == null)
            {
                return View("AccessDenied");
            }

            if (tenantID != null && tenantID != id)
            {
                return View("AccessDenied");
            }

            var tenant = await _context.Tenants.FindAsync(id);

            if (tenant == null)
            {
                return View("Error");
            }

            ViewData["Email"] = tenant.Email;

            if (tenantID != null)
            {
                HttpContext.Session.SetString("tenantID", tenantID);

            } else if (employeeID != null)
            {
                HttpContext.Session.SetString("employeeID", employeeID);
                HttpContext.Session.SetString("jobID", jobID);

            }

            return View(tenant);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TenantId,FirstName,LastName,Email,Phone")] TenantModel tenant)
        {


            if (id != tenant.TenantId)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {

                var employeeID = HttpContext.Session.GetString("employeeID");
                var jobID = HttpContext.Session.GetString("jobID");
                var tenantID = HttpContext.Session.GetString("tenantID");

                if ((employeeID == null || jobID != "500") && tenantID == null)
                {
                    return View("AccessDenied");
                }

                if (tenantID != null && tenantID != id)
                {
                    return View("AccessDenied");
                }


                try
                {
                    var existingTenant = await _context.Tenants.FindAsync(tenant.TenantId);

                    if (existingTenant != null)
                    {
                        existingTenant.FirstName = tenant.FirstName;
                        existingTenant.LastName = tenant.LastName;
                        existingTenant.Email = tenant.Email;
                        existingTenant.Phone = tenant.Phone;

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TenantExists(tenant.TenantId))
                    {
                        return View("Error");
                    }
                    else
                    {
                        throw;
                    }
                }


                if (tenantID != null)
                {
                    HttpContext.Session.SetString("tenantID", tenantID);

                }
                else if (employeeID != null)
                {
                    HttpContext.Session.SetString("employeeID", employeeID);
                    HttpContext.Session.SetString("jobID", jobID);

                }

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

            return View(tenant);
        }


        // GET:  Tenants/Messages/5
        public async Task<IActionResult> Messages(string id)
        {


            if (id == null)
            {
                return View("Error");
            }

            var tenantID = HttpContext.Session.GetString("tenantID");

            if (tenantID == null)
            {
                return View("AccessDenied");

            }

            var messages = await _context.Messages
                .Where(sr => sr.Sender == id || sr.Receiver == id)
                .ToListAsync();

            messages.Reverse();

            List<Message> formattedMessages = new List<Message>();

            foreach (var msg in messages)
            {
                var senderDetails = await GetUserDetails(msg.Sender);
                var receiverDetails = await GetUserDetails(msg.Receiver);

                Message foundMessage = new Message
                {
                    MessageId = msg.MessageId,
                    Sender = $"{msg.Sender}|{senderDetails}",
                    Receiver = $"{msg.Receiver}|{receiverDetails}",
                    Subject = msg.Subject,
                    Message1 = msg.Message1
                };

                formattedMessages.Add(foundMessage);
            }

            HttpContext.Session.SetString("tenantID", tenantID);

            return View(formattedMessages);
        }


        // GET:  Tenants/Appointments/5
        public async Task<IActionResult> Appointments(string id)
        {



            if (id == null)
            {
                return View("Error");
            }

            var tenantID = HttpContext.Session.GetString("tenantID");

            if (tenantID == null)
            {
                return View("AccessDenied");

            }

            var appointments = await _context.Appointments
                .Where(ten => ten.TenantId == id)
                .Include(m => m.Manager)
                    .ThenInclude(mn => mn.ManagerNavigation)
                .Include(a => a.Apartment)
                    .ThenInclude(p => p.Properties)
                .Include(sc => sc.Schedule)
                .Include(s => s.Status)
                .OrderByDescending(app => app.AppointmentDate)
                .ToListAsync();

            HttpContext.Session.SetString("tenantID", tenantID);

            return View(appointments);
        }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || jobID != "500")
            {
                return View("AccessDenied");
            }


            var tenant = await _context.Tenants
                .Include(t => t.EmailNavigation)
                .FirstOrDefaultAsync(m => m.TenantId == id);

            if (tenant == null)
            {
                return View("Error");
            }

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);

            return View(tenant);
        }
      

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");
            var tenantID = HttpContext.Session.GetString("tenantID");

            if ((employeeID == null || jobID != "500") && tenantID == null)
            {
                return View("AccessDenied");
            }

            if (tenantID != null && tenantID != id)
            {
                return View("AccessDenied");
            }

            var tenant = await _context.Tenants
                .Include(t => t.EmailNavigation)
                .FirstOrDefaultAsync(m => m.TenantId == id);

            if (tenant == null)
            {
                return View("Error");
            }

            if (tenantID != null)
            {
                HttpContext.Session.SetString("tenantID", tenantID);

            }
            else if (employeeID != null)
            {
                HttpContext.Session.SetString("employeeID", employeeID);
                HttpContext.Session.SetString("jobID", jobID);

            }

            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");
            var tenantID = HttpContext.Session.GetString("tenantID");

            if ((employeeID == null || jobID != "500") && tenantID == null)
            {
                return View("AccessDenied");
            }

            if (tenantID != null && tenantID != id)
            {
                return View("AccessDenied");
            }

            var appointments = await _context.Appointments
                        .Where(t => t.TenantId == id)
                        .ToListAsync();

            foreach (var item in appointments)
            {
                _context.Appointments.Remove(item);
            }

            var tenant = await _context.Tenants.FindAsync(id);

            if (tenant != null)
            {
                _context.Tenants.Remove(tenant);
            }

            await _context.SaveChangesAsync();

            if (tenantID != null)
            {
                HttpContext.Session.SetString("tenantID", tenantID);

            }
            else if (employeeID != null)
            {
                HttpContext.Session.SetString("employeeID", employeeID);
                HttpContext.Session.SetString("jobID", jobID);

            }


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

        private bool TenantExists(string id)
        {
            return _context.Tenants.Any(e => e.TenantId == id);
        }
    }
}
