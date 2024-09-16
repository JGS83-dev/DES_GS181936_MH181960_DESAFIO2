using UsuarioAPI.Models;
using UsuarioAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace UsuarioAPI.Tests
{
    public class PermisosControllerTests
    {
        [Fact]
        public async Task PostPermiso_AgregaPermiso_CuandoPermisoEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new PermisosController(context, redis);

            // Act: Llamar al método del controlador
            var nuevoPermiso = new Permiso
            {
                Nombre = "Crear Permisos",
                Descripcion = null,
            };

            var result = await controller.PostPermiso(nuevoPermiso);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.IsType<Permiso>(createdResult.Value);
        }

        [Fact]
        public async Task PostPermiso_NoAgregaPermiso_CuandoNombreEsNulo()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new PermisosController(context, redis);

            // Act: Llamar al método del controlador
            var nuevoPermiso = new Permiso
            {
                Nombre = null,
                Descripcion = null,
            };

            var result = await controller.PostPermiso(nuevoPermiso);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostPermiso_NoAgregaPermiso_CuandoNombreContieneMenosDe3Caracteres()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new PermisosController(context, redis);

            // Act: Llamar al método del controlador
            var nuevoPermiso = new Permiso
            {
                Nombre = "No",
                Descripcion = null,
            };

            var result = await controller.PostPermiso(nuevoPermiso);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }        

        [Fact]
        public void GetPermiso_RetornaPermiso_CuandoIdEsValido()
        {
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new PermisosController(context, redis);

            // Act: Llamar al método del controlador
            var result = controller.GetPermiso(1);

            // Assert: Verificar que se obtiene el resultado correcto
            var actionResult = Assert.IsType<ActionResult<Permiso>>(result.Result);
            var returnValue = Assert.IsType<Permiso>(actionResult.Value);            
        }

        [Fact]
        public async Task GetPermiso_RetornaPermiso_CuandoIdNoEsValido()
        {
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new PermisosController(context, redis);

            // Act: Llamar al método del controlador
            var result = await controller.GetPermiso(999);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdatePermiso_ActualizaPermiso_CuandoPermisoEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new PermisosController(context, redis);

            var nuevoPermiso = new Permiso
            {
                Nombre = "Generar Informes",
                Descripcion = null,
            };

            var resultPost = await controller.PostPermiso(nuevoPermiso);
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultPost.Result);
            var permiso = Assert.IsType<Permiso>(createdResult.Value);

            // Act: Llamar al método del controlador
            permiso.Descripcion = "Generar Informes";

            var result = await controller.PutPermiso(permiso.Id, permiso);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePermiso_EliminaPermiso_CuandoIdNoEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new PermisosController(context, redis);

            // Act: Llamar al método del controlador           
            var result = await controller.DeletePermiso(999);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePermiso_EliminaPermiso_CuandoIdEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new PermisosController(context, redis);

            var nuevoPermiso = new Permiso
            {
                Nombre = "Exportar Usuarios",
                Descripcion = null,
            };

            var resultPost = await controller.PostPermiso(nuevoPermiso);
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultPost.Result);
            var permiso = Assert.IsType<Permiso>(createdResult.Value);

            // Act: Llamar al método del controlador           
            var result = await controller.DeletePermiso(permiso.Id);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        public ConnectionMultiplexer GetConnectionMultiplexer()
        {
            var configuration = ConfigurationOptions.Parse("redis-13961.c251.east-us-mz.azure.redns.redis-cloud.com:13961,password=C30UCMzzkVaB2UVvKpRpCQeclYDEmYLn", true);
            return ConnectionMultiplexer.Connect(configuration);
        }
    }
}