using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScreeningTool.Models.View_Model
{
    public class NotificationViewModel
    {
        public string EmployeeName { get; set; }
        public int TotalScore { get; set; }
        public string Remarks { get; set; }
        public string Encoder { get; set; }
        public int ScreenLogId { get; set; }
        public int DepartmentId { get; set; }
        public string QRKey { get; set; }
        public string EmployeeNo { get; set; }
        public DateTime ScreenDateTime { get; set; }
    }
}
