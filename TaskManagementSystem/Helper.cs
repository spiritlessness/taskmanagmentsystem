using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TaskManagementSystem.Models;

namespace TaskManagementSystem
{
    public static class Helper
    {
        public static async void DeleteTaskReferences(Task task,ApplicationDbContext db)
        {
            if (task.ResultId != null)
            {
                var result = await db.Results.FirstOrDefaultAsync(a => a.ID == task.ResultId);
                if (result.AttachmentId != null)
                {
                    Attachment atach = await db.Attachments.FirstOrDefaultAsync(a => a.ID == task.Result.AttachmentId);
                    Repository.Delete<Attachment>(atach, db);
                }
                Repository.Delete<Result>(result, db);
            }
            if (task.Attachments.Count > 0)
                db.Attachments.RemoveRange(task.Attachments);
        }
    }
}