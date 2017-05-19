using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    [Table("TaskStatuses")]
    public class TaskStatus : BaseEntity
    {
        [Display(Name = "Статус")]
        public string Status { get; set; }
    }
}