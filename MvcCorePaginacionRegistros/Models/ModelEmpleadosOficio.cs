using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCorePaginacionRegistros.Models
{
    public class ModelEmpleadosOficio
    {
        public List<Empleado> Empleados { get; set; }
        public int NumeroRegistros { get; set; }
    }
}
