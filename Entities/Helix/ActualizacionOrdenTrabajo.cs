namespace Entities.Helix
{
    public class ActualizacionOrdenTrabajo
    {
        public string TipoOperacion { get; set; }
        public string TipoTicket { get; set; }
        public string IdOrdenTrabajo { get; set; }
        public string TicketProveedor { get; set; }
        public string EstadoNuevo { get; set; }
        public string MotivoEstadoNuevo { get; set; }
        public string NotaResolucion { get; set; }
    }
}
