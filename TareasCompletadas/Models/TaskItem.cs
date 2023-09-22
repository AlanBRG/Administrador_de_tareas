using System;
using System.ComponentModel.DataAnnotations;

namespace TareasCompletadas.Models
{
    public class TaskItem
    {
        public int Id{ get; set; }
        [Required]
        public string Titulo{ get; set; }
        [Required]
        [MaxLength(200)]
        public string Descripcion{ get; set; }
        private DateTime _fechaEntrega;
        private bool _isCompleted;

        public DateTime FechaEntrega
        {
            get { return _fechaEntrega; }
            set { _fechaEntrega = value; }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; } //Propiedad publica para acceder al estado de completado
            set
            {
                if (_isCompleted != value) //Comprobar si el valor esta cambiando
                {
                    _isCompleted = value; //Establecer el valor de completado 
                    if (_isCompleted)
                    {
                        _fechaEntrega = DateTime.Now; //Si se marca como completado, actualizar la fecha de entrega
                    }
                }
            }
        }
    }
}
