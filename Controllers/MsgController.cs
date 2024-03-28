using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using property_rental_management.Models;

namespace property_rental_management.Controllers
{
    public class MsgController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public MsgController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        // GET: Msg
        public async Task<IActionResult> Index()
        {
            return View(await _context.Messages.ToListAsync());
        }

        // GET: Msg/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var msg = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (msg == null)
            {
                return NotFound();
            }

            return View(msg);
        }

        // GET: Msg/Create
        public IActionResult Create(String managerId, String tenantID)
        {
            ViewData["tenantID"] = tenantID;
            ViewData["managerID"] = managerId;
            return View();
        }

        // POST: Msg/Create
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

                var message = new Message
                {
                    MessageId = msg.MessageId,
                    ManagerId = msg.ManagerId,
                    TenantId = msg.TenantId,
                    Message1 = msg.Message1
                };
                
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(msg);
        }

        // GET: Msg/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var msg = await _context.Messages.FindAsync(id);
            if (msg == null)
            {
                return NotFound();
            }
            return View(msg);
        }

        // POST: Msg/Edit/5
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

        // GET: Msg/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var msg = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (msg == null)
            {
                return NotFound();
            }

            return View(msg);
        }

        // POST: Msg/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var msg = await _context.Messages.FindAsync(id);
            if (msg != null)
            {
                _context.Messages.Remove(msg);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MsgExists(int id)
        {
            return _context.Messages.Any(e => e.MessageId == id);
        }
    }
}
