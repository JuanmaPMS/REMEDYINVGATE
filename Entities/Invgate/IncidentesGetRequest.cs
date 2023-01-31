using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class IncidentesGetRequest
    {
        public int id { get; set; }
        public string? date_format { get; set; }
        public bool? comments { get; set; }

    }
}
