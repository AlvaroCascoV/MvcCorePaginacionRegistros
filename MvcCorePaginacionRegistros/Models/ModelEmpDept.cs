using Microsoft.EntityFrameworkCore;

namespace MvcCorePaginacionRegistros.Models
{
    [Keyless]
    public class ModelEmpDept
    {
        public List<Empleado> Empleados { get; set; }
        public int NumeroRegistros { get; set; }
    }
}
