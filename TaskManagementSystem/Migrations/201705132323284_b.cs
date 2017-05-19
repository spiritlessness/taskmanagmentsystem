namespace TaskManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GroupApplicationUsers",
                c => new
                    {
                        Group_ID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Group_ID, t.ApplicationUser_Id })
                .ForeignKey("dbo.Groups", t => t.Group_ID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Group_ID)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.Tasks", "GroupId", c => c.Int(nullable: true));
            CreateIndex("dbo.Tasks", "GroupId");
            AddForeignKey("dbo.Tasks", "GroupId", "dbo.Groups", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupApplicationUsers", "Group_ID", "dbo.Groups");
            DropIndex("dbo.GroupApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GroupApplicationUsers", new[] { "Group_ID" });
            DropIndex("dbo.Tasks", new[] { "GroupId" });
            DropColumn("dbo.Tasks", "GroupId");
            DropTable("dbo.GroupApplicationUsers");
            DropTable("dbo.Groups");
        }
    }
}
