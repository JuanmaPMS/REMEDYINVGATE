using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class IncidentesCommentPostRequest
    {
        public int request_id { get; set; }
        public string comment { get; set; }
        public int author_id { get; set; }
        public bool? is_solution { get; set; }
        public bool? customer_visible { get; set; }
        public List<object>? attachments { get; set; }
        
    }
}
