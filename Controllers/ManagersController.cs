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
    public class ManagersController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public ManagersController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        // GET: Managers/Listings/5
        public async Task<IActionResult> Listings(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var manager = await _context.Managers
                .Include(c => c.City)
                .Include(e => e.EmailNavigation)
                .Include(mn => mn.ManagerNavigation)
                .Include(p => p.Properties)
                    .ThenInclude(a => a.Apartments)
                .FirstOrDefaultAsync(m => m.ManagerId.ToString() == id);

            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // GET: Managers
        public async Task<IActionResult> Index()
        {
            var rentaSpaceDbContext = _context.Managers.Include(m => m.City).Include(m => m.EmailNavigation).Include(m => m.ManagerNavigation);
            return View(await rentaSpaceDbContext.ToListAsync());
        }

        // GET: Managers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .Include(m => m.City)
                .Include(m => m.EmailNavigation)
                .Include(m => m.ManagerNavigation)
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // GET: Managers/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId");
            ViewData["Email"] = new SelectList(_context.UserAccounts, "Email", "Email");
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ManagerId,Email,CityId")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", manager.CityId);
            ViewData["Email"] = new SelectList(_context.UserAccounts, "Email", "Email", manager.Email);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", manager.ManagerId);
            return View(manager);
        }

        // GET: Managers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", manager.CityId);
            ViewData["Email"] = new SelectList(_context.UserAccounts, "Email", "Email", manager.Email);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", manager.ManagerId);
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ManagerId,Email,CityId")] Manager manager)
        {
            if (id != manager.ManagerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.ManagerId))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", manager.CityId);
            ViewData["Email"] = new SelectList(_context.UserAccounts, "Email", "Email", manager.Email);
            ViewData["ManagerId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", manager.ManagerId);
            return View(manager);
        }

        // GET: Managers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .Include(m => m.City)
                .Include(m => m.EmailNavigation)
                .Include(m => m.ManagerNavigation)
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager != null)
            {
                _context.Managers.Remove(manager);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManagerExists(int id)
        {
            return _context.Managers.Any(e => e.ManagerId == id);
        }
    }
}
