namespace TaskManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaa : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.Groups SET Name ='qqq' WHERE Id = 5 ");

            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Groups", "Name", c => c.String());
        }
    }
}
