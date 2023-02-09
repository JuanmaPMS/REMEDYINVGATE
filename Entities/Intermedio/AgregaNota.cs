using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entities.Intermedio
{
    public class AgregaNota
    {
        [XmlElement(IsNullable = true)] 
        public string TicketIMSS { get; set; }
        public string Notas { get; set; }
    }
}
