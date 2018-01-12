namespace AspMvcStarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcommentnotificationliketable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        TimePosted = c.DateTime(nullable: false),
                        Photo_Id = c.Int(),
                        Receiver_Id = c.Int(),
                        Sender_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .ForeignKey("dbo.Users", t => t.Receiver_Id)
                .ForeignKey("dbo.Users", t => t.Sender_Id)
                .Index(t => t.Photo_Id)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Photo_Id = c.Int(),
                        Receiver_Id = c.Int(),
                        Sender_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .ForeignKey("dbo.Users", t => t.Receiver_Id)
                .ForeignKey("dbo.Users", t => t.Sender_Id)
                .Index(t => t.Photo_Id)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        isUnread = c.Int(nullable: false),
                        TimePosted = c.DateTime(nullable: false),
                        Comment_Id = c.Int(),
                        Photo_Id = c.Int(),
                        Receiver_Id = c.Int(),
                        Sender_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.Comment_Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .ForeignKey("dbo.Users", t => t.Receiver_Id)
                .ForeignKey("dbo.Users", t => t.Sender_Id)
                .Index(t => t.Comment_Id)
                .Index(t => t.Photo_Id)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Sender_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "Sender_Id", "dbo.Users");
            DropForeignKey("dbo.Notifications", "Receiver_Id", "dbo.Users");
            DropForeignKey("dbo.Notifications", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.Notifications", "Comment_Id", "dbo.Comments");
            DropForeignKey("dbo.Likes", "Sender_Id", "dbo.Users");
            DropForeignKey("dbo.Likes", "Receiver_Id", "dbo.Users");
            DropForeignKey("dbo.Likes", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.Comments", "Sender_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "Receiver_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "Photo_Id", "dbo.Photos");
            DropIndex("dbo.Notifications", new[] { "Sender_Id" });
            DropIndex("dbo.Notifications", new[] { "Receiver_Id" });
            DropIndex("dbo.Notifications", new[] { "Photo_Id" });
            DropIndex("dbo.Notifications", new[] { "Comment_Id" });
            DropIndex("dbo.Likes", new[] { "Sender_Id" });
            DropIndex("dbo.Likes", new[] { "Receiver_Id" });
            DropIndex("dbo.Likes", new[] { "Photo_Id" });
            DropIndex("dbo.Comments", new[] { "Sender_Id" });
            DropIndex("dbo.Comments", new[] { "Receiver_Id" });
            DropIndex("dbo.Comments", new[] { "Photo_Id" });
            DropTable("dbo.Notifications");
            DropTable("dbo.Likes");
            DropTable("dbo.Comments");
        }
    }
}
