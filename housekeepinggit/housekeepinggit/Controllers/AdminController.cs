using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using housekeepinggit.Data;
using housekeepinggit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace housekeepinggit.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;

            _userManager = userManager;
        }

        public async Task<ActionResult> AssignTaskForm(int? taskID)
        {
            if (taskID == null)
            {
                return NotFound();
            }

            var task = _context.Task.Include(l => l.location).Where(m => m.ID == taskID).First();

            ViewBag.users = _context.Users
            .Select(a => new SelectListItem()
            {
                Value = a.Id,
                Text = $"{a.firstName} ({a.UserName}) {a.lastName}"
            })
          .ToList();

            if (task == null)
            {
                return NotFound();
            }

            return View(task);

        }
        public async Task<ActionResult> AssignTask(string userID, int? taskID)
        {
            if (userID == null | taskID == null)
            {
                return NotFound();
            }

            var user = _context.Users.Find(userID);

            var task = _context.Task.Find(taskID);

            if (user == null | task == null)
            {
                return NotFound();
            }

            task.houseKeeper = user;
            task.status = "Назначена на домашен помощник";

            _context.SaveChanges();

            return Redirect("/Home/Index");
        }

        public async Task<ActionResult> MakeHousekeeper(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.AddToRoleAsync(user, "Housekeeper");
            await _userManager.RemoveFromRoleAsync(user, "Client");

            return Redirect("/Home/Index");
        }

        // GET: AdminController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: AdminController/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.Find(id);

            return View(user);
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return LocalRedirect("/Identity/Account/Register");
        }


        //// GET: AdminController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AdminController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(IndexAsync));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: AdminController/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
