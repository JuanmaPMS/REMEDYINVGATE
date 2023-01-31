using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OrdenTrabajoCommentGetRequest
    {
        public int request_id { get; set; }
        public string date_format { get; set; }
        public bool? is_solution { get; set; }
        
    }
}
