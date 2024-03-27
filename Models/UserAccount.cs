using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class UserAccount
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserType { get; set; } = null!;

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();

    public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}
