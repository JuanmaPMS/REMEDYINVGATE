using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entities.Intermedio
{
    public class ActualizaCategorizacion
    {
        [XmlElement(IsNullable = true)]
        public string TicketIMSS { get; set; }

        [XmlElement(IsNullable = true)]
        public string CategoriaOpe01 { get; set; }

        [XmlElement(IsNullable = true)]
        public string CategoriaOpe02 { get; set; }

        [XmlElement(IsNullable = true)]
        public string CategoriaOpe03 { get; set; }

        [XmlElement(IsNullable = true)]
        public string CategoriaPro01 { get; set; }

        [XmlElement(IsNullable = true)]
        public string CategoriaPro02 { get; set; }

        [XmlElement(IsNullable = true)]
        public string CategoriaPro03 { get; set; }
        public string GrupoSoporte { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}
