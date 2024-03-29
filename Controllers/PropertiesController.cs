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
    public class PropertiesController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public PropertiesController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        // GET: Properties
        public async Task<IActionResult> Index()
        {
            var rentaSpaceDbContext = _context.Properties
                .Include(c => c.City)
                .Include(s => s.Status)
                .Include(a => a.Apartments)
                .Include(m => m.Managers);
            return View(await rentaSpaceDbContext.ToListAsync());
        }

        // GET: Properties/List
        public async Task<IActionResult> List()
        {
            var rentaSpaceDbContext = _context.Properties
                .Include(c => c.City)
                .Include(s => s.Status)
                .Include(a => a.Apartments);
            return View(await rentaSpaceDbContext.ToListAsync());
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties
                .Include(c => c.City)
                .Include(m => m.Managers)
                .Include(s => s.Status)
                .Include(a => a.Apartments)
                    .ThenInclude(sa => sa.Status)
                .FirstOrDefaultAsync(m => m.PropertyId == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // GET: Properties/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId");
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PropertyId,Address,CityId,YearEstablished,TotalUnits,AvailableUnits,StatusId")] Property @property)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@property);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", @property.CityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", @property.StatusId);
            return View(@property);
        }

        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties.FindAsync(id);
            if (@property == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", @property.CityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", @property.StatusId);
            return View(@property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PropertyId,Address,CityId,YearEstablished,TotalUnits,AvailableUnits,StatusId")] Property @property)
        {
            if (id != @property.PropertyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(@property.PropertyId))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", @property.CityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", @property.StatusId);
            return View(@property);
        }

        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties
                .Include(c => c.City)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(m => m.PropertyId == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @property = await _context.Properties.FindAsync(id);
            if (@property != null)
            {
                _context.Properties.Remove(@property);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(string id)
        {
            return _context.Properties.Any(e => e.PropertyId == id);
        }
    }
}
