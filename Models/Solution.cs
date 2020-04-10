using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AIMS.Data
{
    public class Solution_
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Enabled";
    }
    public class Solution : Solution_
    {
        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] ProjectDocument { get; set; }
    }
}
