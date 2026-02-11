using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaAPI.Models
{
    public class Prestamo
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Libro")]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El Nombre del Usuario es obligatorio")]
        [MaxLength(100, ErrorMessage = "El Nombre no puede exceder los 100 caracteres")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime FechaPrestamo { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [MaxLength(20, ErrorMessage = "El estado no puede exceder los 20 caracteres")]
        public string Estado { get; set; } = "activo";

        public int DiasRetraso { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Multa { get; set; } = 0;

        public Libro? Libro { get; set; }


    }
}
