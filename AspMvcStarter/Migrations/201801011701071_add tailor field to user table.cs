namespace AspMvcStarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtailorfieldtousertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Tailor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Tailor");
        }
    }
}
