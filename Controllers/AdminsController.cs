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
            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || jobID != "500")
            {
                return View("AccessDenied");
            }

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);

            return View();
        }


        // GET:  Admins/Reports/5
        public async Task<IActionResult> Reports(string id)
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

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);


            return View(formattedMessages);
        }


    }
}
