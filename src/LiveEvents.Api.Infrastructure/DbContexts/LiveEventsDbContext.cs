using Microsoft.EntityFrameworkCore;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Entities.Notification;

namespace LiveEvents.Api.Infrastructure.DbContexts;

public class LiveEventsDbContext : DbContext
{
    public LiveEventsDbContext(DbContextOptions<LiveEventsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RolePermission> RolePermissions { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<UserType> UserTypes { get; set; }
    public virtual DbSet<UserNotification> UserNotifications { get; set; }
    public virtual DbSet<Venue> Venues { get; set; }
    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Declarar enums de PostgreSQL en el esquema authentication
        modelBuilder.HasPostgresEnum<UserStatus>("authentication", "user_status");
        modelBuilder.HasPostgresEnum<EventType>("events", "events_types");
        modelBuilder.HasPostgresEnum<EventStatus>("events", "events_status");
        modelBuilder.HasPostgresEnum<ReservationStatus>("events", "reservations_status");

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Permissions_pkey");

            entity.ToTable("permissions", "authentication");

            entity.HasIndex(e => e.Name, "UQ_Permissions_Name").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasColumnName("status");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pkey");

            entity.ToTable("roles", "authentication");

            entity.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            // Relación configurada através de la entidad RolePermission explícita
            // No necesitamos configuración muchos-a-muchos automática
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Sessions_pkey");

            entity.ToTable("sessions", "authentication");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Expires)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expires");
            entity.Property(e => e.SessionToken)
                .HasColumnName("session_token");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Sessions_UserId_fkey");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserTypes_pkey");

            entity.ToTable("user_types", "authentication");

            entity.HasIndex(e => e.Name, "unique name").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Status)
                .HasColumnName("status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Theme)
                .HasColumnType("character varying")
                .HasColumnName("theme");
            entity.Property(e => e.DefaultLandingPage)
                .HasColumnType("character varying")
                .HasColumnName("default_landing_page");
            entity.Property(e => e.LogoUrl)
                .HasColumnType("character varying")
                .HasColumnName("logo_url");
            entity.Property(e => e.Language)
                .HasColumnType("character varying")
                .HasColumnName("language");
            entity.Property(e => e.AdditionalConfig)
                .HasColumnType("character varying")
                .HasColumnName("additional_config");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.ToTable("users", "authentication");

            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnName("name");
            entity.Property(e => e.Address)
                .HasColumnType("character varying")
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .HasColumnName("password");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.UserTypeId)
                .HasColumnName("user_type_id");
            entity.Property(e => e.ExtraData)
                .HasColumnType("text")
                .HasColumnName("extra_data")
                .HasDefaultValueSql("'{}'::text");

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("user_status");

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .HasConstraintName("Users_UserTypeId_fkey");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<UserRole>(
                    l => l.HasOne<Role>(ur => ur.Role).WithMany()
                        .HasForeignKey(ur => ur.RoleId)
                        .HasConstraintName("UserRoles_RoleId_fkey"),
                    r => r.HasOne<User>(ur => ur.User).WithMany()
                        .HasForeignKey(ur => ur.UserId)
                        .HasConstraintName("UserRoles_UserId_fkey"),
                    j =>
                    {
                        j.HasKey(ur => new { ur.UserId, ur.RoleId }).HasName("UserRoles_pkey");
                        j.ToTable("UserRoles", "authentication");
                    });
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("RolePermissions_pkey");

            entity.ToTable("role_permissions", "authentication");

            entity.Property(e => e.RoleId)
                .HasColumnName("role_id");
            entity.Property(e => e.PermissionId)
                .HasColumnName("permission_id");

            entity.HasOne(d => d.Role).WithMany(r => r.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RolePermissions_RoleId_fkey");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RolePermissions_PermissionId_fkey");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("UserRoles_pkey");

            entity.ToTable("user_roles", "authentication");
            entity.Property(e => e.RoleId)
                .HasColumnName("role_id");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menus__3213E83F");

            entity.ToTable("Menus", "authentication");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("Id");
            entity.Property(e => e.Label)
                .IsRequired()
                .HasColumnName("Label");
            entity.Property(e => e.Icon)
                .IsRequired()
                .HasColumnName("Icon");
            entity.Property(e => e.Route)
                .IsRequired()
                .HasColumnName("Route");
            entity.Property(e => e.Order)
                .HasColumnName("Order");
            entity.Property(e => e.IsGroup)
                .HasColumnName("IsGroup");
            entity.Property(e => e.ParentId)
                .HasColumnName("ParentId");
            entity.Property(e => e.Status)
                .HasColumnName("Status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("CreatedAt");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("UpdatedAt");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Venues_pkey");

            entity.ToTable("venues", "events");

            entity.HasIndex(e => e.Name, "UQ_Venues_Name").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Capacity)
                .HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Events_pkey");

            entity.ToTable("events", "events");

            entity.HasIndex(e => new { e.VenueId, e.StartsAt }, "IX_Events_Venue_Start");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.VenueId)
                .HasColumnName("venue_id");
            entity.Property(e => e.MaxCapacity)
                .HasColumnName("max_capacity");
            entity.Property(e => e.StartsAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("starts_at");
            entity.Property(e => e.EndsAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("ends_at");
            entity.Property(e => e.TicketPrice)
                .HasColumnType("numeric(10,2)")
                .HasColumnName("ticket_price");
            entity.Property(e => e.Type)
                .HasColumnType("events.events_types")
                .HasColumnName("type");
            entity.Property(e => e.Status)
                .HasColumnType("events.events_status")
                .HasColumnName("status");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");

            entity.HasOne(d => d.Venue)
                .WithMany(p => p.Events)
                .HasForeignKey(d => d.VenueId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Events_VenueId_fkey");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Reservations_pkey");

            entity.ToTable("reservations", "events");

            entity.HasIndex(e => e.ReservationCode, "UQ_Reservations_Code").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.EventId)
                .HasColumnName("event_id");
            entity.Property(e => e.Quantity)
                .HasColumnName("quantity");
            entity.Property(e => e.BuyerName)
                .HasMaxLength(150)
                .HasColumnName("buyer_name");
            entity.Property(e => e.BuyerEmail)
                .HasMaxLength(255)
                .HasColumnName("buyer_email");
            entity.Property(e => e.Status)
                .HasColumnType("events.reservations_status")
                .HasColumnName("status");
            entity.Property(e => e.ReservationCode)
                .HasMaxLength(20)
                .HasColumnName("reservation_code");
            entity.Property(e => e.PaidAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("paid_at");
            entity.Property(e => e.CancelledAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("cancelled_at");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");

            entity.HasOne(d => d.Event)
                .WithMany(p => p.Reservations)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Reservations_EventId_fkey");
        });

        modelBuilder.Entity<UserNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserNotifications_pkey");

            entity.ToTable("user_notifications", "notification");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");
            entity.Property(e => e.Message)
                .HasMaxLength(2000)
                .HasColumnName("message");
            entity.Property(e => e.Channel)
                .HasConversion<int>()
                .HasColumnName("channel");
            entity.Property(e => e.Types)
                .HasConversion<int>()
                .HasColumnName("types");
            entity.Property(e => e.Status)
                .HasConversion<int>()
                .HasColumnName("status");
            entity.Property(e => e.Metadata)
                .HasColumnType("text")
                .HasColumnName("metadata");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.ReadAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("read_at");

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("UserNotifications_UserId_fkey");
        });

        base.OnModelCreating(modelBuilder);
    }
}
