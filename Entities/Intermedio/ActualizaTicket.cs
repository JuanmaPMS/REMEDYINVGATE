using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entities.Intermedio
{
    public class ActualizaTicket
    {
        [XmlElement(IsNullable = true)]
        public string TicketIMSS { get; set; }
        public string Impacto { get; set; }
        public string Urgencia { get; set; }
        public string Prioridad { get; set; }
        public string EstadoNuevo { get; set; }
        public string Motivo { get; set; }
        public string FechaCambio { get; set; }
        public string Notas { get; set; }
        
    }
}
