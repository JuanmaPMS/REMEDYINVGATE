using TaskIncidencias.WS_Helix;

namespace TaskIncidencias
{
    public class ServiciosHelix
    {
        private readonly MesaHelix mesaHelix = new MesaHelix();

        public ServiciosHelix()
        {
            mesaHelix.Url = Properties.Settings.Default.TaskIncidencias_WS_Helix_MesaHelix;
        }

        public Result IncidenteActualiza(ActualizacionIncidencia incidente)
        {
            return mesaHelix.IncidenteActualiza(incidente);
        }

        public Result IncidenteAdicionaNotas(BitacoraTrabajoIncidencia nota)
        {
            return mesaHelix.IncidenteAdicionaNotas(nota);
        }

        public Result OrdenTrabajoActualiza(ActualizacionOrdenTrabajo ordenTrabajo)
        {
            return mesaHelix.OrdenTrabajoActualiza(ordenTrabajo);
        }

        public Result OrdenTrabajoAdicionaNotas(BitacoraTrabajoOrdenTrabajo nota)
        {
            return mesaHelix.OrdenTrabajoAdicionaNotas(nota);
        }
    }
}
