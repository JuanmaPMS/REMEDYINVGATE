using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Intermedio
{
    public class Ticket
    {
        public int id { get; set; }
        public string TicketRemedy { get; set; }
        public int TicketInvgate { get; set; }
        public string Descripcion { get; set; }
        public string Resumen { get; set; }
        public string IdImpacto { get; set; }
        public string Impacto { get; set; }
        public string IdUrgencia { get; set; }
        public string Urgencia { get; set; }
        public string IdPrioridad { get; set; }
        public string Prioridad { get; set; }
        public string IdTipoIncidencia { get; set; }
        public string TipoIncidencia { get; set; }
        public string IdFuenteReportada { get; set; }
        public string FuenteReportada { get; set; }
        public string GrupoSoporte { get; set; }
        public string CategoriaOpe01 { get; set; }
        public string CategoriaOpe02 { get; set; }
        public string CategoriaOpe03 { get; set; }
        public string CategoriaPro01 { get; set; }
        public string CategoriaPro02 { get; set; }
        public string CategoriaPro03 { get; set; }
        public string NombreProducto { get; set; }
        public string IdEstado { get; set; }
        public string Estado { get; set; }
        public string IdMotivo { get; set; }
        public string Motivo { get; set; }
        public string FechaCreacion { get; set; }
        public string Cliente { get; set; }
        public bool VIP { get; set; }
        public string Sensibilidad { get; set; }
        public string Usuario { get; set; }
        public DateTime Inclusion { get; set; }
        public string Tipo { get; set; }

    }
}
