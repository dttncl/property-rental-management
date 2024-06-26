﻿using System;
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

        // GET: Msgs/Details/5
        public async Task<IActionResult> Details(int? msgID)
        {
            if (msgID == null)
            {
                return View("Error");
            }

            var tenantID = HttpContext.Session.GetString("tenantID");
            var employeeID = HttpContext.Session.GetString("employeeID");

            if (tenantID == null && employeeID == null)
            {
                return View("AccessDenied");

            }

            var msg = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == msgID);

            if (msg == null)
            {
                return View("Error");
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

            if (!(tenantID == msg.Sender || tenantID == msg.Receiver || employeeID == msg.Sender || employeeID == msg.Receiver))
            {
                return View("AccessDenied");
            }

            if (tenantID != null)
            {
                HttpContext.Session.SetString("tenantID", tenantID);
            }

            if (employeeID != null)
            {
                HttpContext.Session.SetString("employeeID", employeeID);
            }

            return View(foundMessage);
        }

        // GET: Msgs/Create
        public IActionResult Create(String msgTo, String msgFrom, String sender)
        {
            if (msgTo == null && msgFrom == null && sender == null)
            {
                return View("Error");
            }

            var tenantID = HttpContext.Session.GetString("tenantID");
            var employeeID = HttpContext.Session.GetString("employeeID");

            if (tenantID == null && employeeID == null)
            {
                return View("AccessDenied");

            }

            if (sender == "tenant")
            {
                if (tenantID != msgFrom)
                {
                    return View("AccessDenied");

                }

                var isManager = _context.Managers.Any(m => m.ManagerId.ToString() == msgTo);
                if (isManager)
                {
                    ViewData["managerID"] = msgTo;
                }
                else
                {
                    var managers = _context.Managers
                                        .Where(m => m.Properties.Any(p => p.PropertyId == msgTo))
                                        .Where(e => e.ManagerNavigation.StatusId == "E1")
                                        .Select(p => new
                                        {
                                            ManagerID = p.ManagerId,
                                            ManagerValue = $"{p.ManagerNavigation.FirstName} {p.ManagerNavigation.LastName}"
                                        })
                                        .ToList();

                    ViewData["managersList"] = new SelectList(managers, "ManagerID", "ManagerValue");
                }

                ViewData["tenantID"] = msgFrom;

                var tenant = _context.Tenants
                    .FirstOrDefault(t => t.TenantId == msgFrom);

                ViewData["txtName"] = $"{tenant.FirstName} {tenant.LastName}";
                ViewData["txtEmail"] = tenant.Email;
                ViewData["txtPhone"] = tenant.Phone;

            } else if (sender == "employee")
            {

                if (employeeID != msgFrom)
                {
                    return View("AccessDenied");

                }

                ViewData["managerID"] = msgFrom;

                var tenants = _context.Tenants.Select(t => new {
                    TenantID = t.TenantId,
                    TenantEmail = t.Email,
                    TenantValue = $"{t.TenantId}|{t.Email}|{t.FirstName} {t.LastName}|{t.Phone}"
                }).ToList();

                ViewData["tenantIDList"] = new SelectList(tenants, "TenantValue", "TenantEmail");

            } else if (sender == "reply")
            {
                // check if message is from manager
                var isManager = _context.Managers.Any(m => m.ManagerId.ToString() == msgFrom);

                // check if message is from tenant
                var isTenant = _context.Tenants.Any(m => m.TenantId.ToString() == msgFrom);

                if (isTenant)
                {
                    if (tenantID != msgFrom)
                    {
                        return View("AccessDenied");

                    }

                    ViewData["tenantID"] = msgFrom;
                    ViewData["managerID"] = msgTo;

                    var managers = _context.Managers
                        .Where(m => m.ManagerId.ToString() == msgTo)
                        .Select(p => new
                        {
                            ManagerID = p.ManagerId,
                            ManagerValue = $"{p.ManagerNavigation.FirstName} {p.ManagerNavigation.LastName}"
                        })
                        .ToList();

                    ViewData["managersList"] = new SelectList(managers, "ManagerID", "ManagerValue");

                    var tenant = _context.Tenants
                        .FirstOrDefault(t => t.TenantId == msgFrom);

                    ViewData["txtName"] = $"{tenant.FirstName} {tenant.LastName}";
                    ViewData["txtEmail"] = tenant.Email;
                    ViewData["txtPhone"] = tenant.Phone;

                }
                else if (isManager)
                {
                    if (employeeID != msgFrom)
                    {
                        return View("AccessDenied");

                    }

                    ViewData["managerID"] = msgFrom;
                    ViewData["tenantID"] = msgTo;

                    var tenants = _context.Tenants
                        .Where(m => m.TenantId.ToString() == msgTo)
                        .Select(t => new {
                        TenantID = t.TenantId,
                        TenantEmail = t.Email,
                        TenantValue = $"{t.TenantId}|{t.Email}|{t.FirstName} {t.LastName}|{t.Phone}"
                    }).ToList();

                    ViewData["tenantIDList"] = new SelectList(tenants, "TenantValue", "TenantEmail");

                }

                ViewData["sender"] = "reply";

            }
            else if (sender == "employeeReport")
            {

                if (employeeID != msgFrom)
                {
                    return View("AccessDenied");

                }

                ViewData["sender"] = "employeeReport";
                ViewData["managerID"] = msgFrom;

                var admins = _context.Admins
                    .Select(a => new {
                        AdminID = a.AdminId,
                        AdminEmail = a.Email,
                        AdminValue = $"{a.AdminId}|{a.Email}|{a.AdminNavigation.FirstName} {a.AdminNavigation.LastName}|{a.AdminNavigation.Phone}"
                    }).ToList();

                ViewData["adminIDList"] = new SelectList(admins, "AdminValue", "AdminEmail");
            }

            if (tenantID != null)
            {
                HttpContext.Session.SetString("tenantID", tenantID);
            }

            if (employeeID != null)
            {
                HttpContext.Session.SetString("employeeID", employeeID);
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

            var tenantID = HttpContext.Session.GetString("tenantID");
            var employeeID = HttpContext.Session.GetString("employeeID");

            if (tenantID == null && employeeID == null)
            {
                return View("AccessDenied");

            }

            if (tenantID != msg.Sender && employeeID != msg.Sender)
            {
                return View("AccessDenied");

            }

            if (ModelState.IsValid)
            {
                if (msg == null)
                {
                    return View("Error");
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

            if (tenantID != null)
            {
                HttpContext.Session.SetString("tenantID", tenantID);
            }

            if (employeeID != null)
            {
                HttpContext.Session.SetString("employeeID", employeeID);
            }

            return View(msg);
        }

    }
}
