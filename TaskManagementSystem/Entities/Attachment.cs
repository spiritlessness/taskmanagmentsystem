using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    [Table("Attachments")]
    public class Attachment : BaseEntity
    {
        [Display(Name = "Файл")]
        public string FileName { get; set; }

        public string ServerFileName { get; set; }

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