namespace Libraryman.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReturnedBook : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReturnedBook",
                c => new
                    {
                        RecordId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        BookBarcode = c.Int(nullable: false),
                        BorrowingRecordId = c.Int(nullable: false),
                        OverdueFine = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.RecordId, t.UserId, t.BookBarcode })
                .ForeignKey("dbo.Book", t => t.BookBarcode, cascadeDelete: true)
                .ForeignKey("dbo.Record", t => t.BorrowingRecordId, cascadeDelete: true)
                .ForeignKey("dbo.Record", t => t.RecordId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RecordId)
                .Index(t => t.UserId)
                .Index(t => t.BookBarcode)
                .Index(t => t.BorrowingRecordId);
            
            AlterColumn("dbo.Book", "ISBN", c => c.String(maxLength: 20, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReturnedBook", "UserId", "dbo.User");
            DropForeignKey("dbo.ReturnedBook", "RecordId", "dbo.Record");
            DropForeignKey("dbo.ReturnedBook", "BorrowingRecordId", "dbo.Record");
            DropForeignKey("dbo.ReturnedBook", "BookBarcode", "dbo.Book");
            DropIndex("dbo.ReturnedBook", new[] { "BorrowingRecordId" });
            DropIndex("dbo.ReturnedBook", new[] { "BookBarcode" });
            DropIndex("dbo.ReturnedBook", new[] { "UserId" });
            DropIndex("dbo.ReturnedBook", new[] { "RecordId" });
            AlterColumn("dbo.Book", "ISBN", c => c.String(maxLength: 15, storeType: "nvarchar"));
            DropTable("dbo.ReturnedBook");
        }
    }
}
