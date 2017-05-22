using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        public async Task<ActionResult> Index()
        {
            ViewBag.userId = db.Users.FirstOrDefaultAsync(u => u.UserName.Equals(User.Identity.Name)).Result.Id;
            return View(await db.Groups.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            ViewBag.Users = new MultiSelectList(db.Users, "Id", "UserName");
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            ViewBag.Role = new SelectList(roleManager.Roles.Where(r => !(r.Name.Equals("User") || r.Name.Equals("Admin"))), "Name", "Name"," ");

            return View();
        }

        // POST: Groups/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name")] Group group, FormCollection fc)
        {
            if (ModelState.IsValid && fc.GetValues("Users") != null && fc.GetValues("Users").Count() != 0)
            {
                foreach (var userId in fc.GetValues("Users"))
                {
                    group.Users.Add(db.Users.Find(userId));
                }
                var user =await  db.Users.FirstOrDefaultAsync(t => t.UserName.Equals(User.Identity.Name));
                group.UserCreate = user;
                group.UserCreateId = user.Id;
                db.Groups.Add(group);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Users", "Вы не выбрали ни одного пользователя");
            }
            ViewBag.Users = new MultiSelectList(db.Users, "Id", "UserName");
            return View(group);
        }

        // GET: Groups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            List<string> ids = new List<string>();
            foreach (var user in group.Users)
            {
                ids.Add(user.Id);
            }
            ViewBag.UsersList = new MultiSelectList(db.Users, "Id", "UserName", ids);
            return View(group);
        }

        // POST: Groups/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name")] Group group, FormCollection fc)
        {
            if (ModelState.IsValid && fc.GetValues("Users") != null && fc.GetValues("Users").Count() != 0)
            {
                group = db.Groups.Find(group.ID);
                group.Users.Clear();
                db.Entry(group).State = EntityState.Modified;
                await db.SaveChangesAsync();
                foreach (var userId in fc.GetValues("Users"))
                {
                    group.Users.Add(db.Users.Find(userId));
                }
                db.Entry(group).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Users", "Вы не выбрали ни одного пользователя");
            }
            List<string> ids = new List<string>();
            foreach (var user in group.Users)
            {
                ids.Add(user.Id);
            }
            ViewBag.Users = new MultiSelectList(db.Users, "Id", "UserName", ids);
            return View(group);
        }

        // GET: Groups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Group group = await db.Groups.FindAsync(id);
            var tasks = await db.Tasks.Where(g => g.GroupId == group.ID).ToListAsync();
            if (tasks != null && tasks.Count > 0)
            {
                foreach (var task in tasks)
                {
                    task.GroupId = null;
                }

            }
            db.Groups.Remove(group);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async System.Threading.Tasks.Task<JsonResult> GetUserIdsByRole(string roleName)
        {
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Name.Equals(roleName));
            var users = role.Users;
            List<string> userIds = new List<string>();
             foreach (var user in users)
            {
                userIds.Add(user.UserId);
            }
            return Json(userIds, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
