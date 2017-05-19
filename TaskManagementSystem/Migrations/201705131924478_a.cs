namespace TaskManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "isConfirmed", c => c.Boolean(nullable: false,defaultValue : true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "isConfirmed");
        }
    }
}
