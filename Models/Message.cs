using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int ManagerId { get; set; }

    public string TenantId { get; set; } = null!;

    public string Message1 { get; set; } = null!;

    public virtual Manager Manager { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
