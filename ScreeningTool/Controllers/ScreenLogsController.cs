using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScreeningTool.Models;

namespace ScreeningTool.Controllers
{
    [Authorize]
    public class ScreenLogsController : Controller
    {
        private readonly ScreeningToolContext _context;

        public ScreenLogsController(ScreeningToolContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            return View();
        }
        public IActionResult getData(string type)
        {
            DateTime dt = new DateTime(1, 1, 1);
            string status = "";
            var v =

                _context.ScreenLogs.Select(a => new {

                    a.EmployeeId,
                   
                    EmployeeName = a.LastName + ", " + a.FirstName,
                   
                    a.CreatedAt
                      ,
                   a.Temperature,

                   a.Result,
                   Category = a.Result == 0 ? "No Risk" : (a.Result == 1 ? "Low Risk" : (a.Result <= 3 ? "Moderate Risk" : "High Risk" ))


                });

          
            status = "success";


            var model = new
            {
                status
                ,
                data = v.ToList(),
           
               
            };








            return Json(model);
        }
    }
}