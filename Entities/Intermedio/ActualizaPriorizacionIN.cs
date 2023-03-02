using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entities.Intermedio
{
    public class ActualizaPriorizacionIN
    {
        [XmlElement(IsNullable = false)]
        public string TicketIMSS { get; set; }
        public int Impacto { get; set; }
        public int Urgencia { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}
