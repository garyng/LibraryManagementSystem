namespace Libraryman.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthorBook",
                c => new
                    {
                        AuthorId = c.Int(nullable: false),
                        BookBarcode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AuthorId, t.BookBarcode })
                .ForeignKey("dbo.Author", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Book", t => t.BookBarcode, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.BookBarcode);
            
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        PublisherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Publisher", t => t.PublisherId, cascadeDelete: true)
                .Index(t => t.PublisherId);
            
            CreateTable(
                "dbo.Publisher",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        ContactNumber = c.String(maxLength: 15, storeType: "nvarchar"),
                        Address = c.String(maxLength: 500, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Barcode = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255, storeType: "nvarchar"),
                        Edition = c.String(maxLength: 255, storeType: "nvarchar"),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(maxLength: 2000, storeType: "nvarchar"),
                        PublishedYear = c.String(maxLength: 5, storeType: "nvarchar"),
                        Status = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        PublisherId = c.Int(nullable: false),
                        LibraryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Barcode)
                .ForeignKey("dbo.Library", t => t.LibraryId, cascadeDelete: true)
                .ForeignKey("dbo.Publisher", t => t.PublisherId, cascadeDelete: true)
                .ForeignKey("dbo.BookType", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId)
                .Index(t => t.PublisherId)
                .Index(t => t.LibraryId);
            
            CreateTable(
                "dbo.Library",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        Location = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Staff",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        Gender = c.Int(nullable: false),
                        PhoneNumber = c.String(maxLength: 15, storeType: "nvarchar"),
                        PasswordHash = c.String(maxLength: 256, storeType: "nvarchar"),
                        LastLogin = c.DateTime(nullable: false, precision: 0),
                        LibraryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Library", t => t.LibraryId, cascadeDelete: true)
                .Index(t => t.LibraryId);
            
            CreateTable(
                "dbo.Record",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false, precision: 0),
                        UserId = c.Int(nullable: false),
                        StaffId = c.Int(nullable: false),
                        BookBarcode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Book", t => t.BookBarcode, cascadeDelete: true)
                .ForeignKey("dbo.Staff", t => t.StaffId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.StaffId)
                .Index(t => t.BookBarcode);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        Gender = c.Int(nullable: false),
                        PhoneNumber = c.String(maxLength: 15, storeType: "nvarchar"),
                        Email = c.String(maxLength: 255, storeType: "nvarchar"),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MembershipType", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.BorrowedBook",
                c => new
                    {
                        RecordId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        BookBarcode = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.RecordId, t.UserId, t.BookBarcode })
                .ForeignKey("dbo.Book", t => t.BookBarcode, cascadeDelete: true)
                .ForeignKey("dbo.Record", t => t.RecordId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RecordId)
                .Index(t => t.UserId)
                .Index(t => t.BookBarcode);
            
            CreateTable(
                "dbo.MembershipType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        Duration = c.Time(nullable: false, precision: 0),
                        MaximumBooks = c.Int(nullable: false),
                        OverdueFine = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        IsBorrowable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthorBook", "BookBarcode", "dbo.Book");
            DropForeignKey("dbo.AuthorBook", "AuthorId", "dbo.Author");
            DropForeignKey("dbo.Book", "TypeId", "dbo.BookType");
            DropForeignKey("dbo.Book", "PublisherId", "dbo.Publisher");
            DropForeignKey("dbo.User", "Type_Id", "dbo.MembershipType");
            DropForeignKey("dbo.Record", "UserId", "dbo.User");
            DropForeignKey("dbo.BorrowedBook", "UserId", "dbo.User");
            DropForeignKey("dbo.BorrowedBook", "RecordId", "dbo.Record");
            DropForeignKey("dbo.BorrowedBook", "BookBarcode", "dbo.Book");
            DropForeignKey("dbo.Record", "StaffId", "dbo.Staff");
            DropForeignKey("dbo.Record", "BookBarcode", "dbo.Book");
            DropForeignKey("dbo.Staff", "LibraryId", "dbo.Library");
            DropForeignKey("dbo.Book", "LibraryId", "dbo.Library");
            DropForeignKey("dbo.Author", "PublisherId", "dbo.Publisher");
            DropIndex("dbo.BorrowedBook", new[] { "BookBarcode" });
            DropIndex("dbo.BorrowedBook", new[] { "UserId" });
            DropIndex("dbo.BorrowedBook", new[] { "RecordId" });
            DropIndex("dbo.User", new[] { "Type_Id" });
            DropIndex("dbo.Record", new[] { "BookBarcode" });
            DropIndex("dbo.Record", new[] { "StaffId" });
            DropIndex("dbo.Record", new[] { "UserId" });
            DropIndex("dbo.Staff", new[] { "LibraryId" });
            DropIndex("dbo.Book", new[] { "LibraryId" });
            DropIndex("dbo.Book", new[] { "PublisherId" });
            DropIndex("dbo.Book", new[] { "TypeId" });
            DropIndex("dbo.Author", new[] { "PublisherId" });
            DropIndex("dbo.AuthorBook", new[] { "BookBarcode" });
            DropIndex("dbo.AuthorBook", new[] { "AuthorId" });
            DropTable("dbo.BookType");
            DropTable("dbo.MembershipType");
            DropTable("dbo.BorrowedBook");
            DropTable("dbo.User");
            DropTable("dbo.Record");
            DropTable("dbo.Staff");
            DropTable("dbo.Library");
            DropTable("dbo.Book");
            DropTable("dbo.Publisher");
            DropTable("dbo.Author");
            DropTable("dbo.AuthorBook");
        }
    }
}
