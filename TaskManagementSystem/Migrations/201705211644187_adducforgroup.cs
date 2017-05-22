namespace TaskManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adducforgroup : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GroupApplicationUsers", new[] { "Group_ID" });
            DropIndex("dbo.GroupApplicationUsers", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.AspNetUsers", "Group_ID", c => c.Int());
            AddColumn("dbo.Groups", "UserCreateId", c => c.String(maxLength: 128));
            AddColumn("dbo.Groups", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Group_ID");
            CreateIndex("dbo.Groups", "UserCreateId");
            CreateIndex("dbo.Groups", "ApplicationUser_Id");
            AddForeignKey("dbo.Groups", "UserCreateId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Group_ID", "dbo.Groups", "ID");
            AddForeignKey("dbo.Groups", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GroupApplicationUsers",
                c => new
                    {
                        Group_ID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Group_ID, t.ApplicationUser_Id });
            
            DropForeignKey("dbo.Groups", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.Groups", "UserCreateId", "dbo.AspNetUsers");
            DropIndex("dbo.Groups", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Groups", new[] { "UserCreateId" });
            DropIndex("dbo.AspNetUsers", new[] { "Group_ID" });
            DropColumn("dbo.Groups", "ApplicationUser_Id");
            DropColumn("dbo.Groups", "UserCreateId");
            DropColumn("dbo.AspNetUsers", "Group_ID");
            CreateIndex("dbo.GroupApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.GroupApplicationUsers", "Group_ID");
            AddForeignKey("dbo.GroupApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GroupApplicationUsers", "Group_ID", "dbo.Groups", "ID", cascadeDelete: true);
        }
    }
}
