namespace AspMvcStarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSocialMdeiaPropertiesToUserClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "facebook", c => c.String());
            AddColumn("dbo.Users", "twitter", c => c.String());
            AddColumn("dbo.Users", "instgram", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "instgram");
            DropColumn("dbo.Users", "twitter");
            DropColumn("dbo.Users", "facebook");
        }
    }
}
