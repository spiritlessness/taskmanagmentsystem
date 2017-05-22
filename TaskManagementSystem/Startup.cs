using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Shared;
using System.Data.Entity;
using TaskManagementSystem.Models;

[assembly: OwinStartupAttribute(typeof(TaskManagementSystem.Startup))]
namespace TaskManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User  
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);            

                var user = new ApplicationUser();
                user.UserName = "admin";
                string userPWD = "adminadmin";

                var chkUser = UserManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Оператор"))
            {
                var role = new IdentityRole();
                role.Name = "Оператор";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Инженер"))
            {
                var role = new IdentityRole();
                role.Name = "Инженер";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Заведующий"))
            {
                var role = new IdentityRole();
                role.Name = "Заведующий";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Ассистент"))
            {
                var role = new IdentityRole();
                role.Name = "Ассистент";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Cтарший преподаватель"))
            {
                var role = new IdentityRole();
                role.Name = "Cтарший преподаватель";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Доцент"))
            {
                var role = new IdentityRole();
                role.Name = "Доцент";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Профессор"))
            {
                var role = new IdentityRole();
                role.Name = "Профессор";
                roleManager.Create(role);

            }
            
            context.SaveChanges();
        }
    }
}
