using System.ComponentModel.DataAnnotations;

namespace property_rental_management.Models
{
    public class BookAppointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public int ManagerId { get; set; }

        public string TenantId { get; set; } = null!;

        public int ScheduleId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly AppointmentDate { get; set; }

        public string StatusId { get; set; } = null!;

        public string ApartmentId { get; set; } = null!;

    }
}
