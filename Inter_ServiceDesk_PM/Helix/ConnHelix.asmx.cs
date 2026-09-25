using System;
using System.Web.Services;
using Entities.Helix;
using Entities.Intermedio;
using ServiceBitacora;
using ServiceIMSS.Helix;

namespace Inter_ServiceDesk_PM.Helix
{
    [WebService(Namespace = "urn:imss:helix-invgate")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class MesaHelix : WebService
    {
        private readonly IncidentesHelix incidentes = new IncidentesHelix();
        private readonly OrdenesTrabajoHelix ordenesTrabajo = new OrdenesTrabajoHelix();
        private readonly IncidenteData bitacoraIncidentes = new IncidenteData();
        private readonly OrdenTrabajoData bitacoraOrdenesTrabajo = new OrdenTrabajoData();

        [WebMethod]
        public Entities.Result IncidenteActualiza(ActualizacionIncidencia incidente)
        {
            Entities.Result result = incidentes.ActualizaIncidente(incidente);

            if (result.Estatus)
            {
                ActualizaTicketIN cambio = new ActualizaTicketIN
                {
                    TicketIMSS = incidente.IdIncidencia,
                    EstadoNuevo = incidente.EstadoNuevo,
                    Motivo = incidente.MotivoEstadoNuevo,
                    FechaCambio = DateTime.Now
                };

                bitacoraIncidentes.ActualizaIncidente(cambio, Convert.ToInt32(incidente.TicketProveedor), out string mensaje);
            }

            return result;
        }

        [WebMethod]
        public Entities.Result IncidenteAdicionaNotas(BitacoraTrabajoIncidencia nota)
        {
            Entities.Result result = incidentes.AdicionaBitacora(nota);

            if (result.Estatus)
            {
                AgregaNota cambio = new AgregaNota
                {
                    TicketIMSS = nota.IdIncidencia,
                    Notas = nota.Notas
                };

                bitacoraIncidentes.AgregaNota(cambio, Convert.ToInt32(nota.TicketProveedor), out string mensaje);
            }

            return result;
        }

        [WebMethod]
        public Entities.Result OrdenTrabajoActualiza(ActualizacionOrdenTrabajo ordenTrabajo)
        {
            Entities.Result result = ordenesTrabajo.ActualizaOrdenTrabajo(ordenTrabajo);

            if (result.Estatus)
            {
                ActualizaTicketWO cambio = new ActualizaTicketWO
                {
                    TicketIMSS = ordenTrabajo.IdOrdenTrabajo,
                    EstadoNuevo = ordenTrabajo.EstadoNuevo,
                    Motivo = ordenTrabajo.MotivoEstadoNuevo,
                    FechaCambio = DateTime.Now
                };

                bitacoraOrdenesTrabajo.ActualizaOrdenTrabajo(cambio, Convert.ToInt32(ordenTrabajo.TicketProveedor), out string mensaje);
            }

            return result;
        }

        [WebMethod]
        public Entities.Result OrdenTrabajoAdicionaNotas(BitacoraTrabajoOrdenTrabajo nota)
        {
            Entities.Result result = ordenesTrabajo.AdicionaBitacora(nota);

            if (result.Estatus)
            {
                AgregaNota cambio = new AgregaNota
                {
                    TicketIMSS = nota.IdOrdenTrabajo,
                    Notas = nota.Notas
                };

                bitacoraOrdenesTrabajo.AgregaNota(cambio, Convert.ToInt32(nota.TicketProveedor), out string mensaje);
            }

            return result;
        }
    }
}
