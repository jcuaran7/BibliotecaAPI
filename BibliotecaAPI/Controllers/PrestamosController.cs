using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using BibliotecaAPI.DTOs;


namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public PrestamosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Prestamos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrestamoDTO>>> GetPrestamos(
            string? estado = null,           // Filtro opcional por estado
            string? nombreUsuario = null     // Filtro opcional por nombre de usuario
        )
        {
            // Empezar con todos los préstamos
            var query = _context.Prestamos
                .Include(p => p.Libro)
                    .ThenInclude(l => l.Autor)
                .Include(p => p.Libro)
                    .ThenInclude(l => l.Categoria)
                .AsQueryable(); // ← Permite agregar filtros dinámicamente

            // FILTRO 1: Por estado (si se proporciona)
            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(p => p.Estado == estado);
            }

            // FILTRO 2: Por nombre de usuario (si se proporciona)
            if (!string.IsNullOrEmpty(nombreUsuario))
            {
                query = query.Where(p => p.NombreUsuario.Contains(nombreUsuario));
            }

            // Ejecutar la consulta y convertir a DTO
            return await query
                .Select(p => new PrestamoDTO
                {
                    Id = p.Id,
                    TituloLibro = p.Libro != null ? p.Libro.Titulo : "",
                    NombreUsuario = p.NombreUsuario,
                    FechaPrestamo = p.FechaPrestamo,
                    FechaDevolucion = p.FechaDevolucion,
                    Estado = p.Estado,
                    DiasRetraso = p.DiasRetraso,
                    Multa = p.Multa
                })
                .ToListAsync();
        }


        // GET: api/Prestamos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrestamoDTO>> GetPrestamo(int id)
        {
            var prestamoDTO = await _context.Prestamos
                .Include(p => p.Libro)
                    .ThenInclude(l => l.Autor)
                .Include(p => p.Libro)
                    .ThenInclude(l => l.Categoria).Where(p => p.Id == id)
                    .Select(p => new PrestamoDTO
                    {
                        Id = p.Id,
                        TituloLibro = p.Libro != null ? p.Libro.Titulo : "",
                        NombreUsuario = p.NombreUsuario,
                        FechaPrestamo = p.FechaPrestamo,
                        FechaDevolucion = p.FechaDevolucion,
                        Estado = p.Estado,
                        DiasRetraso = p.DiasRetraso,
                        Multa = p.Multa
                    })
                    .FirstOrDefaultAsync();

                if (prestamoDTO == null)
                {
                    return NotFound();
                }
                return prestamoDTO;
        }

        // POST: api/Prestamos
        [HttpPost]
        public async Task<ActionResult<PrestamoDTO>> PostPrestamo(CreatePrestamoDTO createDto)
        {
            // VALIDACIÓN 1: Verificar que el libro existe
            var libro = await _context.Libros.FindAsync(createDto.LibroId);
            if (libro == null)
            {
                return NotFound($"No existe un libro con el ID {createDto.LibroId}");
            }

            // VALIDACIÓN 2: Verificar que el libro no esté prestado actualmente
            var prestamoActivo = await _context.Prestamos
                .AnyAsync(p => p.LibroId == createDto.LibroId && p.Estado == "Activo");

            if (prestamoActivo)
            {
                return BadRequest($"El libro '{libro.Titulo}' ya está prestado y no está disponible");
            }

            // VALIDACIÓN 3: La fecha de préstamo no puede ser futura
            if (createDto.FechaPrestamo > DateTime.Now)
            {
                return BadRequest("La fecha de préstamo no puede ser futura");
            }

            // Crear el modelo Prestamo a partir del DTO
            var prestamo = new Prestamo
            {
                LibroId = createDto.LibroId,
                NombreUsuario = createDto.NombreUsuario,
                FechaPrestamo = createDto.FechaPrestamo,
                FechaDevolucion = null,      // Aún no se devolvió
                Estado = "Activo",           // Préstamo en curso
                DiasRetraso = 0,             // Sin retraso al crear
                Multa = 0                    // Sin multa al crear
            };

            // Guardar en la base de datos
            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            // Consultar el préstamo guardado con sus relaciones
            var prestamoDTO = await _context.Prestamos
                .Include(p => p.Libro)
                .Where(p => p.Id == prestamo.Id)
                .Select(p => new PrestamoDTO
                {
                    Id = p.Id,
                    TituloLibro = p.Libro != null ? p.Libro.Titulo : "",
                    NombreUsuario = p.NombreUsuario,
                    FechaPrestamo = p.FechaPrestamo,
                    FechaDevolucion = p.FechaDevolucion,
                    Estado = p.Estado,
                    DiasRetraso = p.DiasRetraso,
                    Multa = p.Multa
                })
                .FirstOrDefaultAsync();

            // Devolver el DTO al cliente
            return CreatedAtAction(nameof(GetPrestamo), new { id = prestamo.Id }, prestamoDTO);
        }


        // PUT: api/Prestamos/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PrestamoDTO>> PutPrestamo(int id, DevolverLibroDTO devolverDto)
        {
            // Buscar el préstamo
            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
            {
                return NotFound($"No existe un préstamo con el ID {id}");
            }

            // VALIDACIÓN 1: No se puede devolver un libro ya devuelto
            if (prestamo.Estado == "Devuelto" || prestamo.Estado == "Vencido")
            {
                return BadRequest("Este libro ya fue devuelto anteriormente");
            }

            // VALIDACIÓN 2: La fecha de devolución debe ser posterior a la de préstamo
            if (devolverDto.FechaDevolucion <= prestamo.FechaPrestamo)
            {
                return BadRequest("La fecha de devolución debe ser posterior a la fecha de préstamo");
            }

            // VALIDACIÓN 3: La fecha de devolución no puede ser futura
            if (devolverDto.FechaDevolucion > DateTime.Now)
            {
                return BadRequest("La fecha de devolución no puede ser futura");
            }

            // Actualizar fecha de devolución
            prestamo.FechaDevolucion = devolverDto.FechaDevolucion;

            // Calcular fecha límite (fecha préstamo + días límite)
            var fechaLimite = prestamo.FechaPrestamo.AddDays(devolverDto.DiasLimite);

            // Calcular días de retraso (si pasó la fecha límite)
            if (devolverDto.FechaDevolucion > fechaLimite)
            {
                // CASO: DEVUELTO TARDE
                prestamo.DiasRetraso = (devolverDto.FechaDevolucion - fechaLimite).Days;
                prestamo.Multa = prestamo.DiasRetraso * 1000; // $1000 por día de retraso
                prestamo.Estado = "Vencido";
            }
            else
            {
                // CASO: DEVUELTO A TIEMPO
                prestamo.DiasRetraso = 0;
                prestamo.Multa = 0;
                prestamo.Estado = "Devuelto";
            }

            // Guardar cambios
            _context.Entry(prestamo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Devolver el DTO actualizado
            var prestamoDTO = new PrestamoDTO
            {
                Id = prestamo.Id,
                TituloLibro = prestamo.Libro != null ? prestamo.Libro.Titulo : "",
                NombreUsuario = prestamo.NombreUsuario,
                FechaPrestamo = prestamo.FechaPrestamo,
                FechaDevolucion = prestamo.FechaDevolucion,
                Estado = prestamo.Estado,
                DiasRetraso = prestamo.DiasRetraso,
                Multa = prestamo.Multa
            };

            return Ok(prestamoDTO);
        }


        // DELETE: api/Prestamos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.Id == id);
        }
    }
}
