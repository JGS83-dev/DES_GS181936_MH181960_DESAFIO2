using UsuarioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace UsuarioAPI.Tests
{
    public static class Setup
    {
        public static UsuarioContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<UsuarioContext>()
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=UsuarioAPI;Trusted_Connection=True;")
                .Options;

            var context = new UsuarioContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
