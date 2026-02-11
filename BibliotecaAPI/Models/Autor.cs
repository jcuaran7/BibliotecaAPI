using System.ComponentModel.DataAnnotations;


namespace BibliotecaAPI.Models
{
    public class Autor
    {

        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "La nacionalidad no puede exceder 50 caracteres")]
        public string? Nacionalidad { get; set; }

        public DateTime? FechaNacimiento { get; set; }
        public ICollection<Libro>? Libros { get; set; }



    }
}
