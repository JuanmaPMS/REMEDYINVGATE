using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entities.Intermedio
{
    public class ActualizaPriorizacionWO
    {
        [XmlElement(IsNullable = true)]
        public string TicketIMSS { get; set; }
        public int Prioridad { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}
