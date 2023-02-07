//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceBitacora
{
    using System;
    using System.Collections.Generic;
    
    public partial class Incidente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Incidente()
        {
            this.IncidenteNotas = new HashSet<IncidenteNotas>();
            this.IncidentesDocumentos = new HashSet<IncidentesDocumentos>();
        }
    
        public int id { get; set; }
        public string TicketRemedy { get; set; }
        public Nullable<int> TicketInvgate { get; set; }
        public string Descripcion { get; set; }
        public string Resumen { get; set; }
        public string Impacto { get; set; }
        public string Urgencia { get; set; }
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
        public string Estado { get; set; }
        public string Motivo { get; set; }
        public string FechaCreacion { get; set; }
        public string Cliente { get; set; }
        public Nullable<bool> VIP { get; set; }
        public string Sensibilidad { get; set; }
        public string Usuario { get; set; }
        public Nullable<System.DateTime> Inclusion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncidenteNotas> IncidenteNotas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncidentesDocumentos> IncidentesDocumentos { get; set; }
    }
}