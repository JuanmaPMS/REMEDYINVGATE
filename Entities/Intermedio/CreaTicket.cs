﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entities.Intermedio
{
    public class CreaTicket
    {
        public string TipoOperacion { get; set; }
        public string NombreProveedor { get; set; }

        [XmlElement(IsNullable = true)] 
        public string TicketIMSS { get; set; }
        public string Descripcion { get; set; }

        [XmlElement(IsNullable = true)]
        public string Resumen { get; set; }
        public string Impacto { get; set; }
        public string Urgencia { get; set; }
        public string Prioridad { get; set; }
        public string TipoIncidencia { get; set; }
        public string FuenteReportada { get; set; }
        public string NombreProducto { get; set; }
        public string GrupoSoporte { get; set; }
        public string CategoriaOpe01 { get; set; }
        public string CategoriaOpe02 { get; set; }
        public string CategoriaOpe03 { get; set; }
        public string CategoriaPro01 { get; set; }
        public string CategoriaPro02 { get; set; }
        public string CategoriaPro03 { get; set; }
        public int EstadoNuevo { get; set; }
        public string Notas { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Adjunto01 { get; set; }
        public string AdjuntoName01 { get; set; }
        public string AdjuntoSize01 { get; set; }
        public string Adjunto02 { get; set; }
        public string AdjuntoName02 { get; set; }
        public string AdjuntoSize02 { get; set; }
        public string Adjunto03 { get; set; }
        public string AdjuntoName03 { get; set; }
        public string AdjuntoSize03 { get; set; }
        public string Cliente { get; set; }
        public string VIP { get; set; }
        public string Sensibilidad { get; set; }

    }
}
