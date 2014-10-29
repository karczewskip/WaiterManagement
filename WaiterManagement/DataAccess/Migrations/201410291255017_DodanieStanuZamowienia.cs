namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieStanuZamowienia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "State");
        }
    }
}
