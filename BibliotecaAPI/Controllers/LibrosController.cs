using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using BibliotecaAPI.DTOs;

namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetLibros()
        {
            var libros = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Select(l => new LibroDTO
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    ISBN = l.ISBN,
                    AnoPublicacion = l.AnoPublicacion,
                    NombreAutor = l.Autor != null ? l.Autor.Nombre : "",
                    NombreCategoria = l.Categoria != null ? l.Categoria.Nombre : ""
                })
                .ToListAsync();

            return libros;
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroDTO>> GetLibro(int id)
        {
            var libro = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Where(l => l.Id == id)
                .Select(l => new LibroDTO
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    ISBN = l.ISBN,
                    AnoPublicacion = l.AnoPublicacion,
                    NombreAutor = l.Autor != null ? l.Autor.Nombre : "",
                    NombreCategoria = l.Categoria != null ? l.Categoria.Nombre : ""
                })
                .FirstOrDefaultAsync();

            if (libro == null)
            {
                return NotFound();
            }
            return libro;
        }

        // POST: api/Libros
        [HttpPost]
        public async Task<ActionResult<LibroDTO>> PostLibro(CreateLibroDTO createDto)
        {
            var libro = new Libro
            {
                Titulo = createDto.Titulo,
                ISBN = createDto.ISBN,
                AnoPublicacion = createDto.AnoPublicacion,
                AutorId = createDto.AutorId,
                CategoriaId = createDto.CategoriaId
            };

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            // Cargar el libro con sus relaciones y convertir a DTO
            var libroDTO = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Where(l => l.Id == libro.Id)
                .Select(l => new LibroDTO
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    ISBN = l.ISBN,
                    AnoPublicacion = l.AnoPublicacion,
                    NombreAutor = l.Autor != null ? l.Autor.Nombre : "",
                    NombreCategoria = l.Categoria != null ? l.Categoria.Nombre : ""
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetLibro), new { id = libro.Id }, libroDTO);
        }


        // PUT: api/Libros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }
            _context.Entry(libro).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(id))
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

        // DELETE: api/Libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}
