using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Invgate
{
    public class IncidenteColaborador
    {
        public int? IncidentId { get; set; }
        public int? UserId { get; set; }
        public int Type { get; set; }
    }
}
