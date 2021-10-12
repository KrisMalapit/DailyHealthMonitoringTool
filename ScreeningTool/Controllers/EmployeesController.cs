using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScreeningTool.Models;

namespace ScreeningTool.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly ScreeningToolContext _context;

        public EmployeesController(ScreeningToolContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var screeningToolContext = _context.Employees.Include(e => e.Departments);
            return View(await screeningToolContext.ToListAsync());
        }
        public async Task<IActionResult> NoDHM()
        {
            
            return View();
        }

        public IActionResult getData(string type)
        {
            DateTime dt = new DateTime(1, 1, 1);
            string status = "";
            var v =

                _context.Employees.Select(a => new
                {
                    a.Id
                    ,
                    a.EmployeeId,

                    EmployeeName = a.LastName + ", " + a.FirstName,

                    a.ContactNo,

                    Department = a.Departments.Name,

                    a.Status
                    ,Type = a.Organic == 1 ? "Organic" : "In-Organic"

           
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
        public IActionResult getDataNoDHM()
        {
            DateTime dt = new DateTime(1, 1, 1);
            string status = "";

            var v = _context.NoDHMCounters
                
                .GroupJoin(
                       _context.Employees // B
                       .Where(a => a.Status == "Active"),
                       i => i.EmployeeId, //A key
                       p => p.EmployeeId,//B key
                       (i, g) =>
                          new
                          {
                              i, //holds A data
                              g  //holds B data
                          }
                    ).SelectMany(
                       temp => temp.g.DefaultIfEmpty(), //gets data and transfer to B
                       (A, B) =>
                          new
                          {
                              A.i.EmployeeId,
                              EmployeeName = B.LastName + ", " + B.FirstName,
                              A.i.Counter
                              ,A.i.ID
                          }
                    );

            //var v =

            //    _context.Employees.Select(a => new
            //    {
            //        a.Id
            //        ,
            //        a.EmployeeId,

            //        EmployeeName = a.LastName + ", " + a.FirstName,

            //        a.ContactNo,

            //        Department = a.Departments.Name,

            //        a.Status
            //        ,
            //        Type = a.Organic == 1 ? "Organic" : "In-Organic"


            //    });

            status = "success";
            var model = new
            {
                status
                ,
                data = v.ToList(),


            };
            return Json(model);
        }
        public JsonResult UpdateCounter(int Id)
        {
            string status = "";
            string message = "";
            var uid = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            int lastCount = 0;
            try
            {
                var emp = _context.NoDHMCounters.Where(a => a.ID == Id).FirstOrDefault();
                lastCount = emp.Counter;
                emp.LastCounter = emp.Counter;
                emp.Counter = 0;
                emp.ResetDate = DateTime.Now;
                emp.UpdatedDate = DateTime.Now.Date;
               

                _context.Update(emp);

                message = "Reset No DHM. EmployeeId : " + emp.EmployeeId + ", Last Count :" + lastCount;
                status = "success";
                

                Logs log = new Logs();
                log.Action = "Modify";
                log.Description = message;
                log.Status = status;
                log.UserId = Convert.ToInt32(uid);
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

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Departments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var emp = _context.Employees.Find(id);
                    emp.EmployeeId = employee.EmployeeId;
                    emp.FirstName = employee.FirstName;
                    emp.LastName = employee.LastName;
                    emp.ContactNo = employee.ContactNo;
                    emp.ContactPerson = employee.ContactPerson;
                    emp.City = employee.City;
                    emp.Barangay = employee.Barangay;
                    emp.Status = employee.Status;
                    emp.DepartmentId = employee.DepartmentId;
                    emp.Birthday = employee.Birthday;
                    //emp.Vaccinated = employee.Vaccinated;
                   
                    emp.Email = employee.Email;
                    emp.Organic = employee.Organic;
                    _context.Update(emp);
                    //await _context.SaveChangesAsync();


                    Logs log = new Logs();
                    log.Action = "Modify";
                    log.Description = "Modify employee details from Employees Controller. Employee Id : " + employee.Id;
                    log.Status = "success";
                    _context.Logs.Add(log);
                    _context.SaveChanges();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Departments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
