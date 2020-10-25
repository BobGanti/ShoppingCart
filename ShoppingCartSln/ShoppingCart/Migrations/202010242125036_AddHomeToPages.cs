namespace ShoppingCart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHomeToPages : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Pages (Title, Slink, HasSidebar, Sorting, Body) VALUES ('Home', 'home', 0, 0, 'Home Page Body')");
        }
        
        public override void Down()
        {
        }
    }
}
