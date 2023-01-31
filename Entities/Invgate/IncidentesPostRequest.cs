using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class IncidentesPostRequest
    {
        public int creator_id { get; set; }
        public int customer_id { get; set; }
        public int category_id { get; set; }
        public int priority_id { get; set; }
        public int type_id { get; set; }
        public int? source_id { get; set; }
        public string title { get; set; }
        public string? description { get; set; }
        public string? date { get; set; }
        public List<object>? attachments { get; set; }
        public List<object>? related_to { get; set; }
    }
}
