using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        // GET: Msgs/Details/5
        public async Task<IActionResult> Details(int? msgID)
        {
            if (msgID == null)
            {
                return NotFound();
            }

            var msg = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == msgID);

            if (msg == null)
            {
                return NotFound();
            }

            var senderDetails = await GetUserDetails(msg.Sender);
            var receiverDetails = await GetUserDetails(msg.Receiver);

            Msg foundMessage = new Msg
            {
                MessageId = msg.MessageId,
                Sender = $"{msg.Sender}|{senderDetails}",
                Receiver = $"{msg.Receiver}|{receiverDetails}",
                Subject = msg.Subject,
                Message1 = msg.Message1
            };

            return View(foundMessage);
        }

        // GET: Msgs/Create
        public IActionResult Create(String msgTo, String msgFrom, String sender)
        {

            if (sender == null)
            {
                var isManager = _context.Managers.Any(m => m.ManagerId.ToString() == msgTo);
                if (isManager)
                {
                    ViewData["managerID"] = msgTo;
                }
                else
                {
                    var managers = _context.Managers
                                        .Where(m => m.Properties.Any(p => p.PropertyId == msgTo))
                                        .Select(p => new
                                        {
                                            ManagerID = p.ManagerId,
                                            ManagerValue = $"{p.ManagerNavigation.FirstName} {p.ManagerNavigation.LastName}"
                                        })
                                        .ToList();

                    ViewData["managersList"] = new SelectList(managers, "ManagerID", "ManagerValue");
                }
            }
            else
            {
                ViewData["managerID"] = msgFrom;
            }
            

            if (sender == "employee")
            {
                var tenants = _context.Tenants.Select(t => new {
                    TenantID = t.TenantId,
                    TenantEmail = t.Email,
                    TenantValue = $"{t.TenantId}|{t.Email}|{t.FirstName} {t.LastName}|{t.Phone}"
                }).ToList();

                ViewData["tenantIDList"] = new SelectList(tenants, "TenantValue", "TenantEmail");
            } 
            else if (sender == "employeeReply")
            {
                ViewData["tenantID"] = msgFrom;
                ViewData["sender"] = "employeeReply";

                var tenant = _context.Tenants
                    .FirstOrDefault(t => t.TenantId == msgTo);

                ViewData["txtName"] = $"{tenant.FirstName} {tenant.LastName}";
                ViewData["txtEmail"] = tenant.Email;
                ViewData["txtPhone"] = tenant.Phone;

            } else if (sender == "employeeReport")
            {
                ViewData["sender"] = "employeeReport";

                var admins = _context.Admins
                    .Select(a => new {
                    AdminID = a.AdminId,
                    AdminEmail = a.Email,
                    AdminValue = $"{a.AdminId}|{a.Email}|{a.AdminNavigation.FirstName} {a.AdminNavigation.LastName}|{a.AdminNavigation.Phone}"
                }).ToList();

                ViewData["adminIDList"] = new SelectList(admins, "AdminValue", "AdminEmail");
            }       
            else
            {
                ViewData["tenantID"] = msgFrom;

                var tenant = _context.Tenants
                    .FirstOrDefault(t => t.TenantId == msgFrom);

                ViewData["txtName"] = $"{tenant.FirstName} {tenant.LastName}";
                ViewData["txtEmail"] = tenant.Email;
                ViewData["txtPhone"] = tenant.Phone;
            }

            return View();
        }

        // POST: Msgs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Msg msg)
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
                    Sender = msg.Sender,
                    Receiver = msg.Receiver.Split('|')[0],
                    Subject = msg.Subject,
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
