using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Invgate
{
    public class AttachmentResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string attach { get; set; }
        public string name { get; set; }
        public int size { get; set; }
    }
}
