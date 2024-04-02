using System.ComponentModel.DataAnnotations;

namespace property_rental_management.Models
{
    public class Msg
    {
        [Key]
        public int MessageId { get; set; }

        [Display(Name = "From")]
        public string Sender { get; set; } = null!;

        [Display(Name = "To")]
        public string Receiver { get; set; } = null!;

        [Display(Name = "Message")]
        public string Message1 { get; set; } = null!;

        [Display(Name = "Subject")]
        public string Subject { get; set; } = null!;
    }
}
