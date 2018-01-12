namespace AspMvcStarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewtablelist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FollowedUser_Id = c.Int(),
                        FollowingUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FollowedUser_Id)
                .ForeignKey("dbo.Users", t => t.FollowingUser_Id)
                .Index(t => t.FollowedUser_Id)
                .Index(t => t.FollowingUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lists", "FollowingUser_Id", "dbo.Users");
            DropForeignKey("dbo.Lists", "FollowedUser_Id", "dbo.Users");
            DropIndex("dbo.Lists", new[] { "FollowingUser_Id" });
            DropIndex("dbo.Lists", new[] { "FollowedUser_Id" });
            DropTable("dbo.Lists");
        }
    }
}
