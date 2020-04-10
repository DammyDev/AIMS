using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIMS.Data
{
    public class SolutionDatabase
    {
        public int SolutionId { get; set; }
        public int DatabaseId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
