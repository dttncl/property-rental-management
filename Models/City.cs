using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
