namespace MvcCorePaginacionRegistros.Models
{
    public class ModelEmpDept
    {
        Departamento departamento { get; set; }
        List<Empleado> empleados { get; set; }
        int NRegistros { get; set; }
    }
}
