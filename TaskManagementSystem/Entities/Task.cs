using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TaskManagementSystem.Entities;

namespace TaskManagementSystem.Models
{
    [Table("Tasks")]
    public class Task : BaseEntity
    {
        public Task()
        {
            this.Attachments = new HashSet<Attachment>();
        }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        public string Title { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Дата Создания")]
        public DateTime? DateStart { get; set; }
        [Display(Name = "Последнее обновление")]
        public DateTime? LastUpdate { get; set; }
        [Display(Name = "Нужно сделать до")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.DateTime)]
        public DateTime? ScheduledTime { get; set; }
        [ForeignKey("ResponsibleUser")]
        public string ResponsibleUserId { get; set; }
        [ForeignKey("AssignedUser")]
        public string AssignedUserId { get; set; }
        [ForeignKey("TaskStatus")]
        public int CurrentStatusId { get; set; }
        [ForeignKey("Group")]
        public int? GroupId { get; set; }
        [ForeignKey("TaskType")]
        public int TaskTypeId { get; set; }
        [ForeignKey("TaskPriority")]
        public int TaskPriorityId { get; set; }
        [ForeignKey("Result")]
        public int? ResultId { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        [ForeignKey("ParentTask")]
        public int? ParentTaskId { get; set; }

        public bool isTemplate { get; set; }
        [Display(Name ="Основная задача")]
        public virtual Task ParentTask { get; set; }
        [Display(Name = "Файлы")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attachment> Attachments { get; set; }
        [Display(Name = "Комментарии")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [Display(Name = "Назначена группе")]
        public virtual Group Group { get; set; }
        [Display(Name = "Проект")]
        public virtual Project Project { get; set; }
        public virtual TaskType TaskType { get; set; }
        public virtual TaskPriority TaskPriority { get; set; }
        public virtual TaskStatus TaskStatus { get; set; }
        [Display(Name = "Ответственный пользователь")]
        public virtual ApplicationUser ResponsibleUser { get; set; }
        [Display(Name = "Назначена пользователю")]
        public virtual ApplicationUser AssignedUser { get; set; }
        public virtual Result Result { get; set; }
    }
}