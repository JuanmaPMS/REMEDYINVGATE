//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceBitacora
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrdenTrabajoDocumentos
    {
        public int id { get; set; }
        public Nullable<int> OrdenTrabajo_id { get; set; }
        public string Nombre { get; set; }
        public string Tamanio { get; set; }
        public string Contenido { get; set; }
        public string Usuario { get; set; }
        public Nullable<System.DateTime> Inclusion { get; set; }
    
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }
    }
}
