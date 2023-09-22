using System.ComponentModel.DataAnnotations;

namespace TareasCompletadas.Models
{
    public class TaskHistorial
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public  DateTime FechaEntrega { get; set; }
        public bool IsCompleted { get; set; } 
        public DateTime FechaCreacion { get; set; }
        

    }
}
