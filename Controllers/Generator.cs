using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using property_rental_management.Models;

namespace property_rental_management.Controllers
{
    public class Generator
    {
        private readonly RentaSpaceDbContext _context;

        public Generator(RentaSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetUserDetails(string userId)
        {
            bool isManager = await _context.Managers
                .AnyAsync(m => m.ManagerId.ToString() == userId);

            if (isManager)
            {
                var managerDetails = await _context.Employees
                    .FirstOrDefaultAsync(m => m.EmployeeId.ToString() == userId);

                return $"{managerDetails?.FirstName} {managerDetails?.LastName}";

            }
            else
            {
                var tenantDetails = await _context.Tenants
                    .FirstOrDefaultAsync(t => t.TenantId == userId);

                return $"{tenantDetails?.FirstName} {tenantDetails?.LastName}";
            }
        }

    }
}
