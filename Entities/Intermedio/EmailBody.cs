using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entities.Intermedio
{
    public class EmailBody
    {
        public List<string> Destinatarios { get; set; }
        public List<string> DestinatariosCC { get; set; }
        public string Prioridad { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string TicketIMSS { get; set; }
        public string Requerimiento { get; set; }
        public string Aplicativo { get; set; }
    }
}
