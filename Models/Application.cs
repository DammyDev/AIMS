using System;

namespace ProjectAPI.Data
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Enabled";
    }
}
