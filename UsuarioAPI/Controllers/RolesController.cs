using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuarioAPI.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace UsuarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UsuarioContext _context;
        private readonly IConnectionMultiplexer _redis;

        public RolesController(UsuarioContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRoles()
        {
            var db = _redis.GetDatabase();
            string cacheKey = "rolList";
            var rolesCache = await db.StringGetAsync(cacheKey);
            if (!rolesCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<Rol>>(rolesCache);
            }
            var roles = await _context.Roles.ToListAsync();
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(roles), TimeSpan.FromMinutes(10));
            return roles;
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var db = _redis.GetDatabase();
            string cacheKey = "rol_" + id.ToString();
            var rolCache = await db.StringGetAsync(cacheKey);

            if (!rolCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<Rol>(rolCache);
            }
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(rol), TimeSpan.FromSeconds(10));
            return rol;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, Rol rol)
        {            
            if (id != rol.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(rol.Nombre))
            {
                return BadRequest("El nombre no puede estar vació.");
            }

            if (rol.Nombre.Length < 3 || rol.Nombre.Length > 30)
            {
                return BadRequest("La longitud mínima del nombre es de 3 caracteres y máxima de 30.");
            }

            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var db = _redis.GetDatabase();
                string cacheKeyRol = "rol_" + id.ToString();
                string cacheKeyList = "rolList";
                await db.KeyDeleteAsync(cacheKeyRol);
                await db.KeyDeleteAsync(cacheKeyList);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol(Rol rol)
        {
            if (string.IsNullOrEmpty(rol.Nombre))
            {
                return BadRequest("El nombre no puede estar vació.");
            }
            
            if (rol.Nombre.Length < 3 || rol.Nombre.Length > 30)
            {
                return BadRequest("La longitud mínima del nombre es de 3 caracteres y máxima de 30.");
            }

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyList = "rolList";
            await db.KeyDeleteAsync(cacheKeyList);
            return CreatedAtAction("GetRol", new { id = rol.Id }, rol);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyRol = "rol_" + id.ToString();
            string cacheKeyList = "rolList";
            await db.KeyDeleteAsync(cacheKeyRol);
            await db.KeyDeleteAsync(cacheKeyList);
            return NoContent();
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
