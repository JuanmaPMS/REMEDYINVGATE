using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Intermedio
{
    public class ActualizaTicket
    {
        public string TicketIMSS { get; set; }
        public string EstadoNuevo { get; set; }
        public string Motivo { get; set; }
        public DateTime FechaCambio { get; set; }
        public string Notas { get; set; }
        
    }
}
