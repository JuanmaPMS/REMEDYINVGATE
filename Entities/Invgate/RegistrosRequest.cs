using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Invgate
{
    public class RegistrosRequest
    {
        public int id { get; set; }
        public string tipoControl { get; set; }
        public int idIncidencia { get; set; }
        public int idTipoSolicitud { get; set; }
        public int identificadorNum { get; set; }
        public string identificadorAlfa { get; set; }
        public string fecha { get; set; }
        public bool procesado { get; set; }
    }
}
