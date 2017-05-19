using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Entities
{
    public class Comment : BaseEntity
    {
        public string CommentText { get; set; }

        public DateTime Time { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }

        [Display(Name = "Пользователь")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Задача")]
        public virtual Task Task { get; set; }
    }
}