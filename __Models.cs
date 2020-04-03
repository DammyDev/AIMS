using Newtonsoft.Json;
using ProjectAPI.Enums;
using System;

namespace ProjectAPI.Data
{
    public class Solution_
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Enabled";
    }
    public class Solution: Solution_
    {
        [JsonConverter(typeof(Base64FileJsonConverter))]
        public byte[] ProjectDocument { get; set; }
    }

    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public string FullPath { get; set; }
        public DateTime DateDeployed { get; set; } 
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Enabled";
    }

    public class Database
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Engine { get; set; }
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Enabled";
    }

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

    #region JoiningTables
        public class SolutionApplication
        {
            public int SolutionId { get; set; }
            public int ApplicationId { get; set; }
            public bool IsInternal { get; set; } = true;
            public DateTime DateCreated { get; set; } = DateTime.Now;
        }

        public class SolutionDatabase
        {
            public int SolutionId { get; set; }
            public int DatabaseId { get; set; }
            public DateTime DateCreated { get; set; } = DateTime.Now;
        }

        public class ApplicationServer
        {
            public int ApplicationId { get; set; }
            public int ServerInformationId { get; set; }
            public DateTime DateCreated { get; set; } = DateTime.Now;
        }
    #endregion
}
