using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveEvents.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAuthenticationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
INSERT INTO authentication.roles (id, name, description, status, created_at, updated_at)
VALUES
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, $seed$Administrador$seed$, $seed$Gestión y configuración completa del sistema.$seed$, TRUE, '2026-05-01T18:13:16.905553'::timestamp, NULL),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, $seed$Usuario$seed$, $seed$Acceso y gestión de la información del restaurante$seed$, TRUE, '2026-05-01T18:13:16.905553'::timestamp, NULL)
ON CONFLICT (id) DO NOTHING;
""");

            migrationBuilder.Sql("""
INSERT INTO authentication.permissions (id, name, description, status, created_at, updated_at)
VALUES
        ('b023931d-2d93-4d86-a82d-54c587edc08d'::uuid, $seed$$seed$, NULL, TRUE, '2026-06-04T15:18:37.99773'::timestamp, NULL),
        ('6e2281c1-e626-4daf-b5ec-7da12deaa6a5'::uuid, $seed$analytics.dashboard$seed$, $seed$Acceder al dashboard de analytics$seed$, TRUE, '2026-05-01T18:13:16.905545'::timestamp, NULL),
        ('76aeb57f-cdf2-40db-b02e-cc175e7253c0'::uuid, $seed$clients.create$seed$, $seed$Crear clientes$seed$, TRUE, '2026-05-01T18:13:16.905545'::timestamp, NULL),
        ('16156551-e75f-441a-b279-97c5419fbc1e'::uuid, $seed$clients.delete$seed$, $seed$Eliminar clientes$seed$, TRUE, '2026-05-01T18:13:16.905538'::timestamp, NULL),
        ('4f134eb3-2dc0-4806-8519-41cb7727efc6'::uuid, $seed$clients.read$seed$, $seed$Consultar información de clientes$seed$, TRUE, '2026-05-01T18:13:16.905543'::timestamp, NULL),
        ('8e395540-fde7-4f2c-a360-62a9acb08c39'::uuid, $seed$clients.update$seed$, $seed$Actualizar información de clientes$seed$, TRUE, '2026-05-01T18:13:16.905547'::timestamp, NULL),
        ('a5555555-5555-5555-5555-555555555555'::uuid, $seed$dishes.create$seed$, $seed$Crear platos del menú$seed$, TRUE, '2026-05-01T13:13:16.965547'::timestamp, NULL),
        ('a7777777-7777-7777-7777-777777777777'::uuid, $seed$dishes.delete$seed$, $seed$Eliminar platos del menú$seed$, TRUE, '2026-05-01T13:13:16.965547'::timestamp, NULL),
        ('a4444444-4444-4444-4444-444444444444'::uuid, $seed$dishes.read$seed$, $seed$Consultar platos del menú$seed$, TRUE, '2026-05-01T13:13:16.965547'::timestamp, NULL),
        ('a6666666-6666-6666-6666-666666666666'::uuid, $seed$dishes.update$seed$, $seed$Actualizar platos del menú$seed$, TRUE, '2026-05-01T13:13:16.965547'::timestamp, NULL),
        ('f0b694ec-335c-4f3d-b4f3-58e33e423737'::uuid, $seed$menu.create$seed$, $seed$Permiso para crear menu$seed$, TRUE, '2026-05-01T18:13:16.905552'::timestamp, NULL),
        ('0c90e6be-52f5-4378-b6d5-a3c5b1b5cdb6'::uuid, $seed$menu.delete$seed$, $seed$Permiso para eliminar menu$seed$, TRUE, '2026-05-01T18:13:16.905538'::timestamp, NULL),
        ('cc0d0fde-68d4-44c0-8c00-e7ee24501413'::uuid, $seed$menu.read$seed$, $seed$Permiso para leer menu$seed$, TRUE, '2026-05-01T18:13:16.905551'::timestamp, NULL),
        ('c5f88de6-304a-44c2-a28d-178d33d49152'::uuid, $seed$menu.update$seed$, $seed$Permiso para actualizar menu$seed$, TRUE, '2026-05-01T18:13:16.90555'::timestamp, NULL),
        ('a2222222-2222-2222-2222-222222222222'::uuid, $seed$orders.create$seed$, $seed$Crear pedidos$seed$, TRUE, '2026-05-01T13:13:16.965547'::timestamp, NULL),
        ('a1111111-1111-1111-1111-111111111111'::uuid, $seed$orders.read$seed$, $seed$Consultar pedidos$seed$, TRUE, '2026-05-01T13:13:16.965547'::timestamp, NULL),
        ('a3333333-3333-3333-3333-333333333333'::uuid, $seed$orders.update$seed$, $seed$Actualizar pedidos$seed$, TRUE, '2026-05-01T13:13:16.965547'::timestamp, NULL),
        ('4e2f78f0-6d4c-4e65-9029-efa4dbb519b1'::uuid, $seed$permissions.create$seed$, $seed$Crear permisos$seed$, TRUE, '2026-05-01T18:13:16.905543'::timestamp, NULL),
        ('5412ed73-19a3-487a-bcfd-beae36c5a27e'::uuid, $seed$permissions.delete$seed$, $seed$Eliminar permisos$seed$, TRUE, '2026-05-01T18:13:16.905543'::timestamp, NULL),
        ('0f4de161-4aa0-4e78-bbe5-7d083cddd604'::uuid, $seed$permissions.read$seed$, $seed$Consultar permisos$seed$, TRUE, '2026-05-01T18:13:16.905538'::timestamp, NULL),
        ('962f2a3c-5891-4836-b556-dbe287d17876'::uuid, $seed$permissions.update$seed$, $seed$Actualizar permisos$seed$, TRUE, '2026-05-01T18:13:16.905547'::timestamp, NULL),
        ('6d1c2462-fa3e-4a68-86e8-b6896cdaccd6'::uuid, $seed$projects.create$seed$, $seed$Permiso para crear proyectos$seed$, TRUE, '2026-05-01T18:13:16.905544'::timestamp, NULL),
        ('a01ec000-518f-4fec-a8b5-30af492eedc9'::uuid, $seed$projects.delete$seed$, $seed$Permiso para eliminar proyectos$seed$, TRUE, '2026-05-01T18:13:16.905547'::timestamp, NULL),
        ('2660d239-eafc-490b-8cb0-03e39120a244'::uuid, $seed$projects.edit$seed$, $seed$Permiso para editar proyectos$seed$, TRUE, '2026-05-01T18:13:16.90554'::timestamp, NULL),
        ('286989b0-fe3e-42dc-862e-8cfb9a6c47a8'::uuid, $seed$projects.read$seed$, $seed$Permiso para leer proyectos$seed$, TRUE, '2026-05-01T18:13:16.905542'::timestamp, NULL),
        ('c9fe2949-b248-4c13-9e3e-aba40a21b1d9'::uuid, $seed$reports.financial$seed$, $seed$Generar reportes financieros$seed$, TRUE, '2026-05-01T18:13:16.90555'::timestamp, NULL),
        ('a04e82f0-1cb1-4fd3-a3a3-8d945ab90724'::uuid, $seed$reports.inventory$seed$, $seed$Generar reportes de inventario$seed$, TRUE, '2026-05-01T18:13:16.905547'::timestamp, NULL),
        ('80c6834a-7ddf-4232-987d-638196cb3972'::uuid, $seed$reports.purchases$seed$, $seed$Generar reportes de compras$seed$, TRUE, '2026-05-01T18:13:16.905546'::timestamp, NULL),
        ('2145dbdc-3e3b-4074-b3d6-c1c64e4f8124'::uuid, $seed$reports.sales$seed$, $seed$Generar reportes de ventas$seed$, TRUE, '2026-05-01T18:13:16.905538'::timestamp, NULL),
        ('5c2cb257-2d18-4870-9675-da43b772a5cb'::uuid, $seed$reports.users$seed$, $seed$Generar reportes de usuarios$seed$, TRUE, '2026-05-01T18:13:16.905543'::timestamp, NULL),
        ('501cadb6-4f7c-43b8-b2bb-3463ee2d5440'::uuid, $seed$reviews.create$seed$, $seed$Permiso para crear reseña$seed$, TRUE, '2026-05-01T18:13:16.905543'::timestamp, NULL),
        ('2c85aa39-60b0-41e8-9d60-1e12c37918fd'::uuid, $seed$reviews.delete$seed$, $seed$Permiso para borrar reseña$seed$, TRUE, '2026-05-01T18:13:16.905542'::timestamp, NULL),
        ('d175300f-22c8-487e-95eb-54f32fc1b5b5'::uuid, $seed$reviews.edit$seed$, $seed$Permiso para editar reseñas$seed$, TRUE, '2026-05-01T18:13:16.905551'::timestamp, NULL),
        ('c236764a-03a2-428a-b1e2-3819dbd2bfa6'::uuid, $seed$reviews.read$seed$, $seed$Permiso para leer reseñas$seed$, TRUE, '2026-05-01T18:13:16.90555'::timestamp, NULL),
        ('8ac8e2be-83ae-4bbf-a599-91dab83cd1f7'::uuid, $seed$roles.assign_permissions$seed$, $seed$Asignar permisos a roles$seed$, TRUE, '2026-05-01T18:13:16.905546'::timestamp, NULL),
        ('a6e82580-5cef-48aa-acad-edcaa825ade6'::uuid, $seed$roles.create$seed$, $seed$Crear roles$seed$, TRUE, '2026-05-01T18:13:16.905548'::timestamp, NULL),
        ('af015994-520b-4e03-abb0-cf0cf9975675'::uuid, $seed$roles.delete$seed$, $seed$Eliminar roles$seed$, TRUE, '2026-05-01T18:13:16.905549'::timestamp, NULL),
        ('a15c1b24-b3ac-44d4-87c7-b645b54bd32c'::uuid, $seed$roles.manage$seed$, $seed$Permiso para asignar multiples permisos a un rol$seed$, TRUE, '2026-05-01T18:13:16.905548'::timestamp, NULL),
        ('f12b05f9-c256-4dcd-bcfa-1b791e4cdc82'::uuid, $seed$roles.read$seed$, $seed$Consultar roles$seed$, TRUE, '2026-05-01T18:13:16.905552'::timestamp, NULL),
        ('61731b86-0223-44f5-80be-eb8e9dd8327e'::uuid, $seed$roles.update$seed$, $seed$Actualizar roles$seed$, TRUE, '2026-05-01T18:13:16.905544'::timestamp, NULL),
        ('f5054155-11c5-4077-ac9a-4d02733e4ffe'::uuid, $seed$schedules.create$seed$, $seed$Permiso para crear reuniones$seed$, TRUE, '2026-05-01T18:13:16.905552'::timestamp, NULL),
        ('4d1e7d33-3470-4d6d-8d6e-e8227b73a3fd'::uuid, $seed$schedules.delete$seed$, $seed$Permiso para eliminar agendas$seed$, TRUE, '2026-05-01T18:13:16.905542'::timestamp, NULL),
        ('bba7db9f-4562-4849-b620-7a47f100f4cb'::uuid, $seed$schedules.edit$seed$, $seed$Permiso para editar agendas$seed$, TRUE, '2026-05-01T18:13:16.905549'::timestamp, NULL),
        ('355c217f-3ab3-4f82-a753-8a339610a27d'::uuid, $seed$schedules.read$seed$, $seed$Permiso para leer agendas$seed$, TRUE, '2026-05-01T18:13:16.905542'::timestamp, NULL),
        ('dc82f99d-6f48-4fe7-b3f4-49260b8ac9d8'::uuid, $seed$services.create$seed$, $seed$Permiso para crear servicios$seed$, TRUE, '2026-05-01T18:13:16.905551'::timestamp, NULL),
        ('c8354179-dc42-45b7-a1b8-34b7b1073f87'::uuid, $seed$services.delete$seed$, $seed$Permiso para eliminar servicios$seed$, TRUE, '2026-05-01T18:13:16.90555'::timestamp, NULL),
        ('ab8b6e77-fc91-49a2-9800-a859a1e6c648'::uuid, $seed$services.edit$seed$, $seed$Permiso para editar servicios$seed$, TRUE, '2026-05-01T18:13:16.905549'::timestamp, NULL),
        ('b2a3f726-dcf7-4073-ba61-ecd5dd09dd92'::uuid, $seed$services.read$seed$, $seed$Permiso para leer servicios$seed$, TRUE, '2026-05-01T18:13:16.905549'::timestamp, NULL),
        ('00e2ee71-5816-4ce6-aefd-cda8b1f10d0f'::uuid, $seed$suppliers.create$seed$, $seed$Crear proveedores$seed$, TRUE, '2026-05-01T18:13:16.905537'::timestamp, NULL),
        ('73cb2962-aef2-42a4-8407-cf0869625ede'::uuid, $seed$suppliers.delete$seed$, $seed$Eliminar proveedores$seed$, TRUE, '2026-05-01T18:13:16.905545'::timestamp, NULL),
        ('24ef1acc-9a55-4b86-9868-a7fe2d01ca6f'::uuid, $seed$suppliers.read$seed$, $seed$Consultar proveedores$seed$, TRUE, '2026-05-01T18:13:16.905538'::timestamp, NULL),
        ('4792f730-b1ca-4af2-a281-8a97250ffefb'::uuid, $seed$suppliers.update$seed$, $seed$Actualizar proveedores$seed$, TRUE, '2026-05-01T18:13:16.905542'::timestamp, NULL),
        ('b47fac30-82cb-46da-85b8-b1046ae6916d'::uuid, $seed$suppliers.view_by_name$seed$, $seed$Buscar proveedores por nombre$seed$, TRUE, '2026-05-01T18:13:16.905549'::timestamp, NULL),
        ('b3a75e58-e3c2-4519-9cae-ac0653ac0eb3'::uuid, $seed$user_types.create$seed$, $seed$Crear tipos de usuario$seed$, TRUE, '2026-05-01T18:13:16.905549'::timestamp, NULL),
        ('bc6c8488-06c8-4246-8253-a80ac8b5625d'::uuid, $seed$user_types.delete$seed$, $seed$Eliminar tipos de usuario$seed$, TRUE, '2026-05-01T18:13:16.905549'::timestamp, NULL),
        ('5449c302-93c8-4670-ba28-0567dba694ba'::uuid, $seed$user_types.read$seed$, $seed$Consultar tipos de usuario$seed$, TRUE, '2026-05-01T18:13:16.905543'::timestamp, NULL),
        ('030640f8-5b1c-4a44-ac0a-7702a4e63008'::uuid, $seed$user_types.update$seed$, $seed$Actualizar tipos de usuario$seed$, TRUE, '2026-05-01T18:13:16.905537'::timestamp, NULL),
        ('94737ae8-877a-4d01-99ea-8b9f69154f56'::uuid, $seed$users.change_password$seed$, $seed$Cambiar contraseñas de usuarios$seed$, TRUE, '2026-05-01T18:13:16.905547'::timestamp, NULL),
        ('f7c3c0a4-2b6d-4e92-9a0d-6e1d948a43f8'::uuid, $seed$users.create$seed$, $seed$Crear nuevos usuarios$seed$, TRUE, '2026-05-01T18:13:16.905552'::timestamp, NULL),
        ('81addf88-c92f-4d4a-90e2-1969e80a4551'::uuid, $seed$users.delete$seed$, $seed$Eliminar usuarios$seed$, TRUE, '2026-05-01T18:13:16.905546'::timestamp, NULL),
        ('282da9b9-ba97-4d38-a267-2339daeb3957'::uuid, $seed$users.manage_additional_data$seed$, $seed$Gestionar datos adicionales de usuarios$seed$, TRUE, '2026-05-01T18:13:16.90554'::timestamp, NULL),
        ('d8e9507a-5b12-4d2a-8c98-1a52b1466a93'::uuid, $seed$users.read$seed$, $seed$Consultar información de usuarios$seed$, TRUE, '2026-05-01T18:13:16.905551'::timestamp, NULL),
        ('e1d93604-6a4b-4468-ae77-00ea465042a0'::uuid, $seed$users.update$seed$, $seed$Actualizar información de usuarios$seed$, TRUE, '2026-05-01T18:13:16.905552'::timestamp, NULL)
