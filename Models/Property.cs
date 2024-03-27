using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Property
{
    public string PropertyId { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int CityId { get; set; }

    public string YearEstablished { get; set; } = null!;

    public int TotalUnits { get; set; }

    public int AvailableUnits { get; set; }

    public string StatusId { get; set; } = null!;

    public virtual City City { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();

    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();
}
