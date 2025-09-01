using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaReciclaje.Data;
using SistemaReciclaje.Models;

namespace SistemaReciclaje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiosController : ControllerBase
    {
        private readonly ReciclajeDbContext _context;

        public BeneficiosController(ReciclajeDbContext context)
        {
            _context = context;
        }

        // GET: api/Beneficios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Beneficio>>> GetBeneficios()
        {
            return await _context.Beneficios.Where(b => b.Activo).ToListAsync();
        }

        // GET: api/Beneficios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Beneficio>> GetBeneficio(int id)
        {
            var beneficio = await _context.Beneficios.FindAsync(id);

            if (beneficio == null)
            {
                return NotFound("Beneficio no encontrado");
            }

            return Ok(beneficio);
        }

        // POST: api/Beneficios
        [HttpPost]
        public async Task<ActionResult<Beneficio>> PostBeneficio(Beneficio beneficio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Beneficios.Add(beneficio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBeneficio", new { id = beneficio.Id }, beneficio);
        }

        // POST: api/Beneficios/canje
        [HttpPost("canje")]
        public async Task<ActionResult<CanjeBeneficio>> CanjearBeneficio(CanjeBeneficio canje)
        {
            // Verificar que el usuario existe y tiene suficientes puntos
            var usuario = await _context.Usuarios.FindAsync(canje.Id_Usuario);
            if (usuario == null)
            {
                return BadRequest("El usuario no existe");
            }

            var beneficio = await _context.Beneficios.FindAsync(canje.Id_Beneficio);
            if (beneficio == null)
            {
                return BadRequest("El beneficio no existe");
            }

            if (usuario.PuntosAcumulados < beneficio.PuntosRequeridos)
            {
                return BadRequest("El usuario no tiene suficientes puntos");
            }

            // Realizar el canje
            canje.PuntosUtilizados = beneficio.PuntosRequeridos;
            canje.FechaCanje = DateTime.Now;

            // Descontar puntos del usuario
            usuario.PuntosAcumulados -= beneficio.PuntosRequeridos;

            _context.CanjesBeneficios.Add(canje);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Canje realizado exitosamente", canje });
        }

        // GET: api/Beneficios/canjes/usuario/5
        [HttpGet("canjes/usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<CanjeBeneficio>>> GetCanjesUsuario(int userId)
        {
            var canjes = await _context.CanjesBeneficios
                .Include(c => c.Beneficio)
                .Where(c => c.Id_Usuario == userId)
                .ToListAsync();

            return Ok(canjes);
        }

        // GET: api/Beneficios/disponibles/usuario/5
        [HttpGet("disponibles/usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<Beneficio>>> GetBeneficiosDisponibles(int userId)
        {
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var beneficiosDisponibles = await _context.Beneficios
                .Where(b => b.Activo && b.PuntosRequeridos <= usuario.PuntosAcumulados)
                .ToListAsync();

            return Ok(beneficiosDisponibles);
        }
    }
}