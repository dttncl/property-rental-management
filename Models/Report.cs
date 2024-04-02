using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int ManagerId { get; set; }

    public int AdminId { get; set; }

    public string Message { get; set; } = null!;

    public virtual Admin Admin { get; set; } = null!;

    public virtual Manager Manager { get; set; } = null!;
}
