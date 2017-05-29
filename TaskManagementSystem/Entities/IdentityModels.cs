using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using Shared;
using TaskManagementSystem.Entities;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser():base()
        {
            Groups = new HashSet<Group>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
        [InverseProperty("Users")]
        public virtual ICollection<Group> Groups { get; set; }
        public bool isConfirmed { get; set; }
    }

    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            context.TaskTypes.AddOrUpdate(new TaskType { ID = 1, Type = TypesConstants.LEARN });
            context.TaskTypes.AddOrUpdate(new TaskType { ID = 2, Type = TypesConstants.CHANGE });
            context.TaskTypes.AddOrUpdate(new TaskType { ID = 3, Type = TypesConstants.DO });
            context.TaskTypes.AddOrUpdate(new TaskType { ID = 4, Type = TypesConstants.NO_CONFIRMATION });
            context.TaskPriorities.AddOrUpdate(new TaskPriority { ID = 1, Priority = PriorityConstants.VERY_HIGH });
            context.TaskPriorities.AddOrUpdate(new TaskPriority { ID = 2, Priority = PriorityConstants.HIGH });
            context.TaskPriorities.AddOrUpdate(new TaskPriority { ID = 3, Priority = PriorityConstants.NORMAL });
            context.TaskPriorities.AddOrUpdate(new TaskPriority { ID = 4, Priority = PriorityConstants.LOW });
            context.TaskPriorities.AddOrUpdate(new TaskPriority { ID = 5, Priority = PriorityConstants.VERY_LOW });
            context.TaskStatuses.AddOrUpdate(new TaskStatus { ID = 1, Status = StatusConstants.OPEN });
            context.TaskStatuses.AddOrUpdate(new TaskStatus { ID = 2, Status = StatusConstants.IN_PROCESS });
            context.TaskStatuses.AddOrUpdate(new TaskStatus { ID = 3, Status = StatusConstants.NEED_VERIFY });
            context.TaskStatuses.AddOrUpdate(new TaskStatus { ID = 4, Status = StatusConstants.VERIFY_CLOSED });
            context.TaskStatuses.AddOrUpdate(new TaskStatus { ID = 5, Status = StatusConstants.CLOSED });
            context.TaskStatuses.AddOrUpdate(new TaskStatus { ID = 6, Status = StatusConstants.NEED_INPUT });
            context.SaveChanges();
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
           // this.Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }
        public DbSet<TaskPriority> TaskPriorities { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<TaskManagementSystem.Entities.Group> Groups { get; set; }
    }
}