using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaReciclaje.Data;
using SistemaReciclaje.Models;

namespace SistemaReciclaje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuntosVerdesController : ControllerBase
    {
        private readonly ReciclajeDbContext _context;

        public PuntosVerdesController(ReciclajeDbContext context)
        {
            _context = context;
        }

        // GET: api/PuntosVerdes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PuntoVerde>>> GetPuntosVerdes()
        {
            return await _context.PuntosVerdes.Where(p => p.Activo).ToListAsync();
        }

        // GET: api/PuntosVerdes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PuntoVerde>> GetPuntoVerde(int id)
        {
            var puntoVerde = await _context.PuntosVerdes.FindAsync(id);

            if (puntoVerde == null)
            {
                return NotFound("Punto verde no encontrado");
            }

            return Ok(puntoVerde);
        }

        // GET: api/PuntosVerdes/cercanos?lat=&lng=
        [HttpGet("cercanos")]
        public async Task<ActionResult<IEnumerable<PuntoVerde>>> GetPuntosCercanos(double lat, double lng)
        {
            var puntos = await _context.PuntosVerdes
                .Where(p => p.Activo)
                .ToListAsync();

            // Filtrar por distancia (simplificado)
            var puntosCercanos = puntos
                .Where(p => Math.Abs(p.Latitud - lat) < 0.01 && Math.Abs(p.Longitud - lng) < 0.01)
                .ToList();

            return Ok(puntosCercanos);
        }

        // POST: api/PuntosVerdes
        [HttpPost]
        public async Task<ActionResult<PuntoVerde>> PostPuntoVerde(PuntoVerde puntoVerde)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PuntosVerdes.Add(puntoVerde);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPuntoVerde", new { id = puntoVerde.Id }, puntoVerde);
        }

        // PUT: api/PuntosVerdes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPuntoVerde(int id, PuntoVerde puntoVerde)
        {
            if (id != puntoVerde.Id)
            {
                return BadRequest("No coincide el ID");
            }

            _context.Entry(puntoVerde).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuntoVerdeExists(id))
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

        // DELETE: api/PuntosVerdes
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePuntoVerde(int id)
        {
            var puntoVerde = await _context.PuntosVerdes.FindAsync(id);
            if (puntoVerde == null)
            {
                return NotFound();
            }

            puntoVerde.Activo = false; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PuntoVerdeExists(int id)
        {
            return _context.PuntosVerdes.Any(e => e.Id == id);
        }
    }
}