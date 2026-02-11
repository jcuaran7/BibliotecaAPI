namespace BibliotecaAPI.DTOs
{
    public class AutorDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Nacionalidad { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}
