using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs
{
    public class CreateLibroDTO
    {
        [Required(ErrorMessage = "El Titulo es obligatorio")]
        [MaxLength(150, ErrorMessage = "El Titulo no puede exceder los 150 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es obligatorio")]
        [MaxLength(13, ErrorMessage = "El ISBN no puede exceder los 13 caracteres")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Año de publicacion es obligatorio")]
        [Range(1000, 2100, ErrorMessage = "El año debe estar entre 1000 y 2100")]
        public int AnoPublicacion { get; set; }

        [Required(ErrorMessage = "El AutorId es obligatorio")]
        public int AutorId { get; set; }

        [Required(ErrorMessage = "La CategoriaId es obligatoria")]
        public int CategoriaId { get; set; }
    }
}
