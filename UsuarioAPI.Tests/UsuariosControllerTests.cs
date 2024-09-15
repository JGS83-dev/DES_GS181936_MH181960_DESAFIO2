using UsuarioAPI.Models;
using UsuarioAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace UsuarioAPI.Tests
{
    public class UsuariosControllerTests
    {
        [Fact]
        public void GetUsuario_RetornaUsuario_CuandoIdEsValido()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();

            var configuration = ConfigurationOptions.Parse("redis-13961.c251.east-us-mz.azure.redns.redis-cloud.com:13961,password=C30UCMzzkVaB2UVvKpRpCQeclYDEmYLn", true);
            var redis = ConnectionMultiplexer.Connect(configuration);

            var controller = new UsuariosController(context, redis);

            //Act
            var result = controller.GetUsuario(1);

            //Assert
            var actionResult = Assert.IsType<ActionResult<Usuario>>(result);
            var returnValue = Assert.IsType<Usuario>(actionResult.Value);
            Assert.Equal("SysAdmin", returnValue.Nombre);
        }
    }
}