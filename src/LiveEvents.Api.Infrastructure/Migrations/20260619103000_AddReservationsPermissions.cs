using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LiveEvents.Api.Infrastructure.DbContexts;

#nullable disable

namespace LiveEvents.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(LiveEventsDbContext))]
    [Migration("20260619103000_AddReservationsPermissions")]
    public partial class AddReservationsPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
INSERT INTO authentication.permissions (id, name, description, status, created_at, updated_at)
VALUES
        ('c12b4a8d-5d9f-4b85-a3ce-4d5ed9f5b101'::uuid, $seed$reservations.read$seed$, $seed$Consultar reservas propias del usuario.$seed$, TRUE, NOW(), NULL),
        ('f4e58f59-e8da-47df-b733-03d5ed0d4102'::uuid, $seed$reservations.create$seed$, $seed$Crear reservas para eventos.$seed$, TRUE, NOW(), NULL),
        ('bc6e3cd3-cf0d-487c-a883-b82701d26b03'::uuid, $seed$reservations.cancel$seed$, $seed$Cancelar reservas existentes.$seed$, TRUE, NOW(), NULL),
        ('3cc6ae1e-431f-4f98-8e31-58b16be2db04'::uuid, $seed$reservations.confirm_payment$seed$, $seed$Confirmar el pago administrativo de una reserva.$seed$, TRUE, NOW(), NULL)
ON CONFLICT (id) DO NOTHING;
""");

            migrationBuilder.Sql("""
INSERT INTO authentication.role_permissions (role_id, permission_id)
VALUES
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'c12b4a8d-5d9f-4b85-a3ce-4d5ed9f5b101'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'f4e58f59-e8da-47df-b733-03d5ed0d4102'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'bc6e3cd3-cf0d-487c-a883-b82701d26b03'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '3cc6ae1e-431f-4f98-8e31-58b16be2db04'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'c12b4a8d-5d9f-4b85-a3ce-4d5ed9f5b101'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'f4e58f59-e8da-47df-b733-03d5ed0d4102'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'bc6e3cd3-cf0d-487c-a883-b82701d26b03'::uuid)
ON CONFLICT (role_id, permission_id) DO NOTHING;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
DELETE FROM authentication.role_permissions
WHERE permission_id IN (
    'c12b4a8d-5d9f-4b85-a3ce-4d5ed9f5b101'::uuid,
    'f4e58f59-e8da-47df-b733-03d5ed0d4102'::uuid,
    'bc6e3cd3-cf0d-487c-a883-b82701d26b03'::uuid,
    '3cc6ae1e-431f-4f98-8e31-58b16be2db04'::uuid
);
""");

            migrationBuilder.Sql("""
DELETE FROM authentication.permissions
WHERE id IN (
    'c12b4a8d-5d9f-4b85-a3ce-4d5ed9f5b101'::uuid,
    'f4e58f59-e8da-47df-b733-03d5ed0d4102'::uuid,
    'bc6e3cd3-cf0d-487c-a883-b82701d26b03'::uuid,
    '3cc6ae1e-431f-4f98-8e31-58b16be2db04'::uuid
);
""");
        }
    }
}
