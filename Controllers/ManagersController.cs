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

        // GET:  Managers/Messages/5
        public async Task<IActionResult> Messages(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messages = await _context.Messages
                .Where(sr => sr.Sender == id || sr.Receiver == id)
                .ToListAsync();

            messages.Reverse();

            List<Message> formattedMessages = new List<Message>();

            foreach (var msg in messages)
            {
                Generator gen = new Generator(_context);
                var senderDetails = await gen.GetUserDetails(msg.Sender);
                var receiverDetails = await gen.GetUserDetails(msg.Receiver);

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

            return View(formattedMessages);
        }

        // GET:  Managers/Appointments/5
        public async Task<IActionResult> Appointments(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointments = await _context.Appointments
                .Where(man => man.ManagerId.ToString() == id)
                .Include(t => t.Tenant)
                .Include(a => a.Apartment)
                    .ThenInclude(p => p.Properties)
                .Include(sc => sc.Schedule)
                .Include(s => s.Status)
                .OrderByDescending(app => app.AppointmentDate)
                .ToListAsync();

            return View(appointments);
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
            var rentaSpaceDbContext = _context.Managers
                .Include(m => m.City)
                .Include(m => m.EmailNavigation)
                .Include(m => m.ManagerNavigation)
                    .ThenInclude(s => s.Status);
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
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");

            var supervisors = _context.Employees
                .Where(e => e.JobId == 501)
                .Select(e => new
                {
                    EmployeeId = e.EmployeeId,
                    FullName = $"{e.FirstName} {e.LastName}"
                })
                .ToList();

            ViewData["SupervisorId"] = new SelectList(supervisors, "EmployeeId", "FullName");

            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManagerModel manager)
        {
            if (ModelState.IsValid)
            {
                UserAccount newAccount = new UserAccount
                {
                    Email = manager.Email,
                    Password = manager.Password,
                    UserType = "Employee"
                };

                _context.UserAccounts.Add(newAccount);
                await _context.SaveChangesAsync();

                Employee newEmployee = new Employee
                {
                    JobId = 502,
                    FirstName = manager.FirstName,
                    LastName = manager.LastName,
                    Email = manager.Email,
                    Phone = manager.Phone,
                    Salary = manager.Salary,
                    StatusId = "E1",
                    SupervisorId = manager.SupervisorId
                };

                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();

                var newEmp = await _context.Employees
                                .Where(e => e.Email == newEmployee.Email)
                                .FirstOrDefaultAsync();

                Manager newManager = new Manager
                {
                    ManagerId = newEmp.EmployeeId,
                    Email = manager.Email,
                    CityId = manager.CityId
                };

                _context.Managers.Add(newManager);
                await _context.SaveChangesAsync();

                var propertiesInCity = await _context.Properties
                                        .Where(p => p.CityId == manager.CityId)
                                        .ToListAsync();

                foreach (var property in propertiesInCity)
                {
                    string insertSql = @"INSERT INTO PropertyManagers (PropertyId, ManagerId) VALUES ({0}, {1});";
                    await _context.Database.ExecuteSqlRawAsync(insertSql, property.PropertyId, newManager.ManagerId);
                }

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

            return View(manager);
        }

        // GET: Managers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .Include(m => m.ManagerNavigation)
                .Include(e => e.EmailNavigation)
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
            {
                return NotFound();
            }

            ManagerModel modifyManager = new ManagerModel
            {
                EmployeeId = (int)id,
                FirstName = manager.ManagerNavigation.FirstName,
                LastName = manager.ManagerNavigation.LastName,
                Phone = manager.ManagerNavigation.Phone,
                Salary = manager.ManagerNavigation.Salary,
                SupervisorId = manager.ManagerNavigation.SupervisorId,
                Password = manager.EmailNavigation.Password,
                Email = manager.EmailNavigation.Email,
                CityId = manager.CityId,
                StatusId = manager.ManagerNavigation.StatusId
            };

            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");

            var supervisors = _context.Employees
                .Where(e => e.JobId == 501)
                .Select(e => new
                {
                    EmployeeId = e.EmployeeId,
                    FullName = $"{e.FirstName} {e.LastName}"
                })
                .ToList();

            ViewData["SupervisorId"] = new SelectList(supervisors, "EmployeeId", "FullName");

            var filteredStatuses = _context.Statuses
                                    .Where(s => s.StatusId.StartsWith("E"))
                                    .ToList();

            // Create SelectList with filtered Statuses
            ViewData["StatusId"] = new SelectList(filteredStatuses, "StatusId", "Description");

            return View(modifyManager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManagerModel manager)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // before update
                    var existingManager = await _context.Managers
                                    .Include(m => m.ManagerNavigation)
                                    .Include(e => e.EmailNavigation)
                                    .FirstOrDefaultAsync(m => m.ManagerId == manager.EmployeeId);

                    // after update
                    // employee
                    var existingEmployee = await _context.Employees
                                .FirstOrDefaultAsync(e => e.Email == manager.Email);

                    existingEmployee.FirstName = manager.FirstName;
                    existingEmployee.LastName = manager.LastName;
                    existingEmployee.Phone = manager.Phone;
                    existingEmployee.Salary = manager.Salary;
                    existingEmployee.SupervisorId = manager.SupervisorId;
                    existingEmployee.StatusId = manager.StatusId;

                    _context.Employees.Update(existingEmployee);
                    await _context.SaveChangesAsync();

                    // manager
                    if (existingManager.CityId != manager.CityId)
                    {
                        existingManager.CityId = manager.CityId;

                        _context.Managers.Update(existingManager);
                        await _context.SaveChangesAsync();

                        // delete old manage properties
                        string sql = $"DELETE FROM PropertyManagers WHERE ManagerId = {manager.EmployeeId}";
                        await _context.Database.ExecuteSqlRawAsync(sql);

                        var newManageProperties = await _context.Properties
                                        .Where(p => p.CityId == manager.CityId)
                                        .ToListAsync();

                        foreach (var property in newManageProperties)
                        {
                            string insertSql = @"INSERT INTO PropertyManagers (PropertyId, ManagerId) VALUES ({0}, {1});";
                            await _context.Database.ExecuteSqlRawAsync(insertSql, property.PropertyId, manager.EmployeeId);
                        }

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

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

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
                .Include(c => c.City)
                .Include(m => m.ManagerNavigation)
                    .ThenInclude(x => x.Supervisor)
                .Include(e => e.EmailNavigation)
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
            {
                return NotFound();
            }

            ManagerModel modifyManager = new ManagerModel
            {
                EmployeeId = (int)id,
                FirstName = manager.ManagerNavigation.FirstName,
                LastName = manager.ManagerNavigation.LastName,
                Phone = manager.ManagerNavigation.Phone,
                Salary = manager.ManagerNavigation.Salary,
                SupervisorId = manager.ManagerNavigation.SupervisorId,
                Password = manager.EmailNavigation.Password,
                Email = manager.EmailNavigation.Email,
                CityId = manager.CityId
            };

            ViewData["CityName"] = manager.City.CityName;
            ViewData["SupervisorName"] = $"{manager.ManagerNavigation.Supervisor.FirstName} {manager.ManagerNavigation.Supervisor.LastName}";

            return View(modifyManager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                employee.StatusId = "E2";
                _context.Employees.Update(employee);
            }

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

        private bool ManagerExists(int id)
        {
            return _context.Managers.Any(e => e.ManagerId == id);
        }
    }
}
