using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int? JobId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public decimal Salary { get; set; }

    public string StatusId { get; set; } = null!;

    public int? SupervisorId { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual ICollection<Employee> InverseSupervisor { get; set; } = new List<Employee>();

    public virtual Job? Job { get; set; }

    public virtual Manager? Manager { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual Employee? Supervisor { get; set; }
}
