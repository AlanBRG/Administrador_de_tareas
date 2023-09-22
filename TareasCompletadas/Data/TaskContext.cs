using Microsoft.EntityFrameworkCore;
using TareasCompletadas.Models;

namespace TareasCompletadas.Data
{
    public class TaskContext: DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }

        public DbSet<TaskHistorial> Historial { get; set; }

        public TaskContext(DbContextOptions options): base(options) 
        {
            
        }

    }

}
