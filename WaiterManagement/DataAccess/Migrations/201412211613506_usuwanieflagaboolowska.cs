namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usuwanieflagaboolowska : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItemCategories", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.MenuItemQuantities", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.MenuItems", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tables", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.WaiterContexts", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaiterContexts", "IsDeleted");
            DropColumn("dbo.Tables", "IsDeleted");
            DropColumn("dbo.Orders", "IsDeleted");
            DropColumn("dbo.MenuItems", "IsDeleted");
            DropColumn("dbo.MenuItemQuantities", "IsDeleted");
            DropColumn("dbo.MenuItemCategories", "IsDeleted");
        }
    }
}
