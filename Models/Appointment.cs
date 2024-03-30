using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int ManagerId { get; set; }

    public string TenantId { get; set; } = null!;

    public int ScheduleId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public string StatusId { get; set; } = null!;

    public string ApartmentId { get; set; } = null!;

    public virtual Apartment Apartment { get; set; } = null!;

    public virtual Manager Manager { get; set; } = null!;

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
