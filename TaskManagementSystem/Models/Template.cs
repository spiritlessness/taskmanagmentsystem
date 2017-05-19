using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Models
{
    public class Template
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int CurrentStatusId { get; set; }
        public int TaskTypeId { get; set; }
        public int TaskPriorityId { get; set; }
        public int ProjectId { get; set; }
    }
}