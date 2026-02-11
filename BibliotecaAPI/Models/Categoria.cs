using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "La descripcion no puede exceder 200 caracteres")]
        public string? Descripcion { get; set; }
        public ICollection<Libro>? Libros { get; set; }


    }

}
