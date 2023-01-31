using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class IncidentesCommentGetResponse
    {
        public int id { get; set; }
        public int incident_id { get; set; }
        public int author_id { get; set; }
        public string message { get; set; }
        public string created_at { get; set; }
        public bool? customer_visible { get; set; }
        public int? reference { get; set; }
        public int? msg_num { get; set; }
        public bool? is_solution { get; set; }
        public List<object>? attachments { get; set; }
    }
}
