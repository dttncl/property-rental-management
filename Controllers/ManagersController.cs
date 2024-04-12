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
            } else
            {
                userDetails = "[Account Not Found]";
            }

            return userDetails;
        }

        // GET: Managers
        public async Task<IActionResult> Index(string s)
        {

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || jobID != "500")
            {
                return View("AccessDenied");
            }

            var managers = _context.Managers.AsQueryable();

            if (!string.IsNullOrEmpty(s))
            {
                managers = _context.Managers
                                .Where(q =>
                                    q.ManagerNavigation.FirstName.Contains(s) ||
                                    q.ManagerNavigation.LastName.Contains(s) ||
                                    q.Email.Contains(s) ||
                                    q.City.CityName.Contains(s))
                                .Include(m => m.City)
                                .Include(m => m.EmailNavigation)
                                .Include(m => m.ManagerNavigation)
                                    .ThenInclude(s => s.Status);

            }
            else
            {
                managers = _context.Managers
                                .Include(m => m.City)
                                .Include(m => m.EmailNavigation)
                                .Include(m => m.ManagerNavigation)
                                    .ThenInclude(s => s.Status);
            }

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);

            return View(await managers.ToListAsync());
        }

        // GET:  Managers/Messages/id
        public async Task<IActionResult> Messages(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || jobID != "502")
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

        // GET:  Managers/Appointments/id
        public async Task<IActionResult> Appointments(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || jobID != "502")
            {
                return View("AccessDenied");
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

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);

            return View(appointments);
        }

        // GET: Managers/Listings/id
        // display manager dashboard
        public async Task<IActionResult> Listings(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || jobID != "502")
            {
                return View("AccessDenied");
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
                return View("Error");
            }

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);

            return View(manager);
        }

        

        // GET: Managers/Details/id
        public async Task<IActionResult> Details(int? id)
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

            var manager = await _context.Managers
                .Include(m => m.City)
                .Include(m => m.EmailNavigation)
                .Include(m => m.ManagerNavigation)
                    .ThenInclude(m => m.Supervisor)
                .Include(m => m.ManagerNavigation)
                    .ThenInclude(m => m.Status)
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
            {
                return View("Error");
            }

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);


            return View(manager);
        }

        // GET: Managers/Create
        public IActionResult Create()
        {

            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || jobID != "500")
            {
                return View("AccessDenied");
            }

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

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);


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

                var employeeID = HttpContext.Session.GetString("employeeID");
                var jobID = HttpContext.Session.GetString("jobID");

                if (employeeID == null || jobID != "500")
                {
                    return View("AccessDenied");
                }

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

            return View(manager);
        }

        // GET: Managers/Edit/id
        public async Task<IActionResult> Edit(int? id)
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

            var manager = await _context.Managers
                .Include(m => m.ManagerNavigation)
                .Include(e => e.EmailNavigation)
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
            {
                return View("Error");
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

            ViewData["StatusId"] = new SelectList(filteredStatuses, "StatusId", "Description");

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);

            return View(modifyManager);
        }

        // POST: Managers/Edit/id
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManagerModel manager)
        {
            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || jobID != "500")
            {
                return View("AccessDenied");
            }

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
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.EmployeeId))
                    {
                        return View("Error");
                    }
                    else
                    {
                        throw;
                    }
                }
            }



            return View(manager);
        }

        // GET: Managers/Delete/id
        public async Task<IActionResult> Delete(int? id)
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

            var manager = await _context.Managers
                .Include(c => c.City)
                .Include(m => m.ManagerNavigation)
                    .ThenInclude(x => x.Supervisor)
                .Include(e => e.EmailNavigation)
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
            {
                return View("Error");
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

            HttpContext.Session.SetString("employeeID", employeeID);
            HttpContext.Session.SetString("jobID", jobID);


            return View(modifyManager);
        }

        // POST: Managers/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeeID = HttpContext.Session.GetString("employeeID");
            var jobID = HttpContext.Session.GetString("jobID");

            if (employeeID == null || jobID != "500")
            {
                return View("AccessDenied");
            }


            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                employee.StatusId = "E2";
                _context.Employees.Update(employee);
            }

            await _context.SaveChangesAsync();

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

        private bool ManagerExists(int id)
        {
            return _context.Managers.Any(e => e.ManagerId == id);
        }
    }
}
