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
    public class DepartmentHeadsController : Controller
    {
        private readonly ScreeningToolContext _context;

        public DepartmentHeadsController(ScreeningToolContext context)
        {
            _context = context;
        }

        // GET: DepartmentHeads
        public async Task<IActionResult> Index()
        {
            return View(await _context.DepartmentHeads.ToListAsync());
        }
        public IActionResult getData(string type)
        {
            DateTime dt = new DateTime(1, 1, 1);
            string status = "";
            var v =

                _context.DepartmentHeads.Select(a => new
                {
                    a.Id
                    ,EmployeeName = a.Name,
                    a.Email,
                    a.ContactNo,
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

        // GET: DepartmentHeads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentHead = await _context.DepartmentHeads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departmentHead == null)
            {
                return NotFound();
            }

            return View(departmentHead);
        }

        // GET: DepartmentHeads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DepartmentHeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,ContactNo,Status")] DepartmentHead departmentHead)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departmentHead);
                await _context.SaveChangesAsync();


                Logs log = new Logs();
                log.Action = "Add";
                log.Description = "DeptHead employee details from Home Controller. DeptHead Id : " + departmentHead.Id;
                log.Status = "success";
                _context.Logs.Add(log);
                _context.SaveChanges();



                return RedirectToAction(nameof(Index));
            }
            return View(departmentHead);
        }

        // GET: DepartmentHeads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentHead = await _context.DepartmentHeads.FindAsync(id);
            if (departmentHead == null)
            {
                return NotFound();
            }
            return View(departmentHead);
        }

        // POST: DepartmentHeads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,ContactNo,Status")] DepartmentHead departmentHead)
        {
            if (id != departmentHead.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departmentHead);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentHeadExists(departmentHead.Id))
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
            return View(departmentHead);
        }

        // GET: DepartmentHeads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentHead = await _context.DepartmentHeads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departmentHead == null)
            {
                return NotFound();
            }

            return View(departmentHead);
        }

        // POST: DepartmentHeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departmentHead = await _context.DepartmentHeads.FindAsync(id);
            _context.DepartmentHeads.Remove(departmentHead);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentHeadExists(int id)
        {
            return _context.DepartmentHeads.Any(e => e.Id == id);
        }
    }
}
