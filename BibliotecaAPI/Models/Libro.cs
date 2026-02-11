using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaAPI.Models
{
    public class Libro
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Titulo es obligatorio")]
        [MaxLength(150, ErrorMessage = "El Titulo no puede exceder los 150 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es obligatorio")]
        [MaxLength(13, ErrorMessage = "El ISBN no puede exceder los 13 caracteres")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Año de publicacion es obligatorio")]
        public int AnoPublicacion { get; set; }

        [ForeignKey("Autor")]
        public int AutorId { get; set; }

        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public Autor? Autor { get; set; }
        public Categoria? Categoria { get; set; }
        public ICollection<Prestamo>? Prestamos { get; set; }


    }
}
