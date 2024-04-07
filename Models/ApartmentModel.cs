using System.ComponentModel.DataAnnotations;

namespace property_rental_management.Models
{
    public class ApartmentModel
    {
        public string ApartmentId { get; set; } = null!;

        [Required(ErrorMessage = "This field is required")]
        public int Bedrooms { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int Bathrooms { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public decimal FloorArea { get; set; }

        public string StatusId { get; set; } = null!;

        [Required(ErrorMessage = "This field is required")]
        public decimal Price { get; set; }

    }
}