ON CONFLICT (id) DO NOTHING;
""");

            migrationBuilder.Sql("""
INSERT INTO authentication.role_permissions (role_id, permission_id)
VALUES
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '00e2ee71-5816-4ce6-aefd-cda8b1f10d0f'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '030640f8-5b1c-4a44-ac0a-7702a4e63008'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '0c90e6be-52f5-4378-b6d5-a3c5b1b5cdb6'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '0f4de161-4aa0-4e78-bbe5-7d083cddd604'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '16156551-e75f-441a-b279-97c5419fbc1e'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '2145dbdc-3e3b-4074-b3d6-c1c64e4f8124'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '24ef1acc-9a55-4b86-9868-a7fe2d01ca6f'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '2660d239-eafc-490b-8cb0-03e39120a244'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '282da9b9-ba97-4d38-a267-2339daeb3957'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '286989b0-fe3e-42dc-862e-8cfb9a6c47a8'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '2c85aa39-60b0-41e8-9d60-1e12c37918fd'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '355c217f-3ab3-4f82-a753-8a339610a27d'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '4792f730-b1ca-4af2-a281-8a97250ffefb'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '4d1e7d33-3470-4d6d-8d6e-e8227b73a3fd'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '4e2f78f0-6d4c-4e65-9029-efa4dbb519b1'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '4f134eb3-2dc0-4806-8519-41cb7727efc6'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '501cadb6-4f7c-43b8-b2bb-3463ee2d5440'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '5412ed73-19a3-487a-bcfd-beae36c5a27e'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '5449c302-93c8-4670-ba28-0567dba694ba'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '5c2cb257-2d18-4870-9675-da43b772a5cb'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '61731b86-0223-44f5-80be-eb8e9dd8327e'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '6d1c2462-fa3e-4a68-86e8-b6896cdaccd6'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '6e2281c1-e626-4daf-b5ec-7da12deaa6a5'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '73cb2962-aef2-42a4-8407-cf0869625ede'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '76aeb57f-cdf2-40db-b02e-cc175e7253c0'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '80c6834a-7ddf-4232-987d-638196cb3972'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '81addf88-c92f-4d4a-90e2-1969e80a4551'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '8ac8e2be-83ae-4bbf-a599-91dab83cd1f7'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '8e395540-fde7-4f2c-a360-62a9acb08c39'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '94737ae8-877a-4d01-99ea-8b9f69154f56'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '962f2a3c-5891-4836-b556-dbe287d17876'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a01ec000-518f-4fec-a8b5-30af492eedc9'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a04e82f0-1cb1-4fd3-a3a3-8d945ab90724'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a1111111-1111-1111-1111-111111111111'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a15c1b24-b3ac-44d4-87c7-b645b54bd32c'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a2222222-2222-2222-2222-222222222222'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a3333333-3333-3333-3333-333333333333'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a4444444-4444-4444-4444-444444444444'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a5555555-5555-5555-5555-555555555555'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a6666666-6666-6666-6666-666666666666'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a6e82580-5cef-48aa-acad-edcaa825ade6'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a7777777-7777-7777-7777-777777777777'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'ab8b6e77-fc91-49a2-9800-a859a1e6c648'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'af015994-520b-4e03-abb0-cf0cf9975675'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'b2a3f726-dcf7-4073-ba61-ecd5dd09dd92'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'b3a75e58-e3c2-4519-9cae-ac0653ac0eb3'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'b47fac30-82cb-46da-85b8-b1046ae6916d'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'bba7db9f-4562-4849-b620-7a47f100f4cb'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'bc6c8488-06c8-4246-8253-a80ac8b5625d'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'c236764a-03a2-428a-b1e2-3819dbd2bfa6'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'c5f88de6-304a-44c2-a28d-178d33d49152'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'c8354179-dc42-45b7-a1b8-34b7b1073f87'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'c9fe2949-b248-4c13-9e3e-aba40a21b1d9'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'cc0d0fde-68d4-44c0-8c00-e7ee24501413'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'd175300f-22c8-487e-95eb-54f32fc1b5b5'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'd8e9507a-5b12-4d2a-8c98-1a52b1466a93'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'dc82f99d-6f48-4fe7-b3f4-49260b8ac9d8'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'e1d93604-6a4b-4468-ae77-00ea465042a0'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'f0b694ec-335c-4f3d-b4f3-58e33e423737'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'f12b05f9-c256-4dcd-bcfa-1b791e4cdc82'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'f5054155-11c5-4077-ac9a-4d02733e4ffe'::uuid),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'f7c3c0a4-2b6d-4e92-9a0d-6e1d948a43f8'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, '2145dbdc-3e3b-4074-b3d6-c1c64e4f8124'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a1111111-1111-1111-1111-111111111111'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a2222222-2222-2222-2222-222222222222'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a3333333-3333-3333-3333-333333333333'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a4444444-4444-4444-4444-444444444444'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a5555555-5555-5555-5555-555555555555'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a6666666-6666-6666-6666-666666666666'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a7777777-7777-7777-7777-777777777777'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'c236764a-03a2-428a-b1e2-3819dbd2bfa6'::uuid),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'cc0d0fde-68d4-44c0-8c00-e7ee24501413'::uuid)
ON CONFLICT (role_id, permission_id) DO NOTHING;
""");

            migrationBuilder.Sql("""
