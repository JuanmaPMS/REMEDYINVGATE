using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Intermedio
{
    public partial class AttachedFile
    {
        public int Id { get; set; }

        public string? UniqueKey { get; set; }

        public int? IncidentId { get; set; }

        public int? ReplyId { get; set; }

        public int? UserId { get; set; }

        public string? Path { get; set; }

        public string? Name { get; set; }

        public string? Mimetype { get; set; }

        public int? Size { get; set; }

        public string? Hash { get; set; }

        public string? Extension { get; set; }

        public int? KbId { get; set; }

        public bool? Hidden { get; set; }

        public int? InstanceId { get; set; }

        public int? CommentId { get; set; }
    }

}
