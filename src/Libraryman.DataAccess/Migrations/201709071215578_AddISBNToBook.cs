namespace Libraryman.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddISBNToBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Book", "ISBN", c => c.String(maxLength: 15, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Book", "ISBN");
        }
    }
}
