using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaReciclaje.Data;
using SistemaReciclaje.Models;

namespace SistemaReciclaje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialesController : ControllerBase
    {
        private readonly ReciclajeDbContext _context;

        public MaterialesController(ReciclajeDbContext context)
        {
            _context = context;
        }

        // GET: api/Materiales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetMateriales()
        {
            return await _context.Materiales.Where(m => m.Activo).ToListAsync();
        }

        // GET: api/Materiales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterial(int id)
        {
            var material = await _context.Materiales.FindAsync(id);

            if (material == null)
            {
                return NotFound("Material no encontrado");
            }

            return Ok(material);
        }

        // POST: api/Materiales
        [HttpPost]
        public async Task<ActionResult<Material>> PostMaterial(Material material)
        {
            var existeMaterial = await _context.Materiales
                .AnyAsync(m => m.Nombre == material.Nombre);

            if (existeMaterial)
            {
                return BadRequest("Ya existe un material con este nombre");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Materiales.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaterial", new { id = material.Id }, material);
        }

        // PUT: api/Materiales/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial(int id, Material material)
        {
            if (id != material.Id)
            {
                return BadRequest("No coincide el ID");
            }

            _context.Entry(material).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
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

        // DELETE: api/Materiales
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.Materiales.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            material.Activo = false; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool MaterialExists(int id)
        {
            return _context.Materiales.Any(e => e.Id == id);
        }
    }
}