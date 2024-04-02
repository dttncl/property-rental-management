namespace property_rental_management.Models
{
    public class PropertyModel
    {
        public string PropertyId { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int CityId { get; set; }

        public string YearEstablished { get; set; } = null!;

        public int TotalUnits { get; set; }

        public int AvailableUnits { get; set; }

        public string StatusId { get; set; } = null!;
    }
}
