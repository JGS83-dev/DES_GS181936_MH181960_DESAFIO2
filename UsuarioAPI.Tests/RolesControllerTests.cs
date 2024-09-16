using UsuarioAPI.Models;
using UsuarioAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace UsuarioAPI.Tests
{
    public class RolesControllerTests
    {
        [Fact]
        public async Task PostRole_AgregaRole_CuandoRoleEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new RolesController(context, redis);

            // Act: Llamar al método del controlador
            var nuevoRol = new Rol
            {
                Nombre = "Vendedor 3",
                Descripcion = null,
            };

            var result = await controller.PostRol(nuevoRol);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var rol = Assert.IsType<Rol>(createdResult.Value);
        }

        [Fact]
        public async Task PostRol_NoAgregaRol_CuandoNombreEsNulo()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new RolesController(context, redis);

            // Act: Llamar al método del controlador
            var nuevoRol = new Rol
            {
                Nombre = null,
                Descripcion = null,
            };

            var result = await controller.PostRol(nuevoRol);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostRol_NoAgregaRol_CuandoNombreContieneMenosDe3Caracteres()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new RolesController(context, redis);

            // Act: Llamar al método del controlador
            var nuevoRol = new Rol
            {
                Nombre = "Si",
                Descripcion = null,
            };

            var result = await controller.PostRol(nuevoRol);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }        

        [Fact]
        public void GetRol_RetornaRol_CuandoIdEsValido()
        {
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new RolesController(context, redis);

            // Act: Llamar al método del controlador
            var result = controller.GetRol(1);

            // Assert: Verificar que se obtiene el resultado correcto
            var actionResult = Assert.IsType<ActionResult<Rol>>(result.Result);
            var returnValue = Assert.IsType<Rol>(actionResult.Value);            
        }

        [Fact]
        public async Task GetRol_RetornaRol_CuandoIdNoEsValido()
        {
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new RolesController(context, redis);

            // Act: Llamar al método del controlador
            var result = await controller.GetRol(999);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateRol_ActualizaRol_CuandoRolEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new RolesController(context, redis);

            // Act: Llamar al método del controlador
            var rol = new Rol
            {
                Id = 2,
                Nombre = "Vendedor 2",
                Descripcion = "Ahora es personal ...",
            };

            var result = await controller.PutRol(2, rol);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteRol_EliminaRol_CuandoIdNoEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new RolesController(context, redis);

            // Act: Llamar al método del controlador           
            var result = await controller.DeleteRol(999);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteRol_EliminaRol_CuandoIdEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new RolesController(context, redis);

            // Act: Llamar al método del controlador           
            var result = await controller.DeleteRol(3);

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