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
    public class AdminsController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public AdminsController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        private async Task<String> GetUserDetails(string userId)
        {
            string userDetails;
            bool isManager = await _context.Managers
                .AnyAsync(m => m.ManagerId.ToString() == userId);

            bool isAdmin = await _context.Admins
                .AnyAsync(m => m.AdminId.ToString() == userId);

            bool isTenant = await _context.Tenants
                .AnyAsync(m => m.TenantId.ToString() == userId);

            if (isManager || isAdmin)
            {
                var managerDetails = await _context.Employees
                    .FirstOrDefaultAsync(m => m.EmployeeId.ToString() == userId);

                userDetails = $"{managerDetails?.FirstName} {managerDetails?.LastName}";

            }
            else if (isTenant)
            {
                var tenantDetails = await _context.Tenants
                    .FirstOrDefaultAsync(t => t.TenantId == userId);

                userDetails = $"{tenantDetails?.FirstName} {tenantDetails?.LastName}";
            }
            else
            {
                userDetails = "[Account Not Found]";
            }

            return userDetails;
        }

        // GET: Admins
        public IActionResult Index()
        {
            return View();
        }


        // GET:  Admins/Reports/5
        public async Task<IActionResult> Reports(string id)
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


        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.AdminNavigation)
                .Include(a => a.EmailNavigation)
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            ViewData["AdminId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            ViewData["Email"] = new SelectList(_context.UserAccounts, "Email", "Email");
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,Email")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", admin.AdminId);
            ViewData["Email"] = new SelectList(_context.UserAccounts, "Email", "Email", admin.Email);
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", admin.AdminId);
            ViewData["Email"] = new SelectList(_context.UserAccounts, "Email", "Email", admin.Email);
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminId,Email")] Admin admin)
        {
            if (id != admin.AdminId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.AdminId))
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
            ViewData["AdminId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", admin.AdminId);
            ViewData["Email"] = new SelectList(_context.UserAccounts, "Email", "Email", admin.Email);
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.AdminNavigation)
                .Include(a => a.EmailNavigation)
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }
    }
}
