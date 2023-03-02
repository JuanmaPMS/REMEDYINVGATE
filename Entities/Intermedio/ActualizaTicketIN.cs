using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entities.Intermedio
{
    public class ActualizaTicketIN
    {
        [XmlElement(IsNullable = true)]
        public string TicketIMSS { get; set; }

        [XmlElement(IsNullable = true)] 
        public int? Impacto { get; set; }

        [XmlElement(IsNullable = true)] 
        public int? Urgencia { get; set; }
        public string EstadoNuevo { get; set; }
        public string Motivo { get; set; }
        public DateTime FechaCambio { get; set; }
        public string Notas { get; set; }
        
    }
}
