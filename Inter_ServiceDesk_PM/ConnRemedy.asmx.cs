using Entities;
using ServiceIMSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Inter_ServiceDesk_PM
{
    /// <summary>
    /// Summary description for MesaIMSS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MesaIMSS : System.Web.Services.WebService
    {
        Incidentes inc = new Incidentes();
        OrdenesTrabajo wo = new OrdenesTrabajo();
        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        [WebMethod]
        public Result ActualizaCategorizacionIncidente(Categorizacion categorizacion)
        {
            return inc.ActualizaCategorizacion(categorizacion);
        }

        [WebMethod]
        public Result ActualizaIncidente(Incidente incidente)
        {
            return inc.ActualizaIncidente(incidente);
        }

        [WebMethod]
        public Result ActualizaPrioridadIncidente(Prioridad prioridad)
        {
            return inc.ActualizaPrioridad(prioridad);
        }

        [WebMethod]
        public Result AdicionaNotasIncidente(Comentario comentario)
        {
            return inc.AdicionaNotas(comentario);
        }

        [WebMethod]
        public Result ActualizaCategorizacionOrdenTrabajo(Categorizacion categorizacion)
        {
            return wo.ActualizaCategorizacion(categorizacion);
        }

        [WebMethod]
        public Result ActualizaOrdenTrabajo(OrdenTrabajo ordenTrabajo)
        {
            return wo.ActualizaOrdenes(ordenTrabajo);
        }

        [WebMethod]
        public Result ActualizaPrioridadOrdenTrabajo(PrioridadOT prioridad)
        {
            return wo.ActualizaPrioridad(prioridad);
        }

        [WebMethod]
        public Result AdicionaNotasOrdenTrabajo(Comentario comentario)
        {
            return wo.AdicionaNotas(comentario);
        }
    }
}
