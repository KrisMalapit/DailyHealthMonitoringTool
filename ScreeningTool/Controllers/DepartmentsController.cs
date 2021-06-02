using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScreeningTool.Models;
using ScreeningTool.Models.View_Model;

namespace ScreeningTool.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly ScreeningToolContext _context;

        public DepartmentsController(ScreeningToolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult getData(string type)
        {
            DateTime dt = new DateTime(1, 1, 1);
            string status = "";
            var v =

                _context.Departments.Select(a => new
                {
                    a.Id
                    ,
                    a.Code,

                    a.Name,

                    ComapanyName = a.Companies.Name,

                    DepartmentHead = a.DepartmentHeads,

                    a.Status


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
        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var screeningToolContext = _context.Departments.Include(d => d.Companies);
            return View(await screeningToolContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Companies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            var deptheadList = _context.DepartmentHeads.Where(a => a.Status == "Active").Select(b => new
            {
                b.Id,
                b.Name,
            });
       
            ViewData["DepartmentHeads"] = new SelectList(deptheadList.OrderBy(a => a.Name), "Id", "Name");

            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult CreateEdit(Department department)
        {
            string status = "";
            string message = "";

            try
            {
                if (department.Id == 0)
                {
                    _context.Add(department);
                    _context.SaveChanges();
                    status = "success";

                    Logs log = new Logs();
                    log.Action = "Add";
                    log.Description = "Add Department details. Department Id : " + department.Id;
                    log.Status = status;
                    _context.Logs.Add(log);
                    _context.SaveChanges();
                }
                else
                {

                    _context.Update(department);
                    _context.SaveChanges();
                    status = "success";


                    Logs log = new Logs();
                    log.Action = "Modify";
                    log.Description = "Modify Department details. Department Id : " + department.Id;
                    log.Status = status;
                    _context.Logs.Add(log);
                    _context.SaveChanges();

                }
               

            }
            catch (Exception e)
            {
                status = "fail";
                message = e.InnerException.Message.ToString();
      
            }

            //ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", department.CompanyId);


            var model = new
            {
                status,
                message
                
            };

            return Json(model);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", department.CompanyId);


            var deptheadList = _context.DepartmentHeads.Where(a => a.Status == "Active").Select(b => new
            {
                b.Id,
                b.Name,
            });

            ViewData["DepartmentHeads"] = new SelectList(deptheadList.OrderBy(a => a.Name), "Id", "Name");

            ViewData["DepartmentHeadsId"] = department.DepartmentHeads;

            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,CompanyId,DepartmentHeads,Status")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", department.CompanyId);
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Companies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }

    }
}
