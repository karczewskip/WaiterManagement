namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Zmiana1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuItemCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Price_Amount = c.Single(nullable: false),
                        Price_Currency = c.String(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuItemCategories", t => t.Category_Id)
                .Index(t => t.Category_Id);

            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        WaiterId = c.Int(nullable: false),
                        TableId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuItemQuantities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MenuItemId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Order_Id);

            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.WaiterContexts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuItemQuantities", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.MenuItems", "Category_Id", "dbo.MenuItemCategories");
            DropIndex("dbo.MenuItemQuantities", new[] { "Order_Id" });
            DropIndex("dbo.MenuItems", new[] { "Category_Id" });
            DropTable("dbo.WaiterContexts");
            DropTable("dbo.Tables");
            DropTable("dbo.MenuItemQuantities");
            DropTable("dbo.Orders");
            DropTable("dbo.MenuItems");
            DropTable("dbo.MenuItemCategories");
        }
    }
}