INSERT INTO authentication.user_types (id, name, description, status, theme, default_landing_page, logo_url, language, additional_config, created_at, updated_at)
VALUES
        ('82d97e80-eba4-45ea-9f41-0d85e0a7413d'::uuid, $seed$Administrador$seed$, $seed$Usuarios con control total sobre la aplicación.$seed$, TRUE, NULL, NULL, NULL, NULL, $seed$
                        {
                          "navigation": [
                            {
                              "menuId": "ac8a0b8c-7125-4a1c-9be5-0a875f840451",
                              "label": "Dashboard",
                              "icon": "fas fa-tachometer-alt",
                              "route": "/dashboard",
                              "parentId": "",
                              "permissions": "read,create,edit,delete",
                              "children": []
                            },
                            {
                              "menuId": "ddf6af91-aac5-43c2-923c-4ec0b5b288d6",
                              "label": "Autenticación",
                              "icon": "shield",
                              "route": "/Auth",
                              "permissions": "read,create,edit,delete",
                              "children": [
                                {
                                  "menuId": "88dab63f-1cb1-4908-a067-b3c7fa8bb614",
                                  "label": "Usuarios",
                                  "icon": "people",
                                  "route": "/auth/users",
                                  "parentId": "ddf6af91-aac5-43c2-923c-4ec0b5b288d6",
                                  "permissions": "read,create,edit,delete"
                                },
                                {
                                  "menuId": "c1ad6944-862c-4feb-8cab-f7ad411a9623",
                                  "label": "Roles",
                                  "icon": "AssignmentInd",
                                  "route": "/auth/roles",
                                  "parentId": "ddf6af91-aac5-43c2-923c-4ec0b5b288d6",
                                  "permissions": "read,create,edit,delete"
                                },
                                {
                                  "menuId": "0a15d5a2-626a-4209-92e9-34f63cef4a0f",
                                  "label": "Permisos",
                                  "icon": "VerifiedUser",
                                  "route": "/auth/permissions",
                                  "parentId": "ddf6af91-aac5-43c2-923c-4ec0b5b288d6",
                                  "permissions": "read,create,edit,delete"
                                },
                                {
                                  "menuId": "a7d1eb00-712c-4e63-8c83-83a0c71cfce9",
                                  "label": "Tipos de usuarios",
                                  "icon": "GroupAdd",
                                  "route": "/auth/user-types",
                                  "parentId": "ddf6af91-aac5-43c2-923c-4ec0b5b288d6",
                                  "permissions": "read,create,edit,delete"
                                }
                              ]
                            }
                          ]
                        }$seed$, '2026-05-01T18:13:16.905302'::timestamp, NULL),
        ('d13d0d11-acc7-4c8f-8cb9-e6b2706cc89a'::uuid, $seed$Usuario$seed$, $seed$Usuario del sistema$seed$, TRUE, NULL, NULL, NULL, NULL, $seed$
                        {
                          "navigation": [
                            {
                              "menuId": "ac8a0b8c-7125-4a1c-9be5-0a875f840451",
                              "label": "Dashboard",
                              "icon": "fas fa-tachometer-alt",
                              "route": "/dashboard",
                              "parentId": "",
                              "permissions": "read,create,edit,delete",
                              "children": []
                            },
                            {
                              "menuId": "2b1ca67f-fb93-4e29-b613-96044636133f",
                              "label": "Restaurante",
                              "icon": "Restaurant",
                              "route": "/restaurant",
                              "parentId": "",
                              "permissions": "read,create,edit,delete",
                              "children": [
                                {
                                  "menuId": "2e348424-8184-4e7b-aaff-c88b44f0df13",
                                  "label": "Menu",
                                  "icon": "MenuBook",
                                  "route": "/restaurant/dishes",
                                  "parentId": "2b1ca67f-fb93-4e29-b613-96044636133f",
                                  "permissions": "read,create,edit,delete",
                                  "children": []
                                },
                                {
                                  "menuId": "95dab6c5-b459-449d-9d88-cb7dbee78f04",
                                  "label": "Pedidos",
                                  "icon": "DinnerDining",
                                  "route": "/restaurant/orders",
                                  "parentId": "2b1ca67f-fb93-4e29-b613-96044636133f",
                                  "permissions": "read,create,edit,delete",
                                  "children": []
                                }
                              ]
                            },
                            {
                              "menuId": "d036385a-ac9f-4e02-a7d1-a6f6c7148bce",
                              "label": "Informes y Analíticas",
                              "icon": "Analytics",
                              "route": "/report",
                              "parentId": "",
                              "permissions": "read,create,edit,delete",
                              "children": [
                                {
                                  "menuId": "ac8a0b8c-7125-4a1c-9be5-0a875f840444",
                                  "label": "Reportes",
                                  "icon": "BarChart",
                                  "route": "/restaurant/reports",
                                  "parentId": "d036385a-ac9f-4e02-a7d1-a6f6c7148bce",
                                  "permissions": "read,create,edit,delete",
                                  "children": []
                                }
                              ]
                            }
                          ]
                        }$seed$, '2026-05-01T18:13:16.905303'::timestamp, NULL)
