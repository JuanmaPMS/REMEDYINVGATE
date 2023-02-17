using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIncidencias.WS_Remedy;


namespace TaskIncidencias
{
    public class ServiciosImss
    {
        MesaIMSS mesaIMSS = new MesaIMSS();

        public Result IncidenteActualiza(Incidente _incidente)
        {  
            return mesaIMSS.IncidenteActualiza(_incidente);
        }

        public Result IncidenteActualizaCategorizacion(Categorizacion _categorizacion)
        {
            return mesaIMSS.IncidenteActualizaCategorizacion(_categorizacion);
        }

        public Result IncidenteActualizaPrioridad(Prioridad _prioridad)
        {
            return mesaIMSS.IncidenteActualizaPrioridad(_prioridad);
        }

        public Result IncidenteAdicionaNotas(Comentario _notas)
        {
            return mesaIMSS.IncidenteAdicionaNotas(_notas);
        }

        public Result OrdenTrabajoActualiza(OrdenTrabajo _wo)
        {
            return mesaIMSS.OrdenTrabajoActualiza(_wo);
        }

        public Result OrdenTrabajoActualizaCategorizacion(Categorizacion _categorizacion)
        {
            return mesaIMSS.OrdenTrabajoActualizaCategorizacion(_categorizacion);
        }

        public Result OrdenTrabajoActualizaPrioridad(PrioridadOT _prioridad)
        {
            return mesaIMSS.OrdenTrabajoActualizaPrioridad(_prioridad);
        }

        public Result OrdenTrabajoAdicionaNotas(Comentario _notas)
        {
            return mesaIMSS.OrdenTrabajoAdicionaNotas(_notas);
        }


    }
}
