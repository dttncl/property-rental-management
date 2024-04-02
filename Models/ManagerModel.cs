using System.ComponentModel.DataAnnotations;

namespace property_rental_management.Models
{
    public class ManagerModel
    {

        // employee
        [Key]
        public int EmployeeId { get; set; }

        //public int? JobId { get; set; } // 502 - manager

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public decimal Salary { get; set; }

        //public string StatusId { get; set; } = null!; // E1 - active

        public int? SupervisorId { get; set; }

        // useraccount
        public string Password { get; set; } = null!;

        // manager
        //public int ManagerId { get; set; } // same as employeeId

        public string Email { get; set; } = null!;

        public int? CityId { get; set; }
         
    }
}