ON CONFLICT (id) DO NOTHING;
""");

            migrationBuilder.Sql("""
INSERT INTO authentication.users (id, name, address, email, password, image, phone, user_type_id, extra_data, status, created_at, updated_at, deleted_at)
VALUES
        ('57a98f97-9271-4aff-83d4-09332cfacb31'::uuid, $seed$Administrador$seed$, NULL, $seed$admin@gmail.com$seed$, $seed$$2a$12$yYX4KKLz5QIUB0Ee/mNG3eK6iwdfg/Vn3OWx7n7EQmO1hLlLIOhE2$seed$, NULL, NULL, '82d97e80-eba4-45ea-9f41-0d85e0a7413d'::uuid, $seed${"securityStamp":"7a63217cdcfb493db97ab97ebcf3764e"}$seed$, 'Activo'::authentication.user_status, '2026-05-01T18:13:16.905554'::timestamp, NULL, NULL),
        ('383d5956-1326-4263-b6a6-6d8bf9be4728'::uuid, $seed$Usuario$seed$, NULL, $seed$usuario@gmail.com$seed$, $seed$$2a$12$yYX4KKLz5QIUB0Ee/mNG3eK6iwdfg/Vn3OWx7n7EQmO1hLlLIOhE2$seed$, NULL, NULL, 'd13d0d11-acc7-4c8f-8cb9-e6b2706cc89a'::uuid, $seed${}$seed$, 'Activo'::authentication.user_status, '2026-05-01T18:13:16.905555'::timestamp, NULL, NULL)
