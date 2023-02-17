using Entities;
using ServiceBitacora;
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

        IncidenteData bitacora = new IncidenteData();
        OrdenTrabajoData bitacoraWO = new OrdenTrabajoData();

        [WebMethod]
        public Result IncidenteActualizaCategorizacion(Categorizacion categorizacion)
        {
            Result result = inc.ActualizaCategorizacion(categorizacion);

            //Registra bitacora
            if (result.Estatus)
            {
                Entities.Intermedio.ActualizaCategorizacion _cambio = new Entities.Intermedio.ActualizaCategorizacion();
                _cambio.TicketIMSS = categorizacion.IDTicketRemedy;
                _cambio.CategoriaOpe01 = categorizacion.CatOperacion01;
                _cambio.CategoriaOpe02 = categorizacion.CatOperacion02;
                _cambio.CategoriaOpe03 = categorizacion.CatOperacion03;
                _cambio.CategoriaPro01 = categorizacion.CatProducto01;
                _cambio.CategoriaPro02 = categorizacion.CatProducto02;
                _cambio.CategoriaPro03 = categorizacion.CatProducto03;
                _cambio.FechaCambio = DateTime.Now.ToString();

                bitacora.ActualizaCategoria(_cambio, Convert.ToInt32(categorizacion.IDTicketInvgate), out string msg);
            }
                
            return result;
        }

        [WebMethod]
        public Result IncidenteActualiza(Entities.Incidente incidente)
        {
            Result result = inc.ActualizaIncidente(incidente);

            //Registra bitacora
            if (result.Estatus)
            {
                Entities.Intermedio.ActualizaTicket _cambio = new Entities.Intermedio.ActualizaTicket();
                _cambio.TicketIMSS = incidente.IDTicketRemedy;
                _cambio.EstadoNuevo = incidente.EstadoNuevo == 0 ? "" : incidente.EstadoNuevo.ToString();
                _cambio.Motivo = incidente.MotivoEstado == 0 ? "" : incidente.MotivoEstado.ToString();
                _cambio.FechaCambio = DateTime.Now.ToString();

                bitacora.ActualizaIncidente(_cambio, Convert.ToInt32(incidente.IDTicketInvgate), out string msg);
            }

            return result;
        }

        [WebMethod]
        public Result IncidenteActualizaPrioridad(Prioridad prioridad)
        {
            Result result = inc.ActualizaPrioridad(prioridad);

            //Registra bitacora
            if (result.Estatus)
            {
                Entities.Intermedio.ActualizaPriorizacion _cambio = new Entities.Intermedio.ActualizaPriorizacion();
                _cambio.TicketIMSS = prioridad.IDTicketRemedy;
                _cambio.Impacto = prioridad.Impacto == 0 ? "" : prioridad.Impacto.ToString();
                _cambio.Urgencia = prioridad.Urgencia == 0 ? "" : prioridad.Urgencia.ToString();
                _cambio.FechaCambio = DateTime.Now.ToString();

                bitacora.ActualizaPriorizacion(_cambio, Convert.ToInt32(prioridad.IDTicketInvgate), out string msg);
            }

            return result;
        }

        [WebMethod]
        public Result IncidenteAdicionaNotas(Comentario comentario)
        {
            Result result = inc.AdicionaNotas(comentario);

            //Registra bitacora
            if (result.Estatus)
            {
                Entities.Intermedio.AgregaNota _cambio = new Entities.Intermedio.AgregaNota();
                _cambio.TicketIMSS = comentario.IDTicketRemedy;
                _cambio.Notas = comentario.Notas;

                bitacora.AgregaNota(_cambio, Convert.ToInt32(comentario.IDTicketInvgate), out string msg);
            }

            return result;
        }

        [WebMethod]
        public Result OrdenTrabajoActualizaCategorizacion(Categorizacion categorizacion)
        {
            Result result = wo.ActualizaCategorizacion(categorizacion);

            //Registra bitacora
            if (result.Estatus)
            {
                Entities.Intermedio.ActualizaCategorizacion _cambio = new Entities.Intermedio.ActualizaCategorizacion();
                _cambio.TicketIMSS = categorizacion.IDTicketRemedy;
                _cambio.CategoriaOpe01 = categorizacion.CatOperacion01;
                _cambio.CategoriaOpe02 = categorizacion.CatOperacion02;
                _cambio.CategoriaOpe03 = categorizacion.CatOperacion03;
                _cambio.CategoriaPro01 = categorizacion.CatProducto01;
                _cambio.CategoriaPro02 = categorizacion.CatProducto02;
                _cambio.CategoriaPro03 = categorizacion.CatProducto03;
                _cambio.FechaCambio = DateTime.Now.ToString();

                bitacoraWO.ActualizaCategoria(_cambio, Convert.ToInt32(categorizacion.IDTicketInvgate), out string msg);
            }

            return result;
        }

        [WebMethod]
        public Result OrdenTrabajoActualiza(Entities.OrdenTrabajo ordenTrabajo)
        {
            Result result = wo.ActualizaOrdenes(ordenTrabajo);

            //Registra bitacora
            if (result.Estatus)
            {
                Entities.Intermedio.ActualizaTicket _cambio = new Entities.Intermedio.ActualizaTicket();
                _cambio.TicketIMSS = ordenTrabajo.IDTicketRemedy;
                _cambio.EstadoNuevo = ordenTrabajo.EstadoNuevo == 0 ? "" : ordenTrabajo.EstadoNuevo.ToString();
                _cambio.Motivo = ordenTrabajo.MotivoEstado == 0 ? "" : ordenTrabajo.MotivoEstado.ToString();
                _cambio.FechaCambio = DateTime.Now.ToString();

                bitacoraWO.ActualizaOrdenTrabajo(_cambio, Convert.ToInt32(ordenTrabajo.IDTicketInvgate), out string msg);
            }

            return result;
        }

        [WebMethod]
        public Result OrdenTrabajoActualizaPrioridad(PrioridadOT prioridad)
        {
            Result result = wo.ActualizaPrioridad(prioridad);

            //Registra bitacora
            if (result.Estatus)
            {
                Entities.Intermedio.ActualizaPriorizacion _cambio = new Entities.Intermedio.ActualizaPriorizacion();
                _cambio.TicketIMSS = prioridad.IDTicketRemedy;
                _cambio.Prioridad = prioridad.Prioridad == 0 ? "" : prioridad.Prioridad.ToString();
                _cambio.FechaCambio = DateTime.Now.ToString();

                bitacoraWO.ActualizaPriorizacion(_cambio, Convert.ToInt32(prioridad.IDTicketInvgate), out string msg);
            }

            return result;
        }

        [WebMethod]
        public Result OrdenTrabajoAdicionaNotas(Comentario comentario)
        {
            Result result = wo.AdicionaNotas(comentario);

            //Registra bitacora
            if (result.Estatus)
            {
                Entities.Intermedio.AgregaNota _cambio = new Entities.Intermedio.AgregaNota();
                _cambio.TicketIMSS = comentario.IDTicketRemedy;
                _cambio.Notas = comentario.Notas;

                bitacoraWO.AgregaNota(_cambio, Convert.ToInt32(comentario.IDTicketInvgate), out string msg);
            }

            return result;
        }
    }
}
