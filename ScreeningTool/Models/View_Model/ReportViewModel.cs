using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScreeningTool.Models.View_Model
{
    public class ReportViewModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int[] LevelId { get; set; }
        public string TypeId { get; set; }
        public string Report { get; set; }
        public string rptType { get; set; }
    }
    public class SMSArray
    {
        public string[] PhoneNumbers { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}