ON CONFLICT (id) DO NOTHING;
""");

            migrationBuilder.Sql("""
INSERT INTO authentication.user_roles (user_id, role_id)
VALUES
        ('383d5956-1326-4263-b6a6-6d8bf9be4728'::uuid, 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid),
        ('57a98f97-9271-4aff-83d4-09332cfacb31'::uuid, 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid)
ON CONFLICT (user_id, role_id) DO NOTHING;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
DELETE FROM authentication.user_roles WHERE (user_id, role_id) IN (('383d5956-1326-4263-b6a6-6d8bf9be4728'::uuid, 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid), ('57a98f97-9271-4aff-83d4-09332cfacb31'::uuid, 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid));
""");

            migrationBuilder.Sql("""
DELETE FROM authentication.users WHERE id IN ('57a98f97-9271-4aff-83d4-09332cfacb31'::uuid, '383d5956-1326-4263-b6a6-6d8bf9be4728'::uuid);
""");

            migrationBuilder.Sql("""
DELETE FROM authentication.role_permissions WHERE (role_id, permission_id) IN (('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '00e2ee71-5816-4ce6-aefd-cda8b1f10d0f'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '030640f8-5b1c-4a44-ac0a-7702a4e63008'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '0c90e6be-52f5-4378-b6d5-a3c5b1b5cdb6'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '0f4de161-4aa0-4e78-bbe5-7d083cddd604'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '16156551-e75f-441a-b279-97c5419fbc1e'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '2145dbdc-3e3b-4074-b3d6-c1c64e4f8124'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '24ef1acc-9a55-4b86-9868-a7fe2d01ca6f'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '2660d239-eafc-490b-8cb0-03e39120a244'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '282da9b9-ba97-4d38-a267-2339daeb3957'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '286989b0-fe3e-42dc-862e-8cfb9a6c47a8'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '2c85aa39-60b0-41e8-9d60-1e12c37918fd'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '355c217f-3ab3-4f82-a753-8a339610a27d'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '4792f730-b1ca-4af2-a281-8a97250ffefb'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '4d1e7d33-3470-4d6d-8d6e-e8227b73a3fd'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '4e2f78f0-6d4c-4e65-9029-efa4dbb519b1'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '4f134eb3-2dc0-4806-8519-41cb7727efc6'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '501cadb6-4f7c-43b8-b2bb-3463ee2d5440'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '5412ed73-19a3-487a-bcfd-beae36c5a27e'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '5449c302-93c8-4670-ba28-0567dba694ba'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '5c2cb257-2d18-4870-9675-da43b772a5cb'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '61731b86-0223-44f5-80be-eb8e9dd8327e'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '6d1c2462-fa3e-4a68-86e8-b6896cdaccd6'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '6e2281c1-e626-4daf-b5ec-7da12deaa6a5'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '73cb2962-aef2-42a4-8407-cf0869625ede'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '76aeb57f-cdf2-40db-b02e-cc175e7253c0'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '80c6834a-7ddf-4232-987d-638196cb3972'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '81addf88-c92f-4d4a-90e2-1969e80a4551'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '8ac8e2be-83ae-4bbf-a599-91dab83cd1f7'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '8e395540-fde7-4f2c-a360-62a9acb08c39'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '94737ae8-877a-4d01-99ea-8b9f69154f56'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, '962f2a3c-5891-4836-b556-dbe287d17876'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a01ec000-518f-4fec-a8b5-30af492eedc9'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a04e82f0-1cb1-4fd3-a3a3-8d945ab90724'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a1111111-1111-1111-1111-111111111111'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a15c1b24-b3ac-44d4-87c7-b645b54bd32c'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a2222222-2222-2222-2222-222222222222'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a3333333-3333-3333-3333-333333333333'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a4444444-4444-4444-4444-444444444444'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a5555555-5555-5555-5555-555555555555'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a6666666-6666-6666-6666-666666666666'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a6e82580-5cef-48aa-acad-edcaa825ade6'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'a7777777-7777-7777-7777-777777777777'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'ab8b6e77-fc91-49a2-9800-a859a1e6c648'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'af015994-520b-4e03-abb0-cf0cf9975675'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'b2a3f726-dcf7-4073-ba61-ecd5dd09dd92'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'b3a75e58-e3c2-4519-9cae-ac0653ac0eb3'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'b47fac30-82cb-46da-85b8-b1046ae6916d'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'bba7db9f-4562-4849-b620-7a47f100f4cb'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'bc6c8488-06c8-4246-8253-a80ac8b5625d'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'c236764a-03a2-428a-b1e2-3819dbd2bfa6'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'c5f88de6-304a-44c2-a28d-178d33d49152'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'c8354179-dc42-45b7-a1b8-34b7b1073f87'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'c9fe2949-b248-4c13-9e3e-aba40a21b1d9'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'cc0d0fde-68d4-44c0-8c00-e7ee24501413'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'd175300f-22c8-487e-95eb-54f32fc1b5b5'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'd8e9507a-5b12-4d2a-8c98-1a52b1466a93'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'dc82f99d-6f48-4fe7-b3f4-49260b8ac9d8'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'e1d93604-6a4b-4468-ae77-00ea465042a0'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'f0b694ec-335c-4f3d-b4f3-58e33e423737'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'f12b05f9-c256-4dcd-bcfa-1b791e4cdc82'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'f5054155-11c5-4077-ac9a-4d02733e4ffe'::uuid), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'f7c3c0a4-2b6d-4e92-9a0d-6e1d948a43f8'::uuid), ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, '2145dbdc-3e3b-4074-b3d6-c1c64e4f8124'::uuid), ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a1111111-1111-1111-1111-111111111111'::uuid), ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a2222222-2222-2222-2222-222222222222'::uuid), ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a3333333-3333-3333-3333-333333333333'::uuid), ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a4444444-4444-4444-4444-444444444444'::uuid), ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a5555555-5555-5555-5555-555555555555'::uuid), ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a6666666-6666-6666-6666-666666666666'::uuid), ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'a7777777-7777-7777-7777-777777777777'::uuid), ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'c236764a-03a2-428a-b1e2-3819dbd2bfa6'::uuid), ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid, 'cc0d0fde-68d4-44c0-8c00-e7ee24501413'::uuid));
""");

            migrationBuilder.Sql("""
