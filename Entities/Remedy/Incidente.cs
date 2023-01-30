using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Incidente
    {
        public string IDTicketRemedy { get; set; }
        public string IDTicketInvgate { get; set; }
        public string CatCierreOperacion01 { get; set; }
        public string CatCierreOperacion02 { get; set; }
        public string CatCierreOperacion03 { get; set; }
        public string CatCierreProducto01 { get; set; }
        public string CatCierreProducto02 { get; set; }
        public string CatCierreProducto03 { get; set; }
        public int EstadoNuevo { get; set; }
        public int MotivoEstado { get; set; }
        public string Resolucion { get; set; }
       

    }
}
