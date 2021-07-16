using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ScreeningTool.Models;
using ScreeningTool.Models.View_Model;

//using ScreeningTool.Models.View_Model;

namespace ScreeningTool.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private string[] phonenumber;
        private string[] emailaddress;



        private readonly ScreeningToolContext _context;

        public HomeController(ScreeningToolContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var levelList = _context.Categories.Select(b => new
            {
                b.Id,
                b.Name,
            });
            ViewData["Levels"] = new SelectList(levelList.OrderBy(a => a.Name), "Id", "Name");
            return View();



        }
        public IActionResult Index2()
        {

            return View();



        }
        public IActionResult SaveData(ScreenLogs sclogs, string Encoder)
        {
            sclogs.Id = 0;
            string status = "";
            string message = "";
            string empname = sclogs.LastName + ", " + sclogs.FirstName;
            int totalScore = 0;
            string remarks = "";
            string empno = "";
            int deptId = 0;
            

            try
            {

                if (Encoder == "employee")
                {
                    deptId = _context.Employees.Where(a => a.EmployeeId == sclogs.EmployeeId).FirstOrDefault().DepartmentId;

                }
                else
                {
                    sclogs.EmployeeId = "";
                }

                totalScore = GetTotalScore(sclogs);
                remarks = ScoreRemarks(sclogs);

                Guid qr = Guid.NewGuid();
                sclogs.Id = 0;
                sclogs.CreatedAt = DateTime.Now;
                sclogs.Result = totalScore;
                sclogs.EntryStatus = "Pending";
                sclogs.QRKey = qr.ToString();

                _context.ScreenLogs.Add(sclogs);
                _context.SaveChanges();



                if (totalScore > 1) //save in qurantine table
                {
                    SaveQuarantine(sclogs.EmployeeId, sclogs.Id);
                }


                Logs log = new Logs();
                log.Action = "Add";
                log.Description = "Add new Screen Log. ID : " + sclogs.Id;
                log.Status = "Success";
                log.CreatedDate = DateTime.Now;
                _context.Logs.Add(log);
                _context.SaveChanges();

                status = "success";
                message = "";


                

            }
            catch (Exception e)
            {
                status = "fail";
                message = e.InnerException.Message.ToString();

            }
            var model = new
            {
                status,
                message,
                empname,
                remarks,
                totalScore,
                deptId,
                Encoder,
                ScreenLogId = sclogs.Id,
                employeeNo = sclogs.EmployeeId,
                sclogs.QRKey,
                ScreenDateTime = DateTime.Now
            };

            return Json(model);


        }


        private int QuaratineCounter(string EmployeeId)
        {
            int cnt = 0;
            var data = _context.QurantineDetectors.Where(a => a.EmployeeId == EmployeeId).FirstOrDefault();
            if (data != null)
            {
                var dq = data.DateQuaratineSet;
                cnt = Convert.ToInt32((DateTime.Now.Date - dq).TotalDays);
                cnt = 14 - cnt; // 14 days
            }
            else
            {
                cnt = -1;
            }
            return cnt;
        }
        private void SaveQuarantine(string EmployeeId, int ScreenLogId)
        {
            var data = _context.QurantineDetectors.Where(a => a.EmployeeId == EmployeeId).FirstOrDefault();
            if (data != null)
            {
               
                data.DateQuaratineSet = DateTime.Now.Date;
                _context.Entry(data).State = EntityState.Modified;
                _context.SaveChanges();
                Logs log = new Logs
                {
                    Action = "Modify",
                    Description = "Modified Quarantine.  ScreenLog ID  : " + ScreenLogId,
                    Status = "Success",
                    CreatedDate = DateTime.Now
                };
                _context.Logs.Add(log);
                _context.SaveChanges();

            }
            else
            {
                QurantineDetector qd = new QurantineDetector
                {
                    EmployeeId = EmployeeId,
                    DateQuaratineSet = DateTime.Now.Date
                };
                _context.Add(qd);
                _context.SaveChanges();

                Logs log = new Logs
                {
                    Action = "Add",
                    Description = "Add new Quarantine. ScreenLog ID : " + ScreenLogId,
                    Status = "Success",
                    CreatedDate = DateTime.Now
                };
                _context.Logs.Add(log);
                _context.SaveChanges();


            }


           


        }


        [HttpPost]
        public ActionResult SendFinalNotification(NotificationViewModel nvm)
        {
            string status = "";
            string message = "";
            nvm.Encoder = "employee";


            try
            {
                if (nvm.TotalScore >= 2)
                {
                    if (nvm.Encoder == "employee")
                    {
                        try
                        {

                            status = "success";
                            status = SendNotification(nvm.EmployeeName, nvm.Remarks, nvm.TotalScore, nvm.DepartmentId, nvm.ScreenLogId);
                            message = "Submission success";
                        }
                        catch (Exception e)
                        {

                            status = "Fail from encoder Employee";
                            message = e.InnerException.Message.ToString();
                        }

                    }
                    else
                    {
                        try
                        {
                            status = SendEmail("COVID-19 DAILY HEALTH MONITORING TOOL Infoblast <br /><br />" + nvm.EmployeeName + " has a total score of " + nvm.TotalScore
                               + "<br /> Remarks: <br />" + nvm.Remarks + ". <br /><br />", nvm.ScreenLogId, "Non-Employee");


                            phonenumber = new string[7];
                            phonenumber[0] = "09957566792"; // madz
                            phonenumber[1] = "09197989212"; // charizza
                            phonenumber[2] = "09985974855"; // jonathan
                            phonenumber[3] = "09257061809"; // fides quintos
                            phonenumber[4] = "09173121719"; // joyce lagarde
                            phonenumber[5] = "09953668620"; // raph
                            phonenumber[6] = "09998876595"; // jojo tandoc
                           

                            status = SendSMS("COVID-19 DAILY HEALTH MONITORING TOOL Infoblast" + Environment.NewLine + Environment.NewLine + nvm.EmployeeName + " has a total score of " + nvm.TotalScore
                                                                  + Environment.NewLine + "Remarks:" + Environment.NewLine + nvm.Remarks, nvm.ScreenLogId);
                        }
                        catch (Exception e)
                        {

                            status = "Fail from encoder Non-Employee";
                            message = e.InnerException.Message.ToString();
                        }



                    }

                }
                else
                {
                    message = "Submission success";
                    status = "success";
                }

            }
            catch (Exception e)
            {
                status = "fail";
                message = e.InnerException.Message.ToString();

            }


            Logs log = new Logs();
            log.Action = "From Submission";
            log.Description = message;
            log.Status = status;
            _context.Logs.Add(log);
            _context.SaveChanges();



            var model = new
            {
                status,
                message
            };
            return Json(model);
        }
        public int GetTotalScore(ScreenLogs ts)
        {


            int q1 = ts.Q1 == 1 ? 1 : 0;
            int q2 = ts.Q2 == 1 ? 2 : 0;
            int q3 = ts.Q3 == 1 ? 2 : 0;
            int q4 = ts.Q4 == 1 ? 2 : 0;
            int q5 = ts.Q5 == 1 ? 2 : 0;
            int q6 = ts.Q6 == 1 ? 2 : 0;
            int q7 = ts.Q7 == 1 ? 2 : 0;
            int q8 = ts.Q8 == 1 ? 2 : 0;
            int q9 = ts.Q9 == 1 ? 2 : 0;
            int q10 = ts.Q10 == 1 ? 4 : 0;
            int q11 = ts.Q11 == 1 ? 4 : 0;
            int q12 = ts.Q12 == 1 ? 2 : 0;
            int q13 = ts.Q13 == 1 ? 2 : 0;
            int q14 = ts.Q14 == 1 ? 2 : 0;
            int q15 = ts.Q15 == 1 ? 2 : 0;

            int q16 = ts.Q16 == 1 ? 2 : 0;
            int q17 = ts.Q17 == 1 ? 1 : 0;
            int q18 = ts.Q18 == 1 ? 1 : 0;
            int q19 = ts.Q19 == 1 ? 2 : 0;
            int q20 = ts.Q20 == 1 ? 2 : 0;

            int res = q1 + q2 + q3 + q4 + q5 + q6 + q7 + q8 + q9 + q10 + q11 + q12 + q13 + q14 + q15 + q16 + q17 + q18 + q19 + q20;
            var temp = Convert.ToDouble(ts.Temperature);
            if (temp > 37.5)
            {
                res += 2;
            }
           
            return res;
        }
        public string ScoreRemarks(ScreenLogs ts)
        {

            string rem = "";
            var temp = Convert.ToDouble(ts.Temperature);

            rem = ts.Q1 == 1 ? "SC-P-HW-MC," : "";
            rem += ts.Q2 == 1 ? "COUGH," : "";
            rem += ts.Q3 == 1 ? "COLDS," : "";
            rem += ts.Q4 == 1 ? "DIARRHEA," : "";
            rem += ts.Q5 == 1 ? "SORE THROAT," : "";
            rem += ts.Q6 == 1 ? "BODY WEAKNESS," : "";
            rem += ts.Q7 == 1 ? "HEADACHE," : "";
            rem += ts.Q8 == 1 ? "DIFFICULTY OF BREATHING," : "";
            rem += ts.Q9 == 1 ? "EASY FATIGUABILITY," : "";
            rem += ts.Q10 == 1 ? "LIVING IN A Household WITH A PUI or CONFIRMED COVID-19 Case," : "";
            rem += ts.Q11 == 1 ? "DIRECT-CONTACT," : "";
            rem += ts.Q12 == 1 ? "LIVING IN A Brgy-Comp-St-Condo WITH A PUI or CONFIRMED COVID-19 Case EXPERIENCING ANY OF THE SYMPTOMS," : "";
            rem += ts.Q13 == 1 ? "SICK for a day or two," : "";
            rem += ts.Q14 == 1 ? "LOST sense of taste and smell," : "";
            rem += ts.Q15 == 1 ? "Have experienced ANY 2 OF THE SYMPTOMS mentioned ABOVE," : "";

            rem += ts.Q16 == 1 ? "Notified to undergo quarantine period," : "";
            rem += ts.Q17 == 1 ? "LIVING IN A Brgy-Comp-St-Condo WITH A PUI or CONFIRMED COVID-19 Case w-o ANY OF THE SYMPTOMS," : "";
            rem += ts.Q18 == 1 ? "Visit the hospital/clinic in the last 14 days," : "";
            rem += ts.Q19 == 1 ? "Consulted/Seen a doctor in the last 14 days," : "";
            rem += ts.Q20 == 1 ? "FEVER," : "";

            rem += temp > 37.5 ? "HIGH TEMPERATURE," : ""; ;


            

            if (rem.Trim() != "")
            {
                rem = rem.Remove(rem.Length - 1);
            }


            return rem;
        }
        public JsonResult GetEmployeeDetails(string EmpId)
        {

            string empfname = "";
            string emplastname = "";
            string hasValue = "";
            string contactno = "";
            string city = "";
            int age = 0;
            try
            {
                var emp = _context.Employees.Where(a => a.EmployeeId == EmpId);
                if (emp.Count() > 0)
                {
                    empfname = emp.FirstOrDefault().FirstName;
                    emplastname = emp.FirstOrDefault().LastName;
                    contactno = emp.FirstOrDefault().ContactNo;
                    city = emp.FirstOrDefault().City;
                    age = emp.FirstOrDefault().Age;


                    hasValue = "true";
                }
                else
                {
                    hasValue = "false";
                }



            }
            catch (Exception e)
            {
                throw;
            }
            var result = new
            {
                hasValue,
                empfname,
                emplastname,
                contactno,
                city,
                age
            };


            return Json(result);
        }
        public JsonResult UpdateQuarantineStatus(string EmployeeId,string Remarks)
        {
            string status = "";
            string message = "";

            try
            {
                var emp = _context.QurantineDetectors.Where(a => a.EmployeeId == EmployeeId).ToList();

                if (emp.Count() == 0 || emp == null)
                {
                    status = "fail";
                    message = "No record found in Quarantine Table for EmployeeId " + EmployeeId;

                }
                else
                {
                    emp.ForEach(a => {
                        a.DateQuaratineSet = new DateTime(1900, 1, 1);
                        a.Remarks = Remarks;
                    } );

                
                    message = "Modify Quarantine. EmployeeId : " + EmployeeId;
                    status = "success";
                }

                Logs log = new Logs();
                log.Action = "Modify";
                log.Description = message;
                log.Status = status;
                _context.Logs.Add(log);
                _context.SaveChanges();


            }
            catch (Exception e)
            {
                message = e.Message;
                status = "fail";
            }
         
            var model = new
            {
              
                status,
                message
            };


            return Json(model);

        }

            public JsonResult UpdateEmpDetails(Employee emp)
        {
            string status = "";
            string message = "";
            int age = 0;

            try
            {
                var today = DateTime.Today;
                var _empage = today.Year - emp.Birthday.Year;
                if (emp.Birthday.Date > today.AddYears(-_empage)) _empage--;

                age = _empage;
                emp.Age = _empage;
                _context.Update(emp);
                _context.SaveChanges();
                status = "success";

                Logs log = new Logs();
                log.Action = "Modify";
                log.Description = "Modify employee details from Home Controller. Employee Id : " + emp.Id;
                log.Status = status;
                _context.Logs.Add(log);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                status = "fail";
                message = e.InnerException.Message.ToString();
                throw;
            }

            var result = new
            {
                age,
                status,
                message
            };


            return Json(result);
        }
        public JsonResult GetEmpDetails(string EmpId)
        {

            string empfname = "";
            string emplastname = "";
            int age = 0;
            string contactno = "";
            string contactperson = "";
            string contactpersonno = "";
            string city = "";
            string barangay = "";
            string hasValue = "";
            string status = "";
            string message = "";
            int id = 0;
            int departmentid = 0;
            string pickup = "";

            int?vaccinated = 0;
            DateTime firstdose = new DateTime();
            DateTime seconddose = new DateTime();




            DateTime bday = new DateTime();

            try
            {
                var emp = _context.Employees.Where(a => a.Status == "Active").Where(a => a.EmployeeId == EmpId);

                if (emp.Count() > 0)
                {
                    var employee = emp.FirstOrDefault();
                    var today = DateTime.Today;

                    var _empage = today.Year - employee.Birthday.Year;
                    if (employee.Birthday.Date > today.AddYears(-_empage)) _empage--;



                    empfname = employee.FirstName;
                    emplastname = employee.LastName;
                    contactno = employee.ContactNo;
                    contactperson = employee.ContactPerson;
                    contactpersonno = employee.ContactPersonNo;
                    city = employee.City;
                    barangay = employee.Barangay;
                    //age = employee.Age ;
                    age = _empage;
                    status = "success";
                    id = employee.Id;
                    hasValue = "true";
                    departmentid = employee.DepartmentId;
                    pickup = employee.PickUpPoint;
                    bday = employee.Birthday;

                    vaccinated = employee.Vaccinated;
                    firstdose = employee.FirstDose;
                    seconddose = employee.SecondDose;

                }
                else
                {
                    status = "success";
                    hasValue = "false";
                }



            }
            catch (Exception e)
            {
                status = "fail";
                message = e.InnerException.Message.ToString();
                throw;
            }

            var result = new
            {
                hasValue,
                empfname,
                emplastname,
                contactno,
                contactperson,
                contactpersonno,
                city,
                barangay,
                age,
                status,
                message,
                id,
                departmentid,
                pickup,
                bday,
                vaccinated,
            firstdose,
            seconddose

        };


            return Json(result);
        }


        public ActionResult ThankYou(NotificationViewModel nvm)
        {
            if (nvm.ScreenDateTime.Date != DateTime.Now.Date)
            {
                return RedirectToAction("Index");
            }

            int DaysCounter = QuaratineCounter(nvm.EmployeeNo);

            ViewBag.QuarantineCounter = DaysCounter;

            ViewBag.EmployeeName = nvm.EmployeeName;
            ViewBag.EmployeeNo = nvm.EmployeeNo;

            ViewBag.TotalScore = nvm.TotalScore;
            ViewBag.Remarks = nvm.Remarks;
            ViewBag.Encoder = nvm.Encoder;
            ViewBag.ScreenLogId = nvm.ScreenLogId;
            ViewBag.DepartmentId = nvm.DepartmentId;
            ViewBag.QRKey = nvm.QRKey;
            ViewBag.ScreenDateTime = nvm.ScreenDateTime;
            return View();
        }
        public async Task<string> SetDepartmentHeadDetails(int deptId, int screenlog_id)
        {
            string status = "";
            try
            {
                var dept = await _context.Departments.FindAsync(deptId);
                string deptHeads = dept.DepartmentHeads;
                var collection = deptHeads.Split(',');
                phonenumber = new string[collection.Length + 7];
                emailaddress = new string[collection.Length];
                int cnt = 0;
                foreach (var item in collection)
                {
                    int id = Convert.ToInt32(item);
                    var deptHeadDetail = _context.DepartmentHeads.Find(id);
                    phonenumber[cnt] = deptHeadDetail.ContactNo;
                    emailaddress[cnt] = deptHeadDetail.Email;
                    cnt++;
                }

                phonenumber[cnt] = "09957566792"; // madz
                phonenumber[cnt + 1] = "09197989212"; // charizza
                phonenumber[cnt + 2] = "09985974855"; // jonathan
                phonenumber[cnt + 3] = "09257061809"; // fides quintos
                phonenumber[cnt + 4] = "09173121719"; // joyce lagarde
                phonenumber[cnt + 5] = "09953668620"; // raph
                phonenumber[cnt + 6] = "09998876595"; // jojo tandoc
              
                status = "success";
            }
            catch (Exception)
            {
                status = "fail";

            }


            Logs log = new Logs();
            log.Action = "Get Dept Head Details";
            log.Description = "Get DeptHead Details. Details phonenos " + phonenumber.ToString() + " emails " + emailaddress.ToString() + " SceenLog Id : " + screenlog_id;
            log.Status = status;
            _context.Logs.Add(log);
            _context.SaveChanges();



            return "Done Set Dept Head";

        }
        public string SendNotification(string empName, string remarks, int totalScore, int deptId, int screenlog_id)
        {

            string status = "";
            Task<string> task = SetDepartmentHeadDetails(deptId, screenlog_id);
            task.Wait();
            //var x = task.Result;


            try
            {
                //var x = phonenumber;
                string smsResult = SendSMS("COVID-19 DAILY HEALTH MONITORING TOOL Infoblast" + Environment.NewLine + Environment.NewLine + empName + " has a total score of " + totalScore
                                                              + Environment.NewLine + "Remarks:" + Environment.NewLine + remarks, screenlog_id);

                //string emailResult = SendEmail("COVID-19 DAILY HEALTH MONITORING TOOL Infoblast " + Environment.NewLine + Environment.NewLine + empName + " has a total score of " + totalScore
                //                + Environment.NewLine + " Remarks:" + Environment.NewLine + remarks, screenlog_id, "Employee");

                status = SendEmail("COVID-19 DAILY HEALTH MONITORING TOOL Infoblast <br /><br />" + empName + " has a total score of " + totalScore
                               + "<br /> Remarks: <br />" + remarks + ". <br /><br />", screenlog_id, "Employee");
                status = "success";
            }
            catch (Exception e)
            {
                status = "fail " + e.InnerException.Message;

            }
            return status;

        }
        [HttpPost]
        public string SendSMS(string msg, int screenid)
        {
            //msg = msg.Remove(msg.Length - 1);
            SMSArray res = new SMSArray();
            try
            {

                var sms = new
                {
                    message = msg,
                    PhoneNumbers = phonenumber,
                };

                string xstring = JsonConvert.SerializeObject(sms);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                response = client.GetAsync("http://192.168.70.74/smsapi/api/smsweb?cmd=" + xstring).Result; //live

                if (response.IsSuccessStatusCode)
                {
                    res.message = "200";
                }
                else
                {
                    res.message = response.StatusCode.ToString();
                }




            }
            catch (Exception e)
            {

                res.message = e.InnerException.Message.ToString();
            }

            Logs log = new Logs();
            log.Action = "Send SMS";
            log.Description = "Send SMS by SceenLog Id : " + screenid;
            log.Status = res.message;
            _context.Logs.Add(log);
            _context.SaveChanges();

            return res.message;

        }

        private string SendEmail(string msg, int screenid, string sendertype)
        {
            SMSArray res = new SMSArray();

            try
            {
                //msg = msg.Remove(msg.Length - 1);
                var body = msg;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("webhelpdeskadmin@semirarampc.com", "Daily Health Monitoring Tool Notification");



                //get sup email
                if (sendertype == "Employee")
                {
                    mail.To.Add(new MailAddress("hrod@semirarampc.com"));
                    foreach (var item in emailaddress)
                    {
                        mail.CC.Add(new MailAddress(item));
                    }

                }
                else
                {
                    mail.To.Add(new MailAddress("hrod@semirarampc.com"));
                }



                //get sup email


                mail.Bcc.Add(new MailAddress("rpgustilo@semirarampc.com"));
                //mail.Bcc.Add(new MailAddress("kcmalapit@semirarampc.com"));

                mail.Subject = "DAILY HEALTH MONITORING TOOL";
                mail.Body = string.Format(body + " Click on this link to view details. https://www.semirarampc.com:8443/DailyHealthMonitoringTool");
                mail.IsBodyHtml = true;

                using (var smtp = new SmtpClient()) //mail server
                {
                    try
                    {
                        smtp.Host = "mail.hoaccess.com";
                        smtp.Credentials = new System.Net.NetworkCredential(@"smcdacon\webhelpdeskadmin", "Str@wb3rry##");
                        smtp.Port = 587;
                        smtp.EnableSsl = false;
                        smtp.Send(mail);

                    }
                    catch (Exception e)
                    {


                        Environment.Exit(0);
                    }

                }
                res.message = "Done Email";

            }
            catch (Exception e)
            {

                res.message = e.InnerException.Message.ToString();
            }

            Logs log = new Logs();
            log.Action = "Send SMS";
            log.Description = "Send SMS by SceenLog Id : " + screenid;
            log.Status = res.message;
            _context.Logs.Add(log);
            _context.SaveChanges();
            return res.message;

            return "";
        }
        public IActionResult Verification(string id)
        {

            if (id == null)
            {
                return View("NoRecord");
            }

            var employee = _context.ScreenLogs
               .Where(a => a.QRKey == id)
               .FirstOrDefault();

            if (employee == null)
            {
                return View("NoRecord");
            }

            return View(employee);
        }
        public IActionResult AcceptEntry(QREntry qr)
        {
            string status;
            string message;

            try
            {
                var screenlog = _context.ScreenLogs.Find(qr.ScreenLogId);
                screenlog.EntryStatus = "Claimed";
                _context.Update(screenlog);
                //_context.SaveChanges();



                qr.CreatedAt = DateTime.Now;
                _context.QREntry.Add(qr);
                //_context.SaveChanges();
                status = "success";
                message = "";

                Logs log = new Logs();
                log.Action = "QR Entry";
                log.Description = "Accept QR Entry ScreenLog Id : " + qr.ScreenLogId;
                log.Status = status;
                _context.Logs.Add(log);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                status = "fail";
                message = e.InnerException.Message.ToString();

            }

            var result = new
            {

                status,
                message
            };

            return Json(result);
        }



    }
}
