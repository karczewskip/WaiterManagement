namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dopelnienieModelu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Table_Id", c => c.Int());
            AddColumn("dbo.Orders", "Waiter_Id", c => c.Int());
            AddColumn("dbo.MenuItemQuantities", "MenuItem_Id", c => c.Int());
            CreateIndex("dbo.Orders", "Table_Id");
            CreateIndex("dbo.Orders", "Waiter_Id");
            CreateIndex("dbo.MenuItemQuantities", "MenuItem_Id");
            AddForeignKey("dbo.MenuItemQuantities", "MenuItem_Id", "dbo.MenuItems", "Id");
            AddForeignKey("dbo.Orders", "Table_Id", "dbo.Tables", "Id");
            AddForeignKey("dbo.Orders", "Waiter_Id", "dbo.WaiterContexts", "Id");
            DropColumn("dbo.Orders", "WaiterId");
            DropColumn("dbo.Orders", "TableId");
            DropColumn("dbo.MenuItemQuantities", "MenuItemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MenuItemQuantities", "MenuItemId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "TableId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "WaiterId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Orders", "Waiter_Id", "dbo.WaiterContexts");
            DropForeignKey("dbo.Orders", "Table_Id", "dbo.Tables");
            DropForeignKey("dbo.MenuItemQuantities", "MenuItem_Id", "dbo.MenuItems");
            DropIndex("dbo.MenuItemQuantities", new[] { "MenuItem_Id" });
            DropIndex("dbo.Orders", new[] { "Waiter_Id" });
            DropIndex("dbo.Orders", new[] { "Table_Id" });
            DropColumn("dbo.MenuItemQuantities", "MenuItem_Id");
            DropColumn("dbo.Orders", "Waiter_Id");
            DropColumn("dbo.Orders", "Table_Id");
        }
    }
}
