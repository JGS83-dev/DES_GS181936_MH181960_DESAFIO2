## Desafio 2 DES 104 GS181936 - MH181960
Una empresa de tecnología está desarrollando un sistema de gestión de usuarios, roles y permisos.
Se requiere una aplicación que realice operaciones CRUD (Crear, Leer, Actualizar, Eliminar) para estas entidades.
La base de datos debe contener las siguientes tablas:

Usuarios
• ID único de usuario.
• Nombre: Campo obligatorio, longitud mínima de 3 caracteres y máxima de 50.
• Email: Campo obligatorio, debe seguir un formato válido.
• Contraseña: Campo obligatorio, debe tener al menos 8 caracteres.
• Rol asociado.

Roles
• ID único de rol.
• Nombre del rol: Campo obligatorio, longitud mínima de 3 caracteres y máxima de 30.
• Descripción: Opcional.

Permisos
• ID único de permiso.
• Nombre del permiso: Campo obligatorio, longitud mínima de 3 caracteres y máxima de 50.
• Descripción: Opcional.

REQUERIMIENTOS
1. Aplicación API
a. Creación de una API usando .NET Core 8.0 que incluya operaciones CRUD para las tablas
mencionadas (Usuarios, Roles y Permisos).
b. Implementar Redis para almacenar en caché las consultas de lectura de usuarios, roles y
permisos.
c. Implementar pruebas unitarias que validen las operaciones CRUD de las tablas
mencionadas.
d. Creación de migraciones a la base de datos usando Entity Framework o Dapper.
2. API Gateway usando Ocelot
a. Crear un API Gateway que encapsule todos los endpoints de la API mencionada.
b. Configurar el API Gateway para enrutar todas las solicitudes a sus respectivos servicios.
c. Habilitar soporte para Swagger en el API Gateway y la API.
3. Docker
a. Crear un archivo Dockerfile para la API y otro para el API Gateway.
b. Crear un archivo docker-compose.yml que orqueste ambos contenedores (API y API
Gateway), además del contenedor de Redis y SQL Server.
c. Validar que todo el sistema funcione correctamente al ejecutarse en contenedores Docker.
