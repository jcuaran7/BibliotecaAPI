namespace BibliotecaAPI.DTOs
{
    public class PrestamoDTO
    {
        public int Id { get; set; }
        public string TituloLibro { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public int DiasLimite { get; set; } = 7; // Días permitidos (por defecto 7)

        public string Estado { get; set; } = string.Empty;
        public int DiasRetraso { get; set; }
        public decimal Multa { get; set; }
    }
}
