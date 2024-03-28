using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using property_rental_management.Models;

namespace property_rental_management.Models;

public partial class RentaSpaceDbContext : DbContext
{
    public RentaSpaceDbContext()
    {
    }

    public RentaSpaceDbContext(DbContextOptions<RentaSpaceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Apartment> Apartments { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E8B0DEAF7D");

            entity.Property(e => e.AdminId)
                .ValueGeneratedNever()
                .HasColumnName("AdminID");
            entity.Property(e => e.Email).HasMaxLength(50);

            entity.HasOne(d => d.AdminNavigation).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Admins__AdminID__59063A47");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Admins)
                .HasPrincipalKey(p => p.Email)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Admins__Email__59FA5E80");
        });

        modelBuilder.Entity<Apartment>(entity =>
        {
            entity.HasKey(e => e.ApartmentId).HasName("PK__Apartmen__CBDF5744844CC0F2");

            entity.Property(e => e.ApartmentId)
                .HasMaxLength(5)
                .HasColumnName("ApartmentID");
            entity.Property(e => e.FloorArea).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StatusId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("StatusID");

            entity.HasOne(d => d.Status).WithMany(p => p.Apartments)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Apartment__Statu__619B8048");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA2E6E5CCE1");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.StatusId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("StatusID");
            entity.Property(e => e.TenantId)
                .HasMaxLength(5)
                .HasColumnName("TenantID");

            entity.HasOne(d => d.Manager).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Manag__09A971A2");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Sched__0B91BA14");

            entity.HasOne(d => d.Status).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Statu__0C85DE4D");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Tenan__0A9D95DB");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__F2D21A96185B58E3");

            entity.HasIndex(e => e.CityName, "UQ__Cities__886159E5C099DEE4").IsUnique();

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityName).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF14942793E");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.JobId).HasColumnName("JobID");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StatusId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("StatusID");
            entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

            entity.HasOne(d => d.Job).WithMany(p => p.Employees)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employees__JobID__4D94879B");

            entity.HasOne(d => d.Status).WithMany(p => p.Employees)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Statu__4E88ABD4");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.InverseSupervisor)
                .HasForeignKey(d => d.SupervisorId)
                .HasConstraintName("FK__Employees__Super__4F7CD00D");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__Jobs__056690E284A83BC5");

            entity.HasIndex(e => e.JobTitle, "UQ__Jobs__44C68B9F5AB489C9").IsUnique();

            entity.Property(e => e.JobId).HasColumnName("JobID");
            entity.Property(e => e.JobTitle).HasMaxLength(50);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.ManagerId).HasName("PK__Managers__3BA2AA81C07DBFB0");

            entity.Property(e => e.ManagerId)
                .ValueGeneratedNever()
                .HasColumnName("ManagerID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.Email).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.Managers)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Managers__CityID__5629CD9C");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Managers)
                .HasPrincipalKey(p => p.Email)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Managers__Email__5535A963");

            entity.HasOne(d => d.ManagerNavigation).WithOne(p => p.Manager)
                .HasForeignKey<Manager>(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Managers__Manage__5441852A");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C037CDCB956B4");

            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Message1)
                .HasMaxLength(250)
                .HasColumnName("Message");
            entity.Property(e => e.TenantId)
                .HasMaxLength(5)
                .HasColumnName("TenantID");

            entity.HasOne(d => d.Manager).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Manage__17036CC0");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Messages)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Tenant__17F790F9");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.PropertyId).HasName("PK__Properti__70C9A755D0D4FCBC");

            entity.Property(e => e.PropertyId)
                .HasMaxLength(5)
                .HasColumnName("PropertyID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.StatusId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("StatusID");
            entity.Property(e => e.YearEstablished).HasMaxLength(4);

            entity.HasOne(d => d.City).WithMany(p => p.Properties)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__Propertie__CityI__5CD6CB2B");

            entity.HasOne(d => d.Status).WithMany(p => p.Properties)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Propertie__Statu__5DCAEF64");

            entity.HasMany(d => d.Apartments).WithMany(p => p.Properties)
                .UsingEntity<Dictionary<string, object>>(
                    "PropertyApartment",
                    r => r.HasOne<Apartment>().WithMany()
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PropertyA__Apart__74AE54BC"),
                    l => l.HasOne<Property>().WithMany()
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PropertyA__Prope__73BA3083"),
                    j =>
                    {
                        j.HasKey("PropertyId", "ApartmentId").HasName("PK__Property__2C745221A5098A8D");
                        j.ToTable("PropertyApartments");
                        j.IndexerProperty<string>("PropertyId")
                            .HasMaxLength(5)
                            .HasColumnName("PropertyID");
                        j.IndexerProperty<string>("ApartmentId")
                            .HasMaxLength(5)
                            .HasColumnName("ApartmentID");
                    });

            entity.HasMany(d => d.Managers).WithMany(p => p.Properties)
                .UsingEntity<Dictionary<string, object>>(
                    "PropertyManager",
                    r => r.HasOne<Manager>().WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PropertyM__Manag__00200768"),
                    l => l.HasOne<Property>().WithMany()
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PropertyM__Prope__7F2BE32F"),
                    j =>
                    {
                        j.HasKey("PropertyId", "ManagerId").HasName("PK__Property__73738DFD350C6D76");
                        j.ToTable("PropertyManagers");
                        j.IndexerProperty<string>("PropertyId")
                            .HasMaxLength(5)
                            .HasColumnName("PropertyID");
                        j.IndexerProperty<int>("ManagerId").HasColumnName("ManagerID");
                    });
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B699EA15F7B");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Status__C8EE2043051FAC76");

            entity.ToTable("Status");

            entity.Property(e => e.StatusId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("StatusID");
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.TenantId).HasName("PK__Tenants__2E9B47813EAC7E67");

            entity.Property(e => e.TenantId)
                .HasMaxLength(5)
                .HasColumnName("TenantID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(10);

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Tenants)
                .HasPrincipalKey(p => p.Email)
                .HasForeignKey(d => d.Email)
                .HasConstraintName("FK__Tenants__Email__3C69FB99");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserAccountId).HasName("PK__UserAcco__A9D10535CD230647");

            entity.HasIndex(e => e.Email, "UQ__tmp_ms_x__A9D10534658E18A3").IsUnique();

            entity.Property(e => e.UserAccountId).HasColumnName("UserAccount_Id");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserType).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<property_rental_management.Models.BookAppointment> BookAppointment { get; set; } = default!;
}
