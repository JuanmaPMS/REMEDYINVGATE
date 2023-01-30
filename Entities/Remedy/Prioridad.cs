using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Prioridad
    {
        public string IDTicketRemedy { get; set; }
        public string IDTicketInvgate { get; set; }
        public int Impacto { get; set; }
        public int Urgencia { get; set; }
    }
}
