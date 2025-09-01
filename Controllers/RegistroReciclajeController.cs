using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaReciclaje.Data;
using SistemaReciclaje.Models;

namespace SistemaReciclaje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroReciclajeController : ControllerBase
    {
        private readonly ReciclajeDbContext _context;

        public RegistroReciclajeController(ReciclajeDbContext context)
        {
            _context = context;
        }

        // GET: api/RegistroReciclaje
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroReciclaje>>> GetRegistros()
        {
            var registros = await _context.RegistrosReciclaje
                .Include(r => r.Usuario)
                .Include(r => r.Material)
                .Include(r => r.PuntoVerde)
                .ToListAsync();

            return Ok(registros);
        }

        // GET: api/RegistroReciclaje/usuario/5
        [HttpGet("usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<RegistroReciclaje>>> GetRegistrosPorUsuario(int userId)
        {
            var registros = await _context.RegistrosReciclaje
                .Include(r => r.Material)
                .Include(r => r.PuntoVerde)
                .Where(r => r.Id_Usuario == userId)
                .ToListAsync();

            return Ok(registros);
        }

        // POST: api/RegistroReciclaje
        [HttpPost]
        public async Task<ActionResult<RegistroReciclaje>> PostRegistro(RegistroReciclaje registro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // ✅ USAR TRANSACCIÓN PARA GARANTIZAR CONSISTENCIA
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Verificar que el usuario existe
                var usuario = await _context.Usuarios.FindAsync(registro.Id_Usuario);
                if (usuario == null)
                {
                    return BadRequest("El usuario no existe");
                }

                // Verificar que el material existe
                var material = await _context.Materiales.FindAsync(registro.Id_Material);
                if (material == null)
                {
                    return BadRequest("El material no existe");
                }

                // Verificar que el punto verde existe
                var puntoVerde = await _context.PuntosVerdes.FindAsync(registro.Id_PuntoVerde);
                if (puntoVerde == null)
                {
                    return BadRequest("El punto verde no existe");
                }

                // Calcular puntos obtenidos
                registro.PuntosObtenidos = (int)(registro.CantidadKg * material.PuntosPorKg);

                // ✅ ACTUALIZAR PUNTOS DEL USUARIO DE FORMA EXPLÍCITA
                usuario.PuntosAcumulados += registro.PuntosObtenidos;

                // Agregar registro
                _context.RegistrosReciclaje.Add(registro);

                // Guardar todos los cambios
                await _context.SaveChangesAsync();

                // Confirmar transacción
                await transaction.CommitAsync();

                // ✅ CARGAR EL REGISTRO CON TODAS SUS RELACIONES PARA LA RESPUESTA
                var registroCreado = await _context.RegistrosReciclaje
                    .Include(r => r.Usuario)
                    .Include(r => r.Material)
                    .Include(r => r.PuntoVerde)
                    .FirstOrDefaultAsync(r => r.Id == registro.Id);

                return CreatedAtAction("GetRegistros", new { id = registro.Id }, registroCreado);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Error interno del servidor al procesar el registro");
            }
        }

        // GET: api/RegistroReciclaje/estadisticas/usuario/5
        [HttpGet("estadisticas/usuario/{userId}")]
        public async Task<ActionResult> GetEstadisticasUsuario(int userId)
        {
            var registros = await _context.RegistrosReciclaje
                .Where(r => r.Id_Usuario == userId)
                .Include(r => r.Material)
                .ToListAsync();

            var estadisticas = new
            {
                TotalKgReciclados = registros.Sum(r => r.CantidadKg),
                TotalPuntosObtenidos = registros.Sum(r => r.PuntosObtenidos),
                RegistrosRealizados = registros.Count,
                MaterialesPorTipo = registros
                    .GroupBy(r => r.Material?.Nombre)
                    .Select(g => new { Material = g.Key, Cantidad = g.Sum(x => x.CantidadKg) })
                    .ToList()
            };

            return Ok(estadisticas);
        }
    }
}