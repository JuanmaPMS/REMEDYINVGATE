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
    
    public partial class IncidentesDocumentos
    {
        public int id { get; set; }
        public Nullable<int> Incidente_id { get; set; }
        public string Nombre { get; set; }
        public string Tamanio { get; set; }
        public string Contenido { get; set; }
        public string Usuario { get; set; }
        public Nullable<System.DateTime> Inclusion { get; set; }
    
        public virtual Incidente Incidente { get; set; }
    }
}
