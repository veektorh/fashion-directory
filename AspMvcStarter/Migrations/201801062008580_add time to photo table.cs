namespace AspMvcStarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtimetophototable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "TimePosted", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "TimePosted");
        }
    }
}
