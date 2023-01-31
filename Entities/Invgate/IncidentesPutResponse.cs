using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class IncidentesPutResponse
    {
        public int? id { get; set; }
        public string? title { get; set; }
        public int? category_id { get; set; }
        public string? description { get; set; }
        public int? priority_id { get; set; }
        public int? user_id { get; set; }
        public int? creator_id { get; set; }
        public int? assigned_id { get; set; }
        public int? assigned_group_id { get; set; }
        public string? date_ocurred { get; set; }
        public int? source_id { get; set; }
        public int? status_id { get; set; }
        public int? type_id { get; set; }
        public int? location_id { get; set; }
        public string? created_at { get; set; }
        public string? last_update { get; set; }
        public int? process_id { get; set; }
        public string? solved_at { get; set; }
        public string? closed_at { get; set; }
        public string? closed_reason { get; set; }
        public List<object>? attachments { get; set; }
        public List<object>? custom_fields { get; set; }
    }
}
