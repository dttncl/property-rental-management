using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Status
{
    public string StatusId { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
