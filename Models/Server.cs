using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIMS.Data
{
    public class Server
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string IPAddress { get; set; }
        public string Type { get; set; }
        public string OperatingSystem { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Enabled";
    }
}
