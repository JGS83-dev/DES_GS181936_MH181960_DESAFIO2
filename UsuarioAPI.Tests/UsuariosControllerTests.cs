using UsuarioAPI.Models;
using UsuarioAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace UsuarioAPI.Tests
{
    public class UsuariosControllerTests
    {
        [Fact]
        public async Task PostUsuario_AgregaUsuario_CuandoUsuarioEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new UsuariosController(context, redis);

            // Act: Llamar al método del controlador
            var nuevoUsuario = new Usuario
            {
                Nombre = "Administrador",
                Email = "admin@udb.edu.sv",
                Password = "aguanteMillo",
                RolId = 1,
            };

            var result = await controller.PostUsuario(nuevoUsuario);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var usuario = Assert.IsType<Usuario>(createdResult.Value);
        }

        [Fact]
        public async Task PostUsuario_NoAgregaUsuario_CuandoNombreEsNulo()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new UsuariosController(context, redis);

            // Act: Llamar al método del controlador
            var nuevoUsuario = new Usuario
            {
                Nombre = null,
                Email = "sysadmin@udb.edu.sv",
                Password = "aguanteRiver",
                RolId = 1,
            };

            var result = await controller.PostUsuario(nuevoUsuario);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostUsuario_NoAgregaUsuario_CuandoPasswordNoContiene8Caracteres()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new UsuariosController(context, redis);

            // Act: Llamar al método del controlador
            var nuevoUsuario = new Usuario
            {
                Nombre = "SysAdmin 2",
                Email = "sysadmin@udb.edu.sv",
                Password = "river",
                RolId = 1,
            };

            var result = await controller.PostUsuario(nuevoUsuario);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostUsuario_NoAgregaUsuario_CuandoEmailNoEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new UsuariosController(context, redis);

            // Act: Llamar al método del controlador
            var nuevoUsuario = new Usuario
            {
                Nombre = "SysAdmin 5",
                Email = "aguanteRiver.com",
                Password = "aguanteRiver",
                RolId = 1,
            };

            var result = await controller.PostUsuario(nuevoUsuario);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void GetUsuario_RetornaUsuario_CuandoIdEsValido()
        {
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new UsuariosController(context, redis);            

            // Act: Llamar al método del controlador
            var result = controller.GetUsuario(1);

            // Assert: Verificar que se obtiene el resultado correcto
            var actionResult = Assert.IsType<ActionResult<Usuario>>(result.Result);
            var returnValue = Assert.IsType<Usuario>(actionResult.Value);
            Assert.Equal("SysAdmin", returnValue.Nombre);
        }

        [Fact]
        public async Task GetUsuario_RetornaUsuario_CuandoIdNoEsValido()
        {
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new UsuariosController(context, redis);

            // Act: Llamar al método del controlador
            var result = await controller.GetUsuario(999);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateUsuario_ActualizaUsuario_CuandoUsuarioEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new UsuariosController(context, redis);

            var nuevoUsuario = new Usuario
            {
                Nombre = "Contador",
                Email = "contador@udb.edu.sv",
                Password = "aguanteRiver",
                RolId = 1,
            };

            var resultPost = await controller.PostUsuario(nuevoUsuario);
            var createdResultPost = Assert.IsType<CreatedAtActionResult>(resultPost.Result);
            var usuario = Assert.IsType<Usuario>(createdResultPost.Value);

            // Act: Llamar al método del controlador
            usuario.Nombre = "Contador Externo";
            usuario.Email = "contadorexterno@udb.edu.sv";

            var result = await controller.PutUsuario(usuario.Id, usuario);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUsuario_EliminaUsuario_CuandoIdNoEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new UsuariosController(context, redis);

            // Act: Llamar al método del controlador           
            var result = await controller.DeleteUsuario(999);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUsuario_EliminaUsuario_CuandoIdEsValido()
        {
            //Arrange
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();
            var redis = this.GetConnectionMultiplexer();

            // Inicializar el controlador con el contexto y Redis
            var controller = new UsuariosController(context, redis);

            var nuevoUsuario = new Usuario
            {
                Nombre = "Vendedor",
                Email = "vendededor@udb.edu.sv",
                Password = "aguanteMillo",
                RolId = 1,
            };

            var resultPost = await controller.PostUsuario(nuevoUsuario);            
            var createdResultPost = Assert.IsType<CreatedAtActionResult>(resultPost.Result);
            var usuario = Assert.IsType<Usuario>(createdResultPost.Value);

            // Act: Llamar al método del controlador           
            var result = await controller.DeleteUsuario(usuario.Id);

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