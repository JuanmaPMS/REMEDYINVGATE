using System;
using System.Collections.Generic;
using System.ComponentModel;
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



        public byte[] Adjunto01 = new byte[0];

        [XmlElement(IsNullable = true)]
        public string AdjuntoName01 { get; set; }



        [DefaultValueAttribute(0)]
        public int AdjuntoSize01 = 0;

      
        public byte[] Adjunto02 = new byte[0];

        [XmlElement(IsNullable = true)]
        public string AdjuntoName02 { get; set; }



        [DefaultValueAttribute(0)]

        public int AdjuntoSize02 = 0;


        public byte[] Adjunto03 = new byte[0];

        [XmlElement(IsNullable = true)]
        public string AdjuntoName03 { get; set; }


        [DefaultValueAttribute(0)]

        public int AdjuntoSize03 = 0;

        //public AgregaNota()
        //{
        //    this.Adjunto01 = new byte[0];
        //    this.Adjunto02 = new byte[0];
        //    this.Adjunto03 = new byte[0];
        //    this.AdjuntoSize01 = 0;
        //    this.AdjuntoSize02 = 0;
        //    this.AdjuntoSize03 = 0;
        //    this.AdjuntoName01 = String.Empty;
        //    this.AdjuntoName02 = String.Empty;
        //    this.AdjuntoName03 = String.Empty;


        //}
    }
}
