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
        [XmlElement(IsNullable = false)] 
        public string TicketIMSS { get; set; }
        public string Notas { get; set; }

        [XmlElement(IsNullable = true)]
        public byte[] Adjunto01 { get; set; }

        [XmlElement(IsNullable = true)]
        public string AdjuntoName01 { get; set; }

        [XmlElement(IsNullable = true)]
        public int? AdjuntoSize01 { get; set; }

        [XmlElement(IsNullable = true)]
        public byte[] Adjunto02 { get; set; }

        [XmlElement(IsNullable = true)]
        public string AdjuntoName02 { get; set; }

        [XmlElement(IsNullable = true)]
        public int? AdjuntoSize02 { get; set; }

        [XmlElement(IsNullable = true)]
        public byte[] Adjunto03 { get; set; }

        [XmlElement(IsNullable = true)]
        public string AdjuntoName03 { get; set; }

        [XmlElement(IsNullable = true)]
        public int? AdjuntoSize03 { get; set; }
    }
}
