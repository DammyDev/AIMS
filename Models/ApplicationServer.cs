using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAPI.Data
{
    public class ApplicationServer
    {
        public int ApplicationId { get; set; }
        public int ServerInformationId { get; set; }
        public string FolderPath { get; set; }
        public DateTime DateDeployed { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
