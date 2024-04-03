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
        public async Task<IActionResult> Index(string searchString)
        {

            var tenants = _context.Tenants.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {

                tenants = _context.Tenants
                            .Where(q =>
                                q.FirstName.Contains(searchString) ||
                                q.LastName.Contains(searchString) ||
                                q.EmailNavigation.Email.Contains(searchString))
                            .Include(e => e.EmailNavigation);
            } else
            {
                tenants = _context.Tenants
                            .Include(t => t.EmailNavigation);
            }

            return View(await tenants.ToListAsync());

        }



        // GET: Tenants/Profile
        public async Task<IActionResult> Profile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants
                .Include(t => t.EmailNavigation)
                .FirstOrDefaultAsync(m => m.TenantId == id);

            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        // GET: Tenants/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants.FindAsync(id);

            if (tenant == null)
            {
                return NotFound();
            }

            ViewData["Email"] = tenant.Email;

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
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TenantExists(tenant.TenantId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
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
                return NotFound();
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

            return View(formattedMessages);
        }


        // GET:  Tenants/Appointments/5
        public async Task<IActionResult> Appointments(string id)
        {
            if (id == null)
            {
                return NotFound();
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

            return View(appointments);
        }








        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants
                .Include(t => t.EmailNavigation)
                .FirstOrDefaultAsync(m => m.TenantId == id);
            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        // GET: Tenants/Create
        public IActionResult Create()
        {
            ViewData["Email"] = new SelectList(_context.UserAccounts, "Email", "Email");
            return View();
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenantId,FirstName,LastName,Email,Phone")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tenant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Email"] = new SelectList(_context.UserAccounts, "Email", "Email", tenant.Email);
            return View(tenant);
        }

        

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants
                .Include(t => t.EmailNavigation)
                .FirstOrDefaultAsync(m => m.TenantId == id);

            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

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
