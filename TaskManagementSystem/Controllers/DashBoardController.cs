using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: DashBoard
        [Authorize]
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {

            ApplicationUser user = await db.Users.FirstAsync(u => u.UserName.Equals(User.Identity.Name));
            var myTasks = await db.Tasks.Where(t => t.AssignedUserId == user.Id).Where(t => t.isTemplate == false).ToListAsync();
            var allGroups = await db.Groups.ToListAsync();
            var myGroups = new List<Group>();
            var myCreatedTasks = await db.Tasks.Where(u => u.ResponsibleUserId == user.Id).ToListAsync();
            foreach(var gr in allGroups)
            {
                if (gr.Users.Contains(user)) myGroups.Add(gr);
            }
            var myGroupTasks = myTasks.Join(myGroups, f => f.GroupId, s => s.ID, (fir, sec) => fir);
            ViewBag.assignedTasks = myTasks;
            ViewBag.overdueTaks = myTasks.Where(t => t.ScheduledTime < DateTime.Now).ToList();
            ViewBag.soonEndTasks = db.Tasks.Where(t => t.AssignedUserId == user.Id).Where(t => t.isTemplate == false).Where(t => DbFunctions.DiffDays(t.ScheduledTime.Value,DateTime.Now) > 1).ToList();
            ViewBag.myCreatedTasks = myCreatedTasks;
            return View();
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> GetNotifications()
        {
            ApplicationUser user = await db.Users.FirstAsync(u => u.UserName.Equals(User.Identity.Name));
            var myTasks = await db.Tasks.Where(t => t.AssignedUserId == user.Id).Where(t => t.isTemplate == false).ToListAsync();

            List<KeyValuePair<string, string>> notifications = new List<KeyValuePair<string, string>>();
            var todayTasksCount = myTasks.Where(t => t.ScheduledTime.Value.Date == DateTime.Now.Date).Count();
            notifications.Add(new KeyValuePair<string, string>("info", String.Format(Shared.Constants.NotificationMessagesFormatConstants.TODAY_TASKS, todayTasksCount)));

            return Json(new { Data = notifications }, JsonRequestBehavior.AllowGet);
        }
    }
}