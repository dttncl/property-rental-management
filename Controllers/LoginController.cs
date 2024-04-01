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
    public class LoginController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public LoginController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Email,Password")] Login login)
        {
            if (ModelState.IsValid)
            {
                if (login == null)
                {
                    return NotFound();
                }

                var userAccount = await _context.UserAccounts
                    .FirstOrDefaultAsync(m => m.Email == login.Email && m.Password == login.Password);

                if (userAccount == null)
                {
                    return NotFound();

                } else
                {

                    HttpContext.Session.SetString("userEmail", userAccount.Email);
                    HttpContext.Session.SetString("userType", userAccount.UserType);

                    if (userAccount.UserType == "Employee")
                    {
                        var employee = await _context.Employees
                            .FirstOrDefaultAsync(e => e.Email == userAccount.Email);


                        if (employee == null)
                        {
                            return NotFound();
                        }

                        // ADMIN
                        else if (employee.JobId == 500)
                        {
                            HttpContext.Session.SetString("jobID", "500");
                            HttpContext.Session.SetString("employeeID", employee.EmployeeId.ToString());


                        }

                        // MANAGER
                        else if (employee.JobId == 502)
                        {
                            HttpContext.Session.SetString("jobID", "502");
                            HttpContext.Session.SetString("employeeID", employee.EmployeeId.ToString());

                        }
                    } else
                    {
                        var tenant = await _context.Tenants
                            .FirstOrDefaultAsync(t => t.Email == userAccount.Email);

                        if (tenant == null)
                        {
                            return NotFound();
                        }

                        HttpContext.Session.SetString("tenantID", tenant.TenantId);
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

            return View(login);
        }
    
    }
}
