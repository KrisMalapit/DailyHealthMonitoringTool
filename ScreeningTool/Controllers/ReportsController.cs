using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScreeningTool.Models.View_Model;

namespace ScreeningTool.Controllers
{
    public class ReportsController : Controller
    {
       
        public IActionResult printReport(ReportViewModel rvm)
        {
            rvm.Report = "rptScreeningSummary";
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                byte[] bytes = null;
                string xstring = JsonConvert.SerializeObject(rvm);



                string urilive = "http://californium/ScreeningToolAPI/api/printreport?rvm=";
                string uridev = "http://aluminum/ScreeningToolAPI/api/printreport?rvm=";
                string urilocal = "https://localhost:44301/api/printreport?rvm=";

                response = client.GetAsync(urilive + xstring).Result;
                string byteToString = response.Content.ReadAsStringAsync().Result.Replace("\"", string.Empty);
                bytes = Convert.FromBase64String(byteToString);

                string rpttype = "";
                switch (rvm.rptType)
                {
                    case "PDF":
                        rpttype = "application/pdf";
                        break;
                    case "Excel":
                        rpttype = "application/vnd.ms-excel";
                        break;
                    default:
                        break;
                }


                return File(bytes, rpttype);
            }
            catch (Exception e)
            {

                throw;
            }

        }
    }
}