using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Intermedio
{
    public class ActualizaPriorizacion
    {
        public string TicketIMSS { get; set; }
        public string Impacto { get; set; }
        public string Urgencia { get; set; }
        public string Prioridad { get; set; }
        public string FechaCambio { get; set; }
    }
}
