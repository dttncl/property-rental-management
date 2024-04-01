namespace property_rental_management.Models
{
    public class ApartmentModel
    {
        public string ApartmentId { get; set; } = null!;

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public decimal FloorArea { get; set; }

        public string StatusId { get; set; } = null!;

        public decimal Price { get; set; }

    }
}
