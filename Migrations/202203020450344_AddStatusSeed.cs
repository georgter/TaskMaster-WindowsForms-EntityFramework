namespace TaskMaster.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusSeed : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Status (Name) VALUES('To Do');");
            Sql("INSERT INTO Status (Name) VALUES('In Progress');");
            Sql("INSERT INTO Status (Name) VALUES('Done');");
        }
        
        public override void Down()
        {
            Sql("DELETE  FROM dbo.Status WHERE Name = 'To Do';");
            Sql("DELETE  FROM dbo.Status WHERE Name = 'In Progress';");
            Sql("DELETE  FROM dbo.Status WHERE Name = 'Done';");
        }
    }
}
