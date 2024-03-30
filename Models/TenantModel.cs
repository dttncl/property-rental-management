namespace property_rental_management.Models
{
    public class TenantModel
    {
        public string TenantId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

    }
}
