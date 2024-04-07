using System.ComponentModel.DataAnnotations;

namespace property_rental_management.Models
{
    public class ManagerModel
    {

        // employee
        [Key]
        public int EmployeeId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }


        public int? SupervisorId { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        public int? CityId { get; set; }

        public string StatusId { get; set; }

    }
}
