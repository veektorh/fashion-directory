namespace AspMvcStarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtwomorecolumntophototable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Version", c => c.String());
            AddColumn("dbo.Photos", "Format", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "Format");
            DropColumn("dbo.Photos", "Version");
        }
    }
}
