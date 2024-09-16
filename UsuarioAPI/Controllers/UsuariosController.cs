using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuarioAPI.Models;
using StackExchange.Redis;
using System.Text.Json;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace UsuarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioContext _context;
        private readonly IConnectionMultiplexer _redis;

        public UsuariosController(UsuarioContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var db = _redis.GetDatabase();
            string cacheKey = "usuarioList";
            var usuariosCache = await db.StringGetAsync(cacheKey);
            if (!usuariosCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<Usuario>>(usuariosCache);
            }
            var usuarios = await _context.Usuarios.ToListAsync();
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(usuarios), TimeSpan.FromMinutes(10));
            return usuarios;
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Usuario), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var db = _redis.GetDatabase();
            string cacheKey = "usuario_" + id.ToString();
            var usuarioCache = await db.StringGetAsync(cacheKey);

            if (!usuarioCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<Usuario>(usuarioCache);
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(usuario), TimeSpan.FromSeconds(10));
            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Password))
            {
                return BadRequest("El nombre, email y contraseña no puede estar vació.");
            }

            if (usuario.Nombre.Length < 3 || usuario.Nombre.Length > 50)
            {
                return BadRequest("La longitud mínima de Nombre es de 3 caracteres y máxima de 50.");
            }

            if (usuario.Password.Length < 8)
            {
                return BadRequest("La contraseña debe tener al menos 8 caracteres.");
            }

            if (!this.EsCorreoValido(usuario.Email))
            {
                return BadRequest("El Correo Electrónico no es valido.");
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var db = _redis.GetDatabase();
                string cacheKeyUsuario = "usuario_" + id.ToString();
                string cacheKeyList = "usuarioList";
                await db.KeyDeleteAsync(cacheKeyUsuario);
                await db.KeyDeleteAsync(cacheKeyList);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Password))
            {
                return BadRequest("El nombre, email y contraseña no puede estar vació.");
            }

            if (usuario.Nombre.Length < 3 || usuario.Nombre.Length > 50)
            {
                return BadRequest("La longitud mínima de Nombre es de 3 caracteres y máxima de 50.");
            }            

            if (usuario.Password.Length < 8)
            {
                return BadRequest("La contraseña debe tener al menos 8 caracteres.");
            }

            if(!this.EsCorreoValido(usuario.Email))
            {
                return BadRequest("El Correo Electrónico no es valido.");
            }

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyList = "usuarioList";
            await db.KeyDeleteAsync(cacheKeyList);
            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyUsuario = "usuario_" + id.ToString();
            string cacheKeyList = "usuarioList";
            await db.KeyDeleteAsync(cacheKeyUsuario);
            await db.KeyDeleteAsync(cacheKeyList);
            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        public bool EsCorreoValido(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
