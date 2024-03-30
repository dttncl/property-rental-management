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
    public class MsgsController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public MsgsController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        // GET: Msgs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Msg.ToListAsync());
        }

        // GET: Msgs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var msg = await _context.Msg
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (msg == null)
            {
                return NotFound();
            }

            return View(msg);
        }

        // GET: Msgs/Create
        public IActionResult Create(String managerId, String tenantID)
        {
            ViewData["tenantID"] = tenantID;
            ViewData["managerID"] = managerId;

            var tenant = _context.Tenants
                .FirstOrDefault(t => t.TenantId == tenantID);

            if (tenant != null)
            {
                ViewData["tenantName"] = tenant.FirstName;
                ViewData["tenantEmail"] = tenant.Email;
                ViewData["tenantPhone"] = tenant.Phone;
            }

            return View();
        }

        // POST: Msgs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MessageId,ManagerId,TenantId,Message1")] Msg msg)
        {
            if (ModelState.IsValid)
            {
                if (msg == null)
                {
                    return NotFound();
                }

                var newMessage = new Message
                {
                    MessageId = msg.MessageId,
                    ManagerId = msg.ManagerId,
                    TenantId = msg.TenantId,
                    Message1 = msg.Message1
                };

                _context.Messages.Add(newMessage);
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



            return View(msg);
        }

        // GET: Msgs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var msg = await _context.Msg.FindAsync(id);
            if (msg == null)
            {
                return NotFound();
            }
            return View(msg);
        }

        // POST: Msgs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MessageId,ManagerId,TenantId,Message1")] Msg msg)
        {
            if (id != msg.MessageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(msg);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MsgExists(msg.MessageId))
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
            return View(msg);
        }

        // GET: Msgs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var msg = await _context.Msg
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (msg == null)
            {
                return NotFound();
            }

            return View(msg);
        }

        // POST: Msgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var msg = await _context.Msg.FindAsync(id);
            if (msg != null)
            {
                _context.Msg.Remove(msg);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MsgExists(int id)
        {
            return _context.Msg.Any(e => e.MessageId == id);
        }
    }
}
