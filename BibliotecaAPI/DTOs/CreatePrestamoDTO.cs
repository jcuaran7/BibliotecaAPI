using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs
{
    public class CreatePrestamoDTO
    {


        [Required(ErrorMessage = "El LibroId es obligatorio")]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El Nombre del Usuario es obligatorio")]
        [MaxLength(100, ErrorMessage = "El Nombre no puede exceder los 100 caracteres")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime FechaPrestamo { get; set; }

    }
}
