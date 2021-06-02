using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScreeningTool.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string DepartmentHeads { get; set; }

        public virtual Company Companies { get; set; }

        public string Status { get; set; } = "Active";

    }
}
