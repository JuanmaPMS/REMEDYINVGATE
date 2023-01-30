using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OrdenTrabajo
    {
        public string IDTicketRemedy { get; set; }
        public string IDTicketInvgate { get; set; }
        public int EstadoNuevo { get; set; }
        public int MotivoEstado { get; set; }
        public string Resolucion { get; set; }
    }
}
