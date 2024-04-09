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
    public class ApartmentsController : Controller
    {
        private readonly RentaSpaceDbContext _context;

        public ApartmentsController(RentaSpaceDbContext context)
        {
            _context = context;
        }

        private async Task UpdatePropertyUnits (string appId)
        {
            var relatedProperty = await _context.Properties
                .Include(p => p.Apartments)
                .FirstOrDefaultAsync(p => p.Apartments.Any(a => a.ApartmentId == appId));

            if (relatedProperty != null)
            {
                //  update available units
                int availableUnits = relatedProperty.Apartments.Count(a => a.StatusId == "A1");
                relatedProperty.AvailableUnits = availableUnits;

                if (availableUnits == 0)
                {
                    relatedProperty.StatusId = "P1";
                }

                _context.Properties.Update(relatedProperty);
                await _context.SaveChangesAsync();
            }
        }

        // GET: Apartments
        public async Task<IActionResult> Index()
        {
            var rentaSpaceDbContext = _context.Apartments
                .Include(a => a.Status)
                .Include(p => p.Properties)
                    .ThenInclude(c => c.City);
            return View(await rentaSpaceDbContext.ToListAsync());
        }

        // GET: Apartments/List
        public async Task<IActionResult> List()
        {
            var rentaSpaceDbContext = _context.Apartments
                .Include(a => a.Status)
                .Include(p => p.Properties);
            return View(await rentaSpaceDbContext.ToListAsync());
        }

        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var tenantID = HttpContext.Session.GetString("tenantID");
            var employeeID = HttpContext.Session.GetString("employeeID");

            if (tenantID == null && employeeID == null)
            {
                return View("AccessDenied");

            }

            var apartment = await _context.Apartments
                .Include(a => a.Status)
                .Include(p => p.Properties)
                    .ThenInclude(c => c.City)
                .FirstOrDefaultAsync(m => m.ApartmentId == id);

            if (apartment == null)
            {
                return View("Error");
            }

            if (tenantID != null)
            {
                HttpContext.Session.SetString("tenantID", tenantID);
            }

            if (employeeID != null)
            {
                HttpContext.Session.SetString("employeeID", employeeID);
            }


            return View(apartment);
        }

        // GET: Apartments/Create
        public IActionResult Create(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || (jobID != "502" && jobID != "500"))
            {
                return View("AccessDenied");
            }

            ViewBag.PropertyId = id;

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);

            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("ApartmentId,Bedrooms,Bathrooms,FloorArea,StatusId,Price")] ApartmentModel apartment)
        {
            if (ModelState.IsValid)
            {

                var employeeID = HttpContext.Session.GetString("employeeID");
                var jobID = HttpContext.Session.GetString("jobID");

                if (employeeID == null || (jobID != "502" && jobID != "500"))
                {
                    return View("AccessDenied");
                }

                string aID;
                do
                {
                    aID = RandomIDGenerator.GenerateRandomID("A", 4);
                }
                while (_context.Apartments.Any(x => x.ApartmentId == aID));

                Apartment newApartment = new Apartment
                {
                    ApartmentId = aID,
                    Bedrooms = apartment.Bedrooms,
                    Bathrooms = apartment.Bathrooms,
                    FloorArea = apartment.FloorArea,
                    StatusId = apartment.StatusId,
                    Price = apartment.Price
                };

                _context.Apartments.Add(newApartment);
                await _context.SaveChangesAsync();

                // retrieve related property (before relationship change)
                var relatedProperty = await _context.Properties.FindAsync(id);

                await _context.Database.ExecuteSqlRawAsync("INSERT INTO PropertyApartments (PropertyId, ApartmentId) VALUES ({0}, {1})", id, newApartment.ApartmentId);
                await _context.SaveChangesAsync();

                if (relatedProperty != null)
                {
                    // after relationship change
                    var modifyProperty = await _context.Properties
                        .FindAsync(relatedProperty.PropertyId);

                    modifyProperty.TotalUnits += 1;

                    // update available units to number of units with status "A1"
                    int availableUnits = await _context.Apartments
                        .CountAsync(a => a.Properties.FirstOrDefault().PropertyId == modifyProperty.PropertyId && a.StatusId == "A1");

                    modifyProperty.AvailableUnits = availableUnits;

                    if (availableUnits == 0)
                    {
                        modifyProperty.StatusId = "P1";
                    }

                    _context.Properties.Update(modifyProperty);
                    await _context.SaveChangesAsync();

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

                HttpContext.Session.SetString("employeeID", employeeID);
                HttpContext.Session.SetString("jobID", jobID);


            }

            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || (jobID != "502" && jobID != "500"))
            {
                return View("AccessDenied");
            }


            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return View("Error");
            }

            var statuses = _context.Statuses
                .Where(s => s.StatusId.StartsWith("A"))
                .Select(s => new { StatusId = s.StatusId, Description = s.Description })
                .ToList();

            ViewData["StatusId"] = new SelectList(statuses, "StatusId", "Description", apartment.StatusId);

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);


            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ApartmentId,Bedrooms,Bathrooms,FloorArea,StatusId,Price")] ApartmentModel apartment)
        {
            if (id != apartment.ApartmentId)
            {
                return View("Error");
            }

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || (jobID != "502" && jobID != "500"))
            {
                return View("AccessDenied");
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var existingApartment = await _context.Apartments.FindAsync(apartment.ApartmentId);

                    if (existingApartment != null)
                    {
                        existingApartment.Bedrooms = apartment.Bedrooms;
                        existingApartment.Bathrooms = apartment.Bathrooms;
                        existingApartment.FloorArea = apartment.FloorArea;
                        existingApartment.StatusId = apartment.StatusId;
                        existingApartment.Price = apartment.Price;

                        await _context.SaveChangesAsync();

                        await UpdatePropertyUnits(existingApartment.ApartmentId);

                    }
                    else
                    {
                        return View("Error");
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentExists(apartment.ApartmentId))
                    {
                        return View("Error");
                    }
                    else
                    {
                        throw;
                    }
                }

                HttpContext.Session.SetString("employeeID", employeeID);
                HttpContext.Session.SetString("jobID", jobID);

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
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", apartment.StatusId);
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || (jobID != "502" && jobID != "500"))
            {
                return View("AccessDenied");
            }


            var apartment = await _context.Apartments
                .Include(a => a.Status)
                .FirstOrDefaultAsync(m => m.ApartmentId == id);

            if (apartment == null)
            {
                return View("Error");
            }

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);

            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var apartment = await _context.Apartments.FindAsync(id);

            if (apartment == null)
            {
                return View("Error");
            }

            try
            {
                // retrieve related property (before relationship change)
                var relatedProperty = await _context.Properties
                    .Include(p => p.Apartments)
                    .FirstOrDefaultAsync(p => p.Apartments.Any(a => a.ApartmentId == apartment.ApartmentId));

                // Manually delete records from PropertyApartments table
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM PropertyApartments WHERE ApartmentId = {0}", id);

                await _context.SaveChangesAsync();

                if (relatedProperty != null)
                {
                    // after relationship change
                    var modifyProperty = await _context.Properties
                        .FindAsync(relatedProperty.PropertyId);

                    modifyProperty.TotalUnits -= 1;

                    // Update available units to number of units with status "A1"
                    int availableUnits = await _context.Apartments
                        .CountAsync(a => a.Properties.FirstOrDefault().PropertyId == modifyProperty.PropertyId && a.StatusId == "A1");

                    modifyProperty.AvailableUnits = availableUnits;

                    _context.Properties.Update(relatedProperty);

                }              

                //_context.Apartments.Remove(apartment);

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
                return View("Error");
            }
        }


        private bool ApartmentExists(string id)
        {
            return _context.Apartments.Any(e => e.ApartmentId == id);
        }
    }
}
