using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FitnessHard.Models;

public partial class FitnessHardContext : DbContext
{
    public FitnessHardContext()
    {
    }

    public FitnessHardContext(DbContextOptions<FitnessHardContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<JournalService> JournalServices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServicesSubscription> ServicesSubscriptions { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<SubscriptionClient> SubscriptionClients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=HOME-PC\\ISA;Database=FitnessHard;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_CI_AS");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_CLIENT")
                .IsClustered(false);

            entity.ToTable("Client");

            entity.HasIndex(e => e.Phone, "CK_dboClient_Phone").IsUnique();

            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_EMPLOYEE")
                .IsClustered(false);

            entity.ToTable("Employee");

            entity.HasIndex(e => e.RoleId, "Post_Employee_FK");

            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sotrud_Role");
        });

        modelBuilder.Entity<JournalService>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_JOURNALSERVICES")
                .IsClustered(false);

            entity.HasIndex(e => e.SubscriptionClientId, "AbonClient_Journal_FK");

            entity.HasIndex(e => e.EmployeeId, "Employee_Journal_FK");

            entity.HasIndex(e => e.ServiceId, "Service_Journal_FK");

            entity.Property(e => e.Cost).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.DateUse).HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.JournalServiceEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JOURNALS_EMPLOYEE__EMPLOYEE");

            entity.HasOne(d => d.Service).WithMany(p => p.JournalServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_JOURNALS_SERVICE_J_SERVICE");

            entity.HasOne(d => d.SubscriptionClient).WithMany(p => p.JournalServices)
                .HasForeignKey(d => d.SubscriptionClientId)
                .HasConstraintName("FK_JOURNALS_ABONCLIEN_ABONEMET");

            entity.HasOne(d => d.Trainer).WithMany(p => p.JournalServiceTrainers)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JOURNALS_REFERENCE_EMPLOYEE");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_POST")
                .IsClustered(false);

            entity.ToTable("Role");

            entity.HasIndex(e => e.Title, "CK_dboPost").IsUnique();

            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.ToTable("Schedule");

            entity.Property(e => e.DayOfWeek).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Schedule_Employee");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_SERVICE")
                .IsClustered(false);

            entity.ToTable("Service");

            entity.Property(e => e.Information)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ServicesSubscription>(entity =>
        {
            entity.HasKey(e => new { e.SubscriptionId, e.ServiceId })
                .HasName("PK_SERVICESABONEMENT")
                .IsClustered(false);

            entity.ToTable("ServicesSubscription");

            entity.HasIndex(e => e.SubscriptionId, "Abonement_ServAbon_FK");

            entity.HasIndex(e => e.ServiceId, "Service_ServAbon_FK");

            entity.HasOne(d => d.Service).WithMany(p => p.ServicesSubscriptions)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_SERVICES_SERVICE_S_SERVICE");

            entity.HasOne(d => d.Subscription).WithMany(p => p.ServicesSubscriptions)
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("FK_SERVICES_ABONEMENT_ABONEMEN");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_ABONEMENT")
                .IsClustered(false);

            entity.ToTable("Subscription");

            entity.HasIndex(e => e.Title, "CK_dboAbonement").IsUnique();

            entity.HasIndex(e => e.Id, "ClusteredIndex-20180122-164312").IsClustered();

            entity.Property(e => e.IsUnlimited).HasColumnName("isUnlimited");
            entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.TimeEnd).HasColumnType("datetime");
            entity.Property(e => e.TimeStart).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SubscriptionClient>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_ABONEMETSCLIENT")
                .IsClustered(false);

            entity.ToTable("SubscriptionClient");

            entity.HasIndex(e => e.SubscriptionId, "Abonemets_AbonemClients_FK");

            entity.HasIndex(e => e.ClientId, "Client_AbonClients_FK");

            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.SubscriptionClients)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_ABONEMET_CLIENT_AB_CLIENT");

            entity.HasOne(d => d.Employee).WithMany(p => p.SubscriptionClients)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AbonemetsClient_Sotrud");

            entity.HasOne(d => d.Subscription).WithMany(p => p.SubscriptionClients)
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("FK_ABONEMET_ABONEMETS_ABONEMEN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
