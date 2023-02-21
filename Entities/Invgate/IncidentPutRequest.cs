using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Invgate
{
    public class IncidentPutRequest
    {
        public int id { get; set; }
        public string? title { get; set; }
        public int? categoryId { get; set; }
        public int? typeId { get; set; }
        public string? description { get; set; }
        public int? priorityId { get; set; }
        public int? userId { get; set; }
        public int? creatorId { get; set; }
        public int? assignedId { get; set; }
        public int? assignedGroupId { get; set; }
        public int? dateOcurred { get; set; }
        public int? sourceId { get; set; }
        public int? statusId { get; set; }
        public int? createdAt { get; set; }
        public int? lastUpdate { get; set; }
        public int? processId { get; set; }
        public int? solvedAt { get; set; }
        public int? closedAt { get; set; }
        public int? closedReason { get; set; }
        public bool? dataCleaned { get; set; }
        public int? locationId { get; set; }

    }
}
