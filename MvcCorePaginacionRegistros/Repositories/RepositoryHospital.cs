using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.Repositories
{
    #region SQL
    /*
    alter view V_DEPARTAMENTOS_INDIVIDUAL
    as
    select CAST(
        ROW_NUMBER() over (order by DEPT_NO) as int) 
    		as POSICION,
    		DEPT_NO, DNOMBRE, LOC from DEPT
    go
    create procedure SP_GRUPO_DEPARTAMENTOS
    (@posicion int)
as

select DEPT_NO, DNOMBRE, LOC from V_DEPARTAMENTOS_INDIVIDUAL
WHERE POSICION >= @posicion and POSICION<(@posicion +2)

go
    */
    #endregion
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<int> GetNumeroRegistrosVistaDepartamentosAsync()
        {
            return await context.VistaDepartamentos.CountAsync();
        }

        public async Task<VistaDepartamento> GetVistaDepartamentoAsync(int posicion)
        {
            VistaDepartamento departamento = await this.context.VistaDepartamentos.Where(z => z.Posicion == posicion).FirstOrDefaultAsync();
            return departamento;
        }

        public async Task<List<VistaDepartamento>> GetGrupoVistaDepartamentoAsync(int posicion)
        {
            //para paginar de 2 en dos esta seria la consulta
            //select * from V_DEPARTAMENTOS_INDIVIDUAL where POSICION >= 1 and POSICION < (1 + 2)
            var consulta = from datos in this.context.VistaDepartamentos
                           where datos.Posicion >= posicion && datos.Posicion < (posicion + 2)
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<Departamento>> GetGrupoDepartamentosAsync(int posicion)
        {
            string sql = "SP_GRUPO_DEPARTAMENTOS @posicion";
            SqlParameter pamposicion = new SqlParameter("@posicion", posicion);
            var consulta = this.context.Departamentos.FromSqlRaw(sql, pamposicion);
            return await consulta.ToListAsync();
        }
    }
}
