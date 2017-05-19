using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    [Table("TaskTypes")]
    public class TaskType : BaseEntity
    {
        [Display(Name = "Тип")]
        public string Type { get; set; }
    }
}