using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string Email { get; set; } = null!;

    public virtual Employee AdminNavigation { get; set; } = null!;

    public virtual UserAccount EmailNavigation { get; set; } = null!;
}
