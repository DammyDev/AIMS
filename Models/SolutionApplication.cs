using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIMS.Data
{
    public class SolutionApplication
    {
        public int SolutionId { get; set; }
        public int ApplicationId { get; set; }
        public bool IsInternal { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
