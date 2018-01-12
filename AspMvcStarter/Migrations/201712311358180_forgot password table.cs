namespace AspMvcStarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forgotpasswordtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForgotPasswords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Time = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForgotPasswords", "User_Id", "dbo.Users");
            DropIndex("dbo.ForgotPasswords", new[] { "User_Id" });
            DropTable("dbo.ForgotPasswords");
        }
    }
}
