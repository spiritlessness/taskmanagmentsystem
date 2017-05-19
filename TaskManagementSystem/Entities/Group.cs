using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Entities
{
    public class Group:BaseEntity
    {
        public Group()
        {
            Users = new HashSet<ApplicationUser>();
        }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}