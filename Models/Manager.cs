using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Manager
{
    public int ManagerId { get; set; }

    public string Email { get; set; } = null!;

    public int? CityId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual City? City { get; set; }

    public virtual UserAccount EmailNavigation { get; set; } = null!;

    public virtual Employee ManagerNavigation { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
