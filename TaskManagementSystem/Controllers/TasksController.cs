﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tasks
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.Project).Include(t => t.Result).Include(t => t.TaskPriority).Include(t => t.TaskStatus).Include(t => t.UserCreate).Include(t => t.TaskType).Where(t => t.isTemplate == false);
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Include(u => u.UserCreate).Include(au => au.AssignedUser).FirstOrDefault(t => t.ID == id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create(int? id, bool template = false, bool fromTemplate = false)
        {
            if(id!=null)
            {
                ViewBag.ParentTask = db.Tasks.First(t => t.ID == id);
            }
            ViewBag.isTemplate = template;

            ViewBag.TemplateId = new SelectList(db.Tasks.Where(t => t.isTemplate == true).ToList(),"ID","Title");
            ViewBag.fromTemplate = fromTemplate;
            ViewBag.TaskTypeId = new SelectList(db.TaskTypes, "ID", "Type");
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName");
            ViewBag.TaskPriorityId = new SelectList(db.TaskPriorities, "ID", "Priority");
            ViewBag.GroupId = new MultiSelectList(db.Groups, "ID", "Name");
            ViewBag.AssignedUserId = new MultiSelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: Tasks/Create

        public async System.Threading.Tasks.Task<JsonResult> GetTemplate(int? id)
        {
            var task = await db.Tasks.FindAsync(id);
            Template template = new Template()
            {
                Title = task.Title,
                Description = task.Description,
                CurrentStatusId = task.CurrentStatusId,
                ProjectId = task.ProjectId,
                TaskPriorityId = task.TaskPriorityId,
                TaskTypeId = task.TaskTypeId
            };
            return Json(template,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,isTemplate,Title,Description,TaskPriorityId,ProjectId,TaskTypeId")] Task task,FormCollection fc)
        {
            string[] AssignedUserIds = fc.GetValues("AssignedUserId");
            string[] GroupIds = fc.GetValues("GroupId");
           // bool isTemplate = Convert.ToBoolean(fc["isTemplate"]);
            if (!task.isTemplate && AssignedUserIds == null && GroupIds == null)
                ModelState.AddModelError("", "Выберите или пользователя или группу");
            if (ModelState.IsValid)
            {
                if(task.ParentTaskId != null)
                {
                    task.ParentTask = db.Tasks.First(t => t.ID == task.ParentTaskId);
                }
                if (!task.isTemplate)
                {
                    task.DateStart = DateTime.Now;
                    task.LastUpdate = DateTime.Now;
                    task.UserCreate = db.Users.First(user => user.UserName.Equals(User.Identity.Name));
                }
                TaskStatus status = db.TaskStatuses.First(s => s.Status.Equals("Открыта"));
                task.CurrentStatusId = status.ID;
                task.TaskStatus = status;
                
                if(AssignedUserIds != null)
                foreach(var userId in AssignedUserIds)
                {
                    task.AssignedUser = db.Users.Find(userId);
                    db.Tasks.Add(task);
                    db.SaveChanges();
                    AddAttachment(task.ID);
                    db.SaveChanges();
                }
                if (GroupIds != null)
                    foreach (var groupId in GroupIds)
                {
                    task.AssignedUser = null;
                    task.AssignedUserId = null;
                    task.Group = db.Groups.Find(groupId);
                    db.Tasks.Add(task);
                    db.SaveChanges();
                    AddAttachment(task.ID);
                    db.SaveChanges();
                }
                if(GroupIds == null && AssignedUserIds == null && task.isTemplate)
                {
                    db.Tasks.Add(task);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.isTemplate = task.isTemplate;
            ViewBag.TaskTypeId = new SelectList(db.TaskTypes, "ID", "Type", task.TaskTypeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName", task.ProjectId);
            ViewBag.ResultId = new SelectList(db.Results, "ID", "ResultText", task.ResultId);
            ViewBag.TaskPriorityId = new SelectList(db.TaskPriorities, "ID", "Priority", task.TaskPriorityId);
            ViewBag.CurrentStatusId = new SelectList(db.TaskStatuses, "ID", "Status", task.CurrentStatusId);
            ViewBag.UserCreateId = new SelectList(db.Users, "Id", "UserName", task.UserCreateId);
            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "UserName", task.AssignedUserId);
            ViewBag.GroupId = new MultiSelectList(db.Groups, "ID", "Name", GroupIds);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaskTypeId = new SelectList(db.TaskTypes, "ID", "Type", task.TaskTypeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName", task.ProjectId);
            ViewBag.ResultId = new SelectList(db.Results, "ID", "ResultText", task.ResultId);
            ViewBag.TaskPriorityId = new SelectList(db.TaskPriorities, "ID", "Priority", task.TaskPriorityId);
            ViewBag.CurrentStatusId = new SelectList(db.TaskStatuses, "ID", "Status", task.CurrentStatusId);
            ViewBag.UserCreateId = new SelectList(db.Users, "Id", "Email", task.UserCreateId);
            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "UserName",task.AssignedUserId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,AssignedUserId,DateStart,UserCreateId,CurrentStatusId,TaskPriorityId,ProjectId,ScheduledTime,TaskTypeId")] Task task)
        {
            if (ModelState.IsValid)
            {
                task.LastUpdate = DateTime.Now;
                task.AssignedUser = db.Users.Find(task.AssignedUserId);
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TaskTypeId = new SelectList(db.TaskTypes, "ID", "Type", task.TaskTypeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName", task.ProjectId);
            ViewBag.ResultId = new SelectList(db.Results, "ID", "ResultText", task.ResultId);
            ViewBag.TaskPriorityId = new SelectList(db.TaskPriorities, "ID", "Priority", task.TaskPriorityId);
            ViewBag.CurrentStatusId = new SelectList(db.TaskStatuses, "ID", "Status", task.CurrentStatusId);
            ViewBag.UserCreateId = new SelectList(db.Users, "Id", "Email", task.UserCreateId);
            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "UserName", task.AssignedUserId);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AssignToMe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var task = db.Tasks.Find(id);
            task.LastUpdate = DateTime.Now;
            task.TaskStatus = db.TaskStatuses.FirstOrDefault(s => s.Status.Equals(Shared.StatusConstants.IN_PROCESS));
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details",new { id = task.ID });
        }


        public void ChangeStatus(Task task,string status)
        {
            TaskStatus ts = db.TaskStatuses.First(s => s.Status.Equals(status));
            task.CurrentStatusId = ts.ID;
            task.TaskStatus = ts;
        }
        public ActionResult NeedInput(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.First(t => t.ID == id);
            task.AssignedUserId = task.UserCreateId;
            task.AssignedUser = task.UserCreate;
            ChangeStatus(task, Shared.StatusConstants.NEED_INPUT);
            task.LastUpdate = DateTime.Now;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details",new { id= task.ID });
        }
        public Attachment AddAttachment(int TaskId)
        {
            Task task = db.Tasks.First(t => t.ID == TaskId);
            Attachment attach = new Attachment();
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].ContentLength == 0) continue;
                StringBuilder pathToSave = new StringBuilder(Server.MapPath("~/Attachments/"));

                if (!Directory.Exists(pathToSave.ToString()))
                {
                    Directory.CreateDirectory(pathToSave.ToString());
                }
                pathToSave.Append(task.Project.ProjectName + "//");
                if (!Directory.Exists(pathToSave.ToString()))
                {
                    Directory.CreateDirectory(pathToSave.ToString());
                }
                pathToSave.Append(GetShortTaskName(task) + "//");
                if (!Directory.Exists(pathToSave.ToString()))
                {
                    Directory.CreateDirectory(pathToSave.ToString());
                }

                string filename = Path.GetFileName(Request.Files[upload].FileName);
                string serverFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + filename;
                attach.FileName = filename;
                attach.ServerFileName = serverFileName;
                attach.Task = task;
                attach.TaskId = task.ID;
                var us = db.Users.First(user => user.UserName.Equals(User.Identity.Name));
                attach.User = us;
                attach.UserId = us.Id;
                db.Attachments.Add(attach);
                Request.Files[upload].SaveAs(Path.Combine(pathToSave.ToString(), serverFileName));
                db.SaveChanges();
            }
            return attach;
        }

        public string GetShortTaskName(Task task)
        {
            return task.Project.ShortName + "_" + task.ID;
        }

        public ActionResult Download(int? attachId)
        {
            Attachment attach = db.Attachments.Find(attachId);
            string filePath = GetAttachmentPath(attach);
            string fullName = Path.Combine(filePath, attach.ServerFileName);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, attach.FileName);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        
        [HttpPost]
        public ActionResult AddAttachments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.First(t => t.ID == id);
            task.LastUpdate = DateTime.Now;
            AddAttachment(task.ID);
            db.Entry(task).State = EntityState.Modified;
            return View("Details", task);
        }

        [HttpGet]
        public ActionResult DeleteAttachment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Attachment attach = db.Attachments.Find(id);
            Task task = attach.Task;
            task.LastUpdate = DateTime.Now;
            db.Entry(task).State = EntityState.Modified;
            db.Attachments.Remove(attach);
            db.SaveChanges();
            return View("Details", task);
        }
        private string GetAttachmentPath(Attachment attach)
        {
            StringBuilder pathToSave = new StringBuilder(Server.MapPath("~/Attachments/") + attach.Task.Project.ProjectName + "\\" + GetShortTaskName(attach.Task));

            return pathToSave.ToString();
        }

        [HttpPost]
        public ActionResult AddCommentAjax(string comment,int? taskId)
        {
            Task task = db.Tasks.First(t => t.ID == taskId);
            Comment com = new Comment();
            com.CommentText = comment;
            var us = db.Users.First(user => user.UserName.Equals(User.Identity.Name));
            com.User = us;
            com.UserId = us.Id;
            com.Time = DateTime.Now;
            com.Task = task;
            com.TaskId = task.ID;
            db.Comments.Add(com);
            db.SaveChanges();
            
            return PartialView("Comment",com);
        }

       
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> GetComments(int id)
        {
            var comments = db.Tasks.Find(id).Comments.OrderBy(t => t.Time).ToList();
            List<KeyValuePair<string, string>> fooDict = new List<KeyValuePair<string, string>>();
            foreach (var c in comments)
            {
                fooDict.Add(new KeyValuePair<string, string>(c.User.UserName, c.CommentText));
            }
            return Json(new { Data = fooDict }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AssignUserPopUp(int? taskId)
        {
            if (taskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.First(t => t.ID == taskId);
            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "UserName", task.AssignedUserId);
            return PartialView(task);
        }
        [HttpPost]
        public ActionResult AssignUserPopUp(int taskId,string AssignedUserId)
        {
            Task task = db.Tasks.First(t => t.ID == taskId);
            ApplicationUser user = db.Users.Find(AssignedUserId);
            task.AssignedUser = user;
            task.AssignedUserId = user.Id;
            ChangeStatus(task, Shared.StatusConstants.OPEN);
            task.LastUpdate = DateTime.Now;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details",new { id = task.ID });
        }

        public ActionResult DonePopUp(int? taskId)
        {
            if (taskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.First(t => t.ID == taskId);
            if (task.TaskType.Type == Shared.TypesConstants.NO_CONFIRMATION)
            {
                task.LastUpdate = DateTime.Now;
                ChangeStatus(task, Shared.StatusConstants.CLOSED);
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = task.ID });
            }
            return PartialView(task);
        }
        [HttpPost]
        public ActionResult DonePopUp(int taskId, string resultText)
        {
            Result result = new Result();
            Attachment attach = AddAttachment(taskId);
            result.ResultText = resultText;
            if (attach.FileName != null)
            {
                result.Attachment = attach;
                result.AttachmentId = attach.ID;
            }
            Task task = db.Tasks.First(t => t.ID == taskId);
            ChangeStatus(task, Shared.StatusConstants.NEED_VERIFY);
            task.AssignedUserId = task.UserCreateId;
            task.AssignedUser = task.UserCreate;
            task.Result = result;
            task.LastUpdate = DateTime.Now;
            db.Entry(task).State = EntityState.Modified;
            db.Results.Add(result);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = task.ID });
        }

        public ActionResult Verified(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.First(t => t.ID == id);
            ChangeStatus(task, Shared.StatusConstants.VERIFY_CLOSED);
            task.LastUpdate = DateTime.Now;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
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