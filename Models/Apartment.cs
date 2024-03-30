using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Apartment
{
    public string ApartmentId { get; set; } = null!;

    public int Bedrooms { get; set; }

    public int Bathrooms { get; set; }

    public decimal FloorArea { get; set; }

    public string StatusId { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
