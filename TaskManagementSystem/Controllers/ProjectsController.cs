using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        public ActionResult Index()
        {
            var projects = db.Projects.Include(t => t.TeamLead);
            return View(projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            List<Task> tasks = db.Tasks.Where(p => p.ProjectId == id).ToList();
            ViewBag.Tasks = tasks;
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.TeamLead = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: Projects/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectName,TeamLeadId")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.TeamLead = db.Users.Find(project.TeamLeadId);
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamLead = new SelectList(db.Users, "Id", "UserName", project.TeamLeadId);
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamLeadId = new SelectList(db.Users, "Id", "UserName", project.TeamLeadId);
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectName,TeamLeadId")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.TeamLead = db.Users.Find(project.TeamLeadId);
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamLeadId = new SelectList(db.Users, "Id", "UserName", project.TeamLeadId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Include(t => t.Tasks).First(p => p.ID == id);
            if(project.Tasks != null && project.Tasks.Count != 0)
            {
                foreach(var task in db.Tasks.Include(r => r.Result).Include(a => a.Attachments).Where(p=>p.ProjectId == id).ToList())
                {
                    if (task.Result != null)
                        Repository.Delete<Result>(task.Result, db);
                    if (task.Attachments.Count > 0)
                        db.Attachments.RemoveRange(task.Attachments);
                }
                db.Tasks.RemoveRange(project.Tasks);
            }
            Repository.Delete<Project>(project,db);
            return RedirectToAction("Index");
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
