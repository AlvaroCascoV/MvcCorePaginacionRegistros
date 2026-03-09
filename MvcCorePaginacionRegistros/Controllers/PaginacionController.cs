using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class PaginacionController : Controller
    {
        private RepositoryHospital repo;

        public PaginacionController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> RegistroVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numRegistros = await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            //PRIMERO = 1
            //ULTIMO = 4
            //ANTERIOR = posicion - 1
            //SIGUIENTE = posicion + 1
            int siguiente = posicion.Value + 1;
            if(siguiente > numRegistros)
            {
                //si queremos dar la vuelta a la paginacion, el siguiente del ultimo seria el 1
                siguiente = numRegistros;
            }
            int anterior = posicion.Value - 1;
            if(anterior < 1)
            {
                //si queremos dar la vuelta a la paginacion, el anterior del primero seria el numRegistros
                anterior = 1;
            }
            ViewData["ULTIMO"] = numRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            VistaDepartamento departamento = await this.repo.GetVistaDepartamentoAsync(posicion.Value);
            return View(departamento);
        }

        public async Task<IActionResult> GrupoVistaDepartamentos(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            //LO SIGUIENTE SERA QUE DEBEMOS DIBUJAR LOS NUMEROS
            //DE PAGINA EN LOS LINKS por ejemplo:
            //<a href="grupodepts?posicion=1">pagina 1</a>
            //<a href="grupodepts?posicion=3">pagina 2</a>
            //<a href="grupodepts?posicion=5">pagina 3</a>
            //NECESITAMOS UNA VARIABLE PARA EL NUMERO DE PAGINA
            //VOY A REALIZAR EL DIBUJO DESDE AQUI, NO DESDE RAZOR.
            int numPagina = 1;
            int numRegistros = await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            string html = "<div>";
            for (int i = 1; i <= numRegistros; i += 2) //el 2 es el numero de registros que se muestran en cada pagina
            {
                html += "<a href='GrupoVistaDepartamentos?posicion=" + i + "'>Página " + numPagina + "</a> || ";
                numPagina += 1;
            }
            html += "</div>";
            ViewData["NREGISTROS"] = numRegistros;
            ViewData["LINKS"] = html;
            List<VistaDepartamento> departamentos = await this.repo.GetGrupoVistaDepartamentoAsync(posicion.Value);
            return View(departamentos);
        }
        public async Task<IActionResult> GrupoDepartamentos(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numRegistros = await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            
            ViewData["NREGISTROS"] = numRegistros;
            List<Departamento> departamentos = await this.repo.GetGrupoDepartamentosAsync(posicion.Value);
            return View(departamentos);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
