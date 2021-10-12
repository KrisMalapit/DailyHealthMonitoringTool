using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScreeningTool.Models.View_Model
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNo { get; set; }
        public string City { get; set; }
        public string Barangay { get; set; }
        public int Age { get; set; }
        public string Status { get; set; } = "Active";
        public int? Vaccinated { get; set; } = 0;
        public int DepartmentId { get; set; }
        public virtual Department Departments { get; set; }
        public string PickUpPoint { get; set; }
        public DateTime FirstDose { get; set; }
        public DateTime SecondDose { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public int Organic { get; set; }
    }
}
