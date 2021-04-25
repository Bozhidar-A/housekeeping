using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using housekeepinggit.Data;
using housekeepinggit.Models;
using Microsoft.Extensions.Configuration;

namespace housekeepinggit.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        public TasksController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;

            _configuration = configuration;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Task.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.Include(l => l.location)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            ViewBag.category = _configuration.GetSection("TaskCategories").Get<List<string>>()
                .Select(x =>
                new SelectListItem() { Text = x.ToString(), Value = x.ToString() });

            //get category from appsettings

            ViewBag.locs = _context.Location
                .Select(a => new SelectListItem()
                {
                    Value = a.ID.ToString(),
                    Text = a.name
                })
              .ToList();

            //get locs from db

            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,name,description,endDate,budget,category")] Models.Task task, int locID)
        {
            if (ModelState.IsValid)
            {
                Location loc = _context.Location.Find(locID);
                task.location = loc;
                task.status = "Чакаща";
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.Include(l => l.location).Where(m => m.ID == id).FirstAsync();
            if (task == null)
            {
                return NotFound();
            }

            if(task.status != "Чакаща")
            {
                //user tries to edit task that should not be editable
                //return to index of tasks
                return RedirectToAction(nameof(Index));
            }

            var tempsl = _context.Location.Where(m => m.ID != task.location.ID)
                .Select(a => new SelectListItem()
              {
                  Value = a.ID.ToString(),
                  Text = a.name
              })
              .ToList();

            //get all BUT the current loctions

            var currsli = new SelectListItem()
            {
                Value = task.location.ID.ToString() + " (current)",
                Text = task.location.name
            };
            //get the current location and turn into SelectListItem

            tempsl.Insert(0, currsli);
            //insert at the front

            //set to viewbag
            ViewBag.locs = tempsl;

            //set previd to viewbag
            ViewBag.prevLocID = task.location.ID;


            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,name,description,endDate,budget,category")] Models.Task task, int locID, int prevLocID)
        {
            if (id != task.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(locID != prevLocID)
                    {
                        task.location = _context.Location.Find(locID);
                    }
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.ID))
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
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.ID == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Task.FindAsync(id);
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> UpdateStatus(int? id)
        //{
        //    var currtask = await _context.Task.FindAsync(id);

        //    if(currtask == null)
        //    {
        //        return NotFound();
        //    }

        //    if(currtask.status == "За преглед")
        //    {
        //        currtask.status = "Изпълнена";
        //    }

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.ID == id);
        }
    }
}
