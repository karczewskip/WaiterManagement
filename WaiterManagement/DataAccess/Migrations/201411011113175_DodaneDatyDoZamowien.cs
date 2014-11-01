namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodaneDatyDoZamowien : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PlacingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "ClosingDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ClosingDate");
            DropColumn("dbo.Orders", "PlacingDate");
        }
    }
}
