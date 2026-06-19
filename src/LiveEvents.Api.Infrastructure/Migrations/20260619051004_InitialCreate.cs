using System;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveEvents.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "events");

            migrationBuilder.EnsureSchema(
                name: "authentication");

            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.Sql("""
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_type t
        JOIN pg_namespace n ON n.oid = t.typnamespace
        WHERE t.typname = 'user_status' AND n.nspname = 'authentication'
    ) THEN
        CREATE TYPE authentication.user_status AS ENUM ('Activo', 'Suspendido', 'PendienteVerificacion');
    END IF;
END
$$;
""");

            migrationBuilder.Sql("""
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_type t
        JOIN pg_namespace n ON n.oid = t.typnamespace
        WHERE t.typname = 'events_status' AND n.nspname = 'events'
    ) THEN
        CREATE TYPE events.events_status AS ENUM ('Activo', 'Cancelado', 'Completado');
    END IF;
END
$$;
""");

            migrationBuilder.Sql("""
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_type t
        JOIN pg_namespace n ON n.oid = t.typnamespace
        WHERE t.typname = 'events_types' AND n.nspname = 'events'
    ) THEN
        CREATE TYPE events.events_types AS ENUM ('Conferencia', 'Taller', 'Concierto');
    END IF;
END
$$;
""");

            migrationBuilder.Sql("""
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_type t
        JOIN pg_namespace n ON n.oid = t.typnamespace
        WHERE t.typname = 'reservations_status' AND n.nspname = 'events'
    ) THEN
        CREATE TYPE events.reservations_status AS ENUM ('PendientePago', 'Confirmada', 'Cancelada', 'Perdida');
    END IF;
END
$$;
""");

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "authentication",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Label = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    Route = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsGroup = table.Column<bool>(type: "boolean", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Menus__3213E83F", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "authentication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", maxLength: 500, nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Permissions_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "authentication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Roles_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_types",
                schema: "authentication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    theme = table.Column<string>(type: "character varying", nullable: true),
                    default_landing_page = table.Column<string>(type: "character varying", nullable: true),
                    logo_url = table.Column<string>(type: "character varying", nullable: true),
                    language = table.Column<string>(type: "character varying", nullable: true),
                    additional_config = table.Column<string>(type: "character varying", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserTypes_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venues",
                schema: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Venues_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                schema: "authentication",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permission_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RolePermissions_pkey", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "RolePermissions_PermissionId_fkey",
                        column: x => x.permission_id,
                        principalSchema: "authentication",
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "RolePermissions_RoleId_fkey",
                        column: x => x.role_id,
                        principalSchema: "authentication",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "authentication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "character varying", nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    extra_data = table.Column<string>(type: "text", nullable: false, defaultValueSql: "'{}'::text"),
                    status = table.Column<UserStatus>(type: "authentication.user_status", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Users_UserTypeId_fkey",
                        column: x => x.user_type_id,
                        principalSchema: "authentication",
                        principalTable: "user_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "events",
                schema: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    venue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    max_capacity = table.Column<int>(type: "integer", nullable: false),
                    starts_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ends_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ticket_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    type = table.Column<EventType>(type: "events.events_types", nullable: false),
                    status = table.Column<EventStatus>(type: "events.events_status", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Events_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Events_VenueId_fkey",
                        column: x => x.venue_id,
                        principalSchema: "events",
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                schema: "authentication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_token = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sessions_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Sessions_UserId_fkey",
                        column: x => x.user_id,
                        principalSchema: "authentication",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_notifications",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    channel = table.Column<int>(type: "integer", nullable: false),
                    types = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: true),
                    read_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserNotifications_pkey", x => x.id);
                    table.ForeignKey(
                        name: "UserNotifications_UserId_fkey",
                        column: x => x.user_id,
                        principalSchema: "authentication",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                schema: "authentication",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserRoles_pkey", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "UserRoles_RoleId_fkey",
                        column: x => x.role_id,
                        principalSchema: "authentication",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "UserRoles_UserId_fkey",
                        column: x => x.user_id,
                        principalSchema: "authentication",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                schema: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    buyer_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    buyer_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    status = table.Column<ReservationStatus>(type: "events.reservations_status", nullable: false),
                    reservation_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    paid_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Reservations_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Reservations_EventId_fkey",
                        column: x => x.event_id,
                        principalSchema: "events",
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_Venue_Start",
                schema: "events",
                table: "events",
                columns: new[] { "venue_id", "starts_at" });

            migrationBuilder.CreateIndex(
                name: "UQ_Permissions_Name",
                schema: "authentication",
                table: "permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservations_event_id",
                schema: "events",
                table: "reservations",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Reservations_Code",
                schema: "events",
                table: "reservations",
                column: "reservation_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_permission_id",
                schema: "authentication",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Roles_Name",
                schema: "authentication",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessions_user_id",
                schema: "authentication",
                table: "sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_notifications_user_id",
                schema: "notification",
                table: "user_notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                schema: "authentication",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "unique name",
                schema: "authentication",
                table: "user_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_user_type_id",
                schema: "authentication",
                table: "users",
                column: "user_type_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Users_Email",
                schema: "authentication",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Venues_Name",
                schema: "events",
                table: "venues",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menus",
                schema: "authentication");

            migrationBuilder.DropTable(
                name: "reservations",
                schema: "events");

            migrationBuilder.DropTable(
                name: "role_permissions",
                schema: "authentication");

            migrationBuilder.DropTable(
                name: "sessions",
                schema: "authentication");

            migrationBuilder.DropTable(
                name: "user_notifications",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "user_roles",
                schema: "authentication");

            migrationBuilder.DropTable(
                name: "events",
                schema: "events");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "authentication");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "authentication");

            migrationBuilder.DropTable(
                name: "users",
                schema: "authentication");

            migrationBuilder.DropTable(
                name: "venues",
                schema: "events");

            migrationBuilder.DropTable(
                name: "user_types",
                schema: "authentication");
        }
    }
}
