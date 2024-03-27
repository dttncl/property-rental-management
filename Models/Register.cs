using System.ComponentModel.DataAnnotations;

namespace property_rental_management.Models
{
    public class Register
    {
        
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        [Key]
        [Required(ErrorMessage = "Email field is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone field is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
