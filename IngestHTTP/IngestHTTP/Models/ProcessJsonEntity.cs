using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngestHTTP.Models
{
    public class ProcessJsonEntity
    {
        public string type { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string id { get; set; }
        public string total { get; set; }
    }
}
