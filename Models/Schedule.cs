using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
