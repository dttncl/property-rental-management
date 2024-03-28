using System.ComponentModel.DataAnnotations;

namespace property_rental_management.Models
{
    public class Msg
    {
        [Key]
        public int MessageId { get; set; }

        [Display(Name = "Manager ID")]
        public int ManagerId { get; set; }

        [Display(Name = "Tenant ID")]
        public string TenantId { get; set; } = null!;

        [Display(Name = "Message")]
        public string Message1 { get; set; } = null!;
    }
}
