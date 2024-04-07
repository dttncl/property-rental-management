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
        public async Task<IActionResult> Index(string s)
        {

            var properties = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(s))
            {

                properties = _context.Properties
                                .Where(q =>
                                    q.City.CityName.Contains(s) ||
                                    q.Address.Contains(s))
                                .Include(c => c.City)
                                .Include(s => s.Status)
                                .Include(a => a.Apartments)
                                .Include(m => m.Managers);
            }
            else
            {
                properties = _context.Properties
                                .Include(c => c.City)
                                .Include(s => s.Status)
                                .Include(a => a.Apartments)
                                .Include(m => m.Managers);
            }

            return View(await properties.ToListAsync());

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
        public async Task<IActionResult> CreateAsync(string id)
        {
            var manager = await _context.Managers
                                        .Include(m => m.City)
                                        .FirstOrDefaultAsync(m => m.ManagerId == Convert.ToInt32(id));

            if (manager == null)
            {
                return NotFound();
            }

            ViewData["manager"] = manager;

            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string managerId, [Bind("PropertyId,Address,CityId,YearEstablished,TotalUnits,AvailableUnits,StatusId")] PropertyModel prop)
        {
            if (ModelState.IsValid)
            {

                string pID;
                do
                {
                    pID = RandomIDGenerator.GenerateRandomID("P", 4);
                }
                while (_context.Properties.Any(x => x.PropertyId == pID));

                Property newProperty = new Property
                {
                    PropertyId = pID,
                    Address = prop.Address,
                    CityId = prop.CityId,
                    YearEstablished = prop.YearEstablished,
                    TotalUnits = prop.TotalUnits,
                    AvailableUnits = prop.AvailableUnits,
                    StatusId = prop.StatusId
                };

                _context.Properties.Add(newProperty);
                await _context.SaveChangesAsync();

                var mId = managerId;
                await _context.Database.ExecuteSqlRawAsync("INSERT INTO PropertyManagers (ManagerId, PropertyId) VALUES ({0}, {1})", mId, newProperty.PropertyId);
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

            return View(prop);
        }

        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {

                return NotFound();
            }

            var prop = await _context.Properties.FindAsync(id);
            

            if (prop == null)
            {
                return NotFound();
            }

            PropertyModel updatedProperty = new PropertyModel
            {
                PropertyId = prop.PropertyId,
                Address = prop.Address,
                CityId = prop.CityId,
                YearEstablished = prop.YearEstablished,
                TotalUnits = prop.TotalUnits,
                AvailableUnits = prop.AvailableUnits,
                StatusId = prop.StatusId
            };

            var city = await _context.Cities.FindAsync(prop.CityId);
            var status = await _context.Statuses.FindAsync(prop.StatusId);

            ViewData["cityName"] = city.CityName;
            ViewData["statusDesc"] = status.Description;
            return View(updatedProperty);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("PropertyId,Address,CityId,YearEstablished,TotalUnits,AvailableUnits,StatusId")] PropertyModel prop)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProperty = await _context.Properties.FindAsync(prop.PropertyId);

                    if (existingProperty == null)
                    {
                        return NotFound();
                    }

                    existingProperty.Address = prop.Address;
                    existingProperty.YearEstablished = prop.YearEstablished;

                    _context.Properties.Update(existingProperty);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(prop.PropertyId))
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

            return View(prop);
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
            var prop = await _context.Properties.FindAsync(id);

            if (prop == null)
            {
                return NotFound();
            }
            try
            {
                var apartments = await _context.Apartments
                    .Where(a => a.Properties.Any(p => p.PropertyId == id))
                    .ToListAsync();

                // manually delete records from PropertyApartments table
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM PropertyApartments WHERE PropertyId = {0}", id);

                foreach (var apartment in apartments)
                {
                    _context.Apartments.Remove(apartment);
                }

                // manually delete records from PropertyManagers table
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM PropertyManagers WHERE PropertyId = {0}", id);

                _context.Properties.Remove(prop);

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
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        private bool PropertyExists(string id)
        {
            return _context.Properties.Any(e => e.PropertyId == id);
        }
    }
}
