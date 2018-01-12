namespace AspMvcStarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtwomorecolumntousertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Version", c => c.String());
            AddColumn("dbo.Users", "Format", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Format");
            DropColumn("dbo.Users", "Version");
        }
    }
}
