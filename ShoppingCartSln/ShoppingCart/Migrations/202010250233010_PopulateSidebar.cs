namespace ShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateSidebar : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Sidebars (Body) VALUES ('Sidebar Body')");
        }
        
        public override void Down()
        {
        }
    }
}
