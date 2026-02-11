namespace BibliotecaAPI.DTOs
{
    public class DevolverLibroDTO
    {
        public DateTime FechaDevolucion { get; set; }
        public int DiasLimite { get; set; } = 7; // Días permitidos para el préstamo (por defecto 7)
    }
}