DELETE FROM authentication.permissions WHERE id IN ('b023931d-2d93-4d86-a82d-54c587edc08d'::uuid, '6e2281c1-e626-4daf-b5ec-7da12deaa6a5'::uuid, '76aeb57f-cdf2-40db-b02e-cc175e7253c0'::uuid, '16156551-e75f-441a-b279-97c5419fbc1e'::uuid, '4f134eb3-2dc0-4806-8519-41cb7727efc6'::uuid, '8e395540-fde7-4f2c-a360-62a9acb08c39'::uuid, 'a5555555-5555-5555-5555-555555555555'::uuid, 'a7777777-7777-7777-7777-777777777777'::uuid, 'a4444444-4444-4444-4444-444444444444'::uuid, 'a6666666-6666-6666-6666-666666666666'::uuid, 'f0b694ec-335c-4f3d-b4f3-58e33e423737'::uuid, '0c90e6be-52f5-4378-b6d5-a3c5b1b5cdb6'::uuid, 'cc0d0fde-68d4-44c0-8c00-e7ee24501413'::uuid, 'c5f88de6-304a-44c2-a28d-178d33d49152'::uuid, 'a2222222-2222-2222-2222-222222222222'::uuid, 'a1111111-1111-1111-1111-111111111111'::uuid, 'a3333333-3333-3333-3333-333333333333'::uuid, '4e2f78f0-6d4c-4e65-9029-efa4dbb519b1'::uuid, '5412ed73-19a3-487a-bcfd-beae36c5a27e'::uuid, '0f4de161-4aa0-4e78-bbe5-7d083cddd604'::uuid, '962f2a3c-5891-4836-b556-dbe287d17876'::uuid, '6d1c2462-fa3e-4a68-86e8-b6896cdaccd6'::uuid, 'a01ec000-518f-4fec-a8b5-30af492eedc9'::uuid, '2660d239-eafc-490b-8cb0-03e39120a244'::uuid, '286989b0-fe3e-42dc-862e-8cfb9a6c47a8'::uuid, 'c9fe2949-b248-4c13-9e3e-aba40a21b1d9'::uuid, 'a04e82f0-1cb1-4fd3-a3a3-8d945ab90724'::uuid, '80c6834a-7ddf-4232-987d-638196cb3972'::uuid, '2145dbdc-3e3b-4074-b3d6-c1c64e4f8124'::uuid, '5c2cb257-2d18-4870-9675-da43b772a5cb'::uuid, '501cadb6-4f7c-43b8-b2bb-3463ee2d5440'::uuid, '2c85aa39-60b0-41e8-9d60-1e12c37918fd'::uuid, 'd175300f-22c8-487e-95eb-54f32fc1b5b5'::uuid, 'c236764a-03a2-428a-b1e2-3819dbd2bfa6'::uuid, '8ac8e2be-83ae-4bbf-a599-91dab83cd1f7'::uuid, 'a6e82580-5cef-48aa-acad-edcaa825ade6'::uuid, 'af015994-520b-4e03-abb0-cf0cf9975675'::uuid, 'a15c1b24-b3ac-44d4-87c7-b645b54bd32c'::uuid, 'f12b05f9-c256-4dcd-bcfa-1b791e4cdc82'::uuid, '61731b86-0223-44f5-80be-eb8e9dd8327e'::uuid, 'f5054155-11c5-4077-ac9a-4d02733e4ffe'::uuid, '4d1e7d33-3470-4d6d-8d6e-e8227b73a3fd'::uuid, 'bba7db9f-4562-4849-b620-7a47f100f4cb'::uuid, '355c217f-3ab3-4f82-a753-8a339610a27d'::uuid, 'dc82f99d-6f48-4fe7-b3f4-49260b8ac9d8'::uuid, 'c8354179-dc42-45b7-a1b8-34b7b1073f87'::uuid, 'ab8b6e77-fc91-49a2-9800-a859a1e6c648'::uuid, 'b2a3f726-dcf7-4073-ba61-ecd5dd09dd92'::uuid, '00e2ee71-5816-4ce6-aefd-cda8b1f10d0f'::uuid, '73cb2962-aef2-42a4-8407-cf0869625ede'::uuid, '24ef1acc-9a55-4b86-9868-a7fe2d01ca6f'::uuid, '4792f730-b1ca-4af2-a281-8a97250ffefb'::uuid, 'b47fac30-82cb-46da-85b8-b1046ae6916d'::uuid, 'b3a75e58-e3c2-4519-9cae-ac0653ac0eb3'::uuid, 'bc6c8488-06c8-4246-8253-a80ac8b5625d'::uuid, '5449c302-93c8-4670-ba28-0567dba694ba'::uuid, '030640f8-5b1c-4a44-ac0a-7702a4e63008'::uuid, '94737ae8-877a-4d01-99ea-8b9f69154f56'::uuid, 'f7c3c0a4-2b6d-4e92-9a0d-6e1d948a43f8'::uuid, '81addf88-c92f-4d4a-90e2-1969e80a4551'::uuid, '282da9b9-ba97-4d38-a267-2339daeb3957'::uuid, 'd8e9507a-5b12-4d2a-8c98-1a52b1466a93'::uuid, 'e1d93604-6a4b-4468-ae77-00ea465042a0'::uuid);
""");

            migrationBuilder.Sql("""
DELETE FROM authentication.roles WHERE id IN ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid, 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'::uuid);
""");

            migrationBuilder.Sql("""
DELETE FROM authentication.user_types WHERE id IN ('82d97e80-eba4-45ea-9f41-0d85e0a7413d'::uuid, 'd13d0d11-acc7-4c8f-8cb9-e6b2706cc89a'::uuid);
""");
        }
    }
}
