using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Models
{
    [Table("Projects")]
    public class Project : BaseEntity
    {
        [Display(Name = "Название проекта")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Имя проекта обязательно")]
        public string ProjectName { get; set; }
        [ForeignKey("TeamLead")]
        public string TeamLeadId { get; set; }

        [MaxLength(3,ErrorMessage = "Не более 3 символов")]
        public string ShortName { get; set; }
        [Display(Name = "Руководитель")]
        public virtual ApplicationUser TeamLead { get; set; }
        [Display(Name = "Задачи")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}