using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UsuarioAPI.Models;
using System.Text.Json;

namespace UsuarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly UsuarioContext _context;
        private readonly IConnectionMultiplexer _redis;

        public PermisosController(UsuarioContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Permisos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permiso>>> GetPermisos()
        {
            var db = _redis.GetDatabase();
            string cacheKey = "permisoList";
            var permisosCache = await db.StringGetAsync(cacheKey);
            if (!permisosCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<Permiso>>(permisosCache);
            }
            var permisos = await _context.Permisos.ToListAsync();
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(permisos), TimeSpan.FromMinutes(10));
            return permisos;
        }

        // GET: api/Permisos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Permiso>> GetPermiso(int id)
        {
            var db = _redis.GetDatabase();
            string cacheKey = "permiso_" + id.ToString();
            var permisoCache = await db.StringGetAsync(cacheKey);

            if (!permisoCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<Permiso>(permisoCache);
            }
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(permiso), TimeSpan.FromSeconds(10));
            return permiso;
        }

        // PUT: api/Permisos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermiso(int id, Permiso permiso)
        {
            if (id != permiso.Id)
            {
                return BadRequest();
            }

            _context.Entry(permiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var db = _redis.GetDatabase();
                string cacheKeyPermiso = "permiso_" + id.ToString();
                string cacheKeyList = "permisoList";
                await db.KeyDeleteAsync(cacheKeyPermiso);
                await db.KeyDeleteAsync(cacheKeyList);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermisoExists(id))
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

        // POST: api/Permisos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Permiso>> PostPermiso(Permiso permiso)
        {
            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyList = "permisoList";
            await db.KeyDeleteAsync(cacheKeyList);
            return CreatedAtAction("GetPermiso", new { id = permiso.Id }, permiso);
        }

        // DELETE: api/Permisos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermiso(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            _context.Permisos.Remove(permiso);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyPermiso = "permiso_" + id.ToString();
            string cacheKeyList = "permisoList";
            await db.KeyDeleteAsync(cacheKeyPermiso);
            await db.KeyDeleteAsync(cacheKeyList);
            return NoContent();
        }

        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(e => e.Id == id);
        }
    }
}
