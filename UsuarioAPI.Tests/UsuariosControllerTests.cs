using UsuarioAPI.Models;
using UsuarioAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Configuration;

namespace UsuarioAPI.Tests
{
    public class UsuariosControllerTests
    {
        [Fact]
        public void GetUsuario_RetornaUsuario_CuandoIdEsValido()
        {
            // Arrange: Configuración del contexto y las dependencias externas
            var context = Setup.GetDatabaseContext();

            var configuration = ConfigurationOptions.Parse("redis-13961.c251.east-us-mz.azure.redns.redis-cloud.com:13961,password=C30UCMzzkVaB2UVvKpRpCQeclYDEmYLn", true);
            var redis = ConnectionMultiplexer.Connect(configuration);            

            // Inicializar el controlador con el contexto y Redis
            var controller = new UsuariosController(context, redis);

            // Act: Llamar al método del controlador
            var result = controller.GetUsuario(1);

            // Assert: Verificar que se obtiene el resultado correcto
            var actionResult = Assert.IsType<ActionResult<Usuario>>(result.Result);
            var returnValue = Assert.IsType<Usuario>(actionResult.Value);
            Assert.Equal("SysAdmin", returnValue.Nombre);
        }
    }
}