namespace BibliotecaAPI.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int AnoPublicacion { get; set; }
        public string NombreAutor { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
    }
}
