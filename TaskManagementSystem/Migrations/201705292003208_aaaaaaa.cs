namespace TaskManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaaaaaa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "groupCreateId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "groupCreateId");
            AddForeignKey("dbo.AspNetUsers", "groupCreateId", "dbo.Groups", "ID");
            DropColumn("dbo.AspNetUsers", "groupId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "groupCreateId", "dbo.Groups");
            DropIndex("dbo.AspNetUsers", new[] { "groupCreateId" });
            DropColumn("dbo.AspNetUsers", "groupCreateId");
        }
    }
}
