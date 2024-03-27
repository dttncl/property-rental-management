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
    public class RegisterController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public RegisterController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        // GET: Register
        public IActionResult Index()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Register register)
        {
            if (ModelState.IsValid)
            {
                if (register == null)
                {
                    return NotFound();
                }

                var userAccount = await _context.UserAccounts
                    .FirstOrDefaultAsync(m => m.Email == register.Email);

                if (userAccount == null)
                {
                    var newUserAccount = new UserAccount
                    {
                        Email = register.Email,
                        Password = register.Password,
                        UserType = "Tenant"
                    };

                    _context.UserAccounts.Add(newUserAccount);
                    await _context.SaveChangesAsync();

                    string tID;
                    do
                    {
                        tID = RandomIDGenerator.GenerateRandomID("T", 4);
                    }
                    while (_context.Tenants.Any(x => x.TenantId == tID));

                    var tenant = new Tenant
                    {
                        TenantId = tID,
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        Phone = register.Phone
                    };

                    _context.Tenants.Add(tenant);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                }
            }

            return View(register);
        }

    }
}
