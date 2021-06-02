using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ScreeningTool.Models
{
    public class ScreenLogs
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string ContactPerson { get; set; }
        public string City { get; set; }
        public string Barangay { get; set; }
        public decimal Temperature { get; set; }
        public int Age { get; set; }
        public int Q1 { get; set; }
        public int Q2 { get; set; }
        public int Q3 { get; set; }
        public int Q4 { get; set; }
        public int Q5 { get; set; }
        public int Q6 { get; set; }
        public int Q7 { get; set; }
        public int Q8 { get; set; }
        public int Q9 { get; set; }
        public int Q10 { get; set; }
        public int Q11 { get; set; }
        public int Q12 { get; set; }
        [DefaultValue(0)]
        public int Q13 { get; set; }
        [DefaultValue(0)]
        public int Q14 { get; set; }
        [DefaultValue(0)]
        public int Q15 { get; set; }
        public int Q16 { get; set; }
        public int Q17 { get; set; }
        public int Q18 { get; set; }
        public int Q19 { get; set; }
        public int Q20 { get; set; }


        public string RemarksQ1 { get; set; }
        public string RemarksQ2 { get; set; }
        public string RemarksQ3 { get; set; }
        public string RemarksQ4 { get; set; }
        public string RemarksQ5 { get; set; }
        public string RemarksQ6 { get; set; }
        public string RemarksQ7 { get; set; }
        public string RemarksQ8 { get; set; }
        public string RemarksQ9 { get; set; }
        public string RemarksQ10 { get; set; }
        public string RemarksQ11 { get; set; }


        public string QRKey { get; set; } 
        public string EntryStatus { get; set; }



        public string Remarks { get; set; }
        public int Result { get; set; }

        public string WorkPlace { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now.Date;
    }
}
