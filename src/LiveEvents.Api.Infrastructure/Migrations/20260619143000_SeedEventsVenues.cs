using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LiveEvents.Api.Infrastructure.DbContexts;

#nullable disable

namespace LiveEvents.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(LiveEventsDbContext))]
    [Migration("20260619143000_SeedEventsVenues")]
    public partial class SeedEventsVenues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
INSERT INTO events.venues (id, name, capacity, city, status, created_at, updated_at, deleted_at)
VALUES
    ('a0a5cb48-d2f0-488c-aafb-41e1a6cbc67e'::uuid, $seed$Arena Sur$seed$, 500, $seed$Medellín$seed$, TRUE, TIMESTAMP '2026-06-18 13:27:29.035933', NULL, NULL),
    ('2346dbb8-33b2-4c2d-8e2a-c90fa36a4ee6'::uuid, $seed$Auditorio Central$seed$, 200, $seed$Bogotá$seed$, TRUE, TIMESTAMP '2026-06-18 13:26:30.812985', NULL, NULL),
    ('755a04ef-07c5-4f84-acf6-a86fa551b1d4'::uuid, $seed$Sala Norte$seed$, 50, $seed$Bogotá$seed$, TRUE, TIMESTAMP '2026-06-18 13:27:29.035933', NULL, NULL)
ON CONFLICT (id) DO NOTHING;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
DELETE FROM events.venues
WHERE id IN (
    'a0a5cb48-d2f0-488c-aafb-41e1a6cbc67e'::uuid,
    '2346dbb8-33b2-4c2d-8e2a-c90fa36a4ee6'::uuid,
    '755a04ef-07c5-4f84-acf6-a86fa551b1d4'::uuid
);
""");
        }
    }
}
