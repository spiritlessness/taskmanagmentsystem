using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    [Table("TaskPriorities")]
    public class TaskPriority : BaseEntity
    {
        [Display(Name = "Приоритет")]
        public string Priority { get; set; }
    }
}