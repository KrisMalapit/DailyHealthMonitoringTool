using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScreeningTool.Models
{
    public class QurantineDetector
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateQuaratineSet { get; set; }
        public string Remarks { get; set; }

    }
}